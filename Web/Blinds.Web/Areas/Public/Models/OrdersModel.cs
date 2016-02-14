namespace Blinds.Web.Areas.Public.Models
{
    using Data.Models;
    using Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Web.Models;
    using Data.Repositories;
    using System.Web.Mvc;
    using Data.Models.Enumerations;
    using Common;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Text;
    using Proxies;
    using System.ComponentModel;
    using System.Transactions;
    public class OrdersModel : MenuModel, IModel<bool>
    {
        public OrdersModel()
        {
            this.Blinds = new HashSet<BlindsModel>();
        }

        public int Id { get; set; }

        public int BlindTypeId { get; set; }

        [Required( ErrorMessage = GlobalConstants.OrderNumberRequireText)]
        [RegularExpression("^[0-9]+$", ErrorMessage = GlobalConstants.OrderNumberRegex)]
        [MaxLength(40, ErrorMessage = GlobalConstants.OrderNumberMaxLength)]
        public string Number { get; set; }

        public Control Control { get; set; }

        public InstalationType InstalationType { get; set; }

        public virtual ICollection<BlindsModel> Blinds { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        [DisplayName("Цвят на релсата")]
        public Color RailColor { get; set; }

        public IEnumerable<SelectListItem> RailColors { get; set; }

        [DisplayName("Цвят на щората")]
        public Color FabricAndLamelColor { get; set; }

        public IEnumerable<SelectListItem> FabricAndLamelColors { get; set; }

        [DisplayName("Материал")]
        public Material FabricAndLamelMaterial { get; set; }

        public IEnumerable<SelectListItem> FabricAndLamelMaterials { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public void Init(bool init)
        {
            base.Init();

            if (init)
            {
                var blindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                blindTypes.Insert(0, new SelectListItem { Value = "", Text = "Select one" });

                this.BlindTypes = blindTypes;

                this.InstalationTypes = Enum.GetValues(typeof(InstalationType)).Cast<InstalationType>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();

                this.RailColors = new List<SelectListItem>();
                this.FabricAndLamelColors = new List<SelectListItem>();
                this.FabricAndLamelMaterials = new List<SelectListItem>();
            }
        }

        public List<SelectListItem> GetRailColors(int blindTypeId)
        {
            var colors = this.RepoFactory.Get<RailRepository>().GetAll().Where(r => r.BlindTypeId == blindTypeId).Select(v => new ColorProxy
            {
                Color = v.Color,
                Value = (int)v.Color
            }).ToList();

            return this.ColorToSelectedListItems(colors);
        }

        public List<SelectListItem> GetFabricAndLamelColors(int blindTypeId)
        {
            var colors = this.RepoFactory.Get<FabricAndLamelRepository>().GetAll().Where(r => r.BlindTypeId == blindTypeId).Select(v => new ColorProxy
            {
                Color = v.Color,
                Value = (int)v.Color
            })
            .ToList();

            return this.ColorToSelectedListItems(colors);
        }

        public List<SelectListItem> GetFabricAndLamelMaterials(int colorId, int blindTypeId)
        {
            var color = (Color)colorId;
            var materials = this.RepoFactory.Get<FabricAndLamelRepository>().GetAll().Where(r => r.BlindTypeId == blindTypeId && r.Color == color).Select(v => new MaterialProxy
            {
                Material = v.Material,
                Value = (int)v.Material
            }).ToList();


            var result = new List<SelectListItem>();

            materials.ForEach(c =>
            {
                if (!result.Any(r => r.Text == c.MaterialName))
                {
                    result.Add(new SelectListItem
                    {
                        Text = c.MaterialName,
                        Value = c.Value.ToString()
                    });
                }

            });

            return result;
        }

        public OrdersModel GetDetails(int id)
        {
            var repo = this.RepoFactory.Get<OrderRepository>();
            var model = repo.GetActive()
                .To<OrdersModel>()
                .FirstOrDefault(x => x.Id == id);

            model.BlindCategories = this.RepoFactory.Get<BlindTypeRepository>()
                .GetActive()
                .To<ProductsModel>()
                .ToList();

            return model;
        }

        public IEnumerable<OrdersModel> GetMyOrders(string userId)
        {
            if (userId == null)
            {
                userId = HttpContext.Current.User.Identity.GetUserId();
            }

            var repo = this.RepoFactory.Get<OrderRepository>();
            return repo.GetActive()
                .Where(x => x.UserId == userId)
                .To<OrdersModel>()
                .ToList();
        }

        public DataSourceResult Save(OrderProxy proxy, ModelStateDictionary modelState)
        {
            if (proxy.Blinds.Any(b => b.Count == 0))
            {
                return new DataSourceResult
                {
                    Errors = GlobalConstants.OrderBlindCountErrorMessage
                };
            }

            if (proxy != null && modelState.IsValid)
            {
                try
                {
                    var repo = this.RepoFactory.Get<OrderRepository>();
                    var loggedUserId = HttpContext.Current.User.Identity.GetUserId();
                    var numberPostfix = "__" + loggedUserId.Substring(0, 12);
                    var orderNumber = proxy.OrderNumber + numberPostfix;

                    var numberExist = repo.GetActive().Where(x => x.Number == (orderNumber) && x.UserId == loggedUserId).Any();

                    if (numberExist)
                    {
                        return new DataSourceResult
                        {
                            Errors = GlobalConstants.OrderNumberExistsMessage
                        };
                    }

                    var rail = this.RepoFactory.Get<RailRepository>().Get(proxy.BlindTypeId, (Color)proxy.RailColorId);

                    if (rail == null)
                    {
                        return new DataSourceResult
                        {
                            Errors = "Rail error"
                        };
                    }

                    var fabricAndLamel = this.RepoFactory.Get<FabricAndLamelRepository>().Get(proxy.BlindTypeId, (Color)proxy.FabricAndLamelColorId, (Material)proxy.FabricAndLamelMaterialId);

                    if (fabricAndLamel == null)
                    {
                        return new DataSourceResult
                        {
                            Errors = "fabricAndLamels error"
                        };
                    }


                    Blind newBlind;
                    List<Blind> blinds = new List<Blind>();

                    decimal totalWidth = 0;
                    decimal totalArea = 0;

                    using (var transaction = new TransactionScope())
                    {
                        var entity = new Order()
                        {
                            Number = orderNumber,
                            BlindTypeId = proxy.BlindTypeId,
                            RailId = rail.Id,
                            FabricAndLamelId = fabricAndLamel.Id,
                            InstalationType = (InstalationType)proxy.InstalationTypeId,
                            UserId = loggedUserId,
                            OrderDate = DateTime.Now,
                            ExpeditionDate = DateTime.Now
                        };

                        repo.Add(entity);
                        repo.SaveChanges();

                        proxy.Id = entity.Id;

                        var blindrepo = this.RepoFactory.Get<BlindRepository>();
                        foreach (var blind in proxy.Blinds)
                        {
                            for (int i = 0; i < blind.Count; i++)
                            {
                                newBlind = new Blind
                                {
                                    Width = blind.Width,
                                    Height = blind.Height,
                                    Control = (Control)blind.Control,
                                    OrderId = entity.Id
                                };

                                totalWidth += blind.Width / 1000;
                                totalArea += blind.Width * blind.Height / 1000000;

                                blinds.Add(newBlind);
                                blindrepo.Add(newBlind);
                                blindrepo.SaveChanges();
                            }
                        }

                        var components = this.RepoFactory.Get<ComponentRepository>().GetByBlindType(proxy.BlindTypeId).ToList();

                        fabricAndLamel.Quantity -= totalArea;
                        rail.Quantity -= totalWidth;


                        entity.TotalPrice = this.GetComponentPrice(proxy, components) + totalWidth * rail.Price + totalArea * fabricAndLamel.Price;
                        entity.ExpeditionDate = this.CalculateManufactireDays(components, rail.Quantity, fabricAndLamel.Quantity, totalArea);
                        entity.Blinds = blinds;

                        repo.SaveChanges();

                        transaction.Complete();
                    }

                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        builder.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            builder.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }

                    return new DataSourceResult
                    {
                        Errors = builder.ToString()
                    };
                }
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        private DateTime CalculateManufactireDays(List<Data.Models.Component> components, decimal railQuantity, decimal fabricAndLamelQuantity, decimal totalArea)
        {
            int totalDays = (int)(2 + 2 + totalArea / 50);

            if (components.Any(c => c.Quantity <= 0))
            {
                totalDays += 3;
            }

            if (railQuantity <= 0)
            {
                totalDays += 5;
            }

            if (fabricAndLamelQuantity <= 0)
            {
                totalDays += 7;
            }

            return DateTime.Now.AddDays(totalDays);
        }

        private decimal GetComponentPrice(OrderProxy proxy, List<Data.Models.Component> components)
        {
            decimal totalPrice = 0;
            decimal expence;

            foreach (var blind in proxy.Blinds)
            {
                for (int i = 0; i < blind.Count; i++)
                {
                    foreach (var component in components)
                    {
                        if (component.HeigthBased && component.WidthBased)
                        {
                            var wide = blind.Width < 1000 ? 1000 : (blind.Width / 1000);
                            expence = (blind.Height * (component.DefaultAmount * wide)) / 1000;
                        }
                        else if (component.HeigthBased)
                        {
                            expence = (component.DefaultAmount * blind.Height) / 1000;

                        }
                        else if (component.WidthBased)
                        {
                            expence = (component.DefaultAmount * blind.Width) / 1000;
                        }
                        else
                        {
                            expence = component.DefaultAmount;
                        }

                        totalPrice += expence * component.Price;
                        component.Quantity -= expence;
                    }
                }
            }

            return totalPrice;
        }

        private List<SelectListItem> ColorToSelectedListItems(List<ColorProxy> colors)
        {
            var result = new List<SelectListItem>();

            colors.ForEach(c =>
            {
                if (!result.Any(r => r.Text == c.ColorName))
                {
                    result.Add(new SelectListItem
                    {
                        Text = c.ColorName,
                        Value = c.Value.ToString()
                    });
                }

            });

            return result;
        }

    }
}