﻿namespace Blinds.Web.Areas.Public.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Transactions;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;
    using Common;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Microsoft.AspNet.Identity;
    using Proxies;
    using Web.Models;

    public class OrdersModel : PublicModel, IModel<bool>
    {
        public int Id { get; set; }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public int BlindTypeId { get; set; }

        [Required( ErrorMessage = GlobalConstants.OrderNumberRequireText)]
        [RegularExpression("^[0-9]+$", ErrorMessage = GlobalConstants.OrderNumberRegex)]
        [MaxLength(40, ErrorMessage = GlobalConstants.OrderNumberMaxLength)]
        [DisplayName(GlobalConstants.OrderNumberDisplayText)]
        public string Number { get; set; }

        public bool HasAgreedTerms { get; set; }

        public Control Control { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsInstalation)]
        public InstalationType InstalationType { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        [DisplayName(GlobalConstants.OrderRailDisplayText)]
        public Color RailColor { get; set; }

        public IEnumerable<SelectListItem> RailColors { get; set; }

        [DisplayName(GlobalConstants.OrderColorDisplayText)]
        public Color FabricAndLamelColor { get; set; }

        public IEnumerable<SelectListItem> FabricAndLamelColors { get; set; }

        [DisplayName(GlobalConstants.OrderMaterialDisplayText)]
        public Material FabricAndLamelMaterial { get; set; }

        public IEnumerable<SelectListItem> FabricAndLamelMaterials { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public IEnumerable<OrderProxy> MyOrders { get; set; }

        public void Init(bool init)
        {
            this.Init();

            if (init)
            {
                var blindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                blindTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = GlobalConstants.OrderTypeDefaultDisplayText });

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

        public OrdersModel GetMyOrders(string userId)
        {
            if (userId == null)
            {
                userId = HttpContext.Current.User.Identity.GetUserId();
            }

            var repo = this.RepoFactory.Get<OrderRepository>();
            this.MyOrders = repo.GetByUserId(userId)
                .Project()
                .To<OrderProxy>()
                .ToList();

            return this;
        }

        public object Save(OrderProxy proxy, ModelStateDictionary modelState)
        {
            int entityId;

            if (proxy != null && modelState.IsValid)
            {
                try
                {
                    if (proxy.Blinds == null || proxy.Blinds.Any(b => b.Count == 0))
                    {
                        return GlobalConstants.OrderBlindCountErrorMessage;
                    }

                    var repo = this.RepoFactory.Get<OrderRepository>();

                    var loggedUserId = HttpContext.Current.User.Identity.GetUserId();
                    var numberPostfix = "__" + loggedUserId.Substring(0, 12);
                    var orderNumber = proxy.OrderNumber + numberPostfix;

                    var numberExist = repo.GetActive().Where(x => x.Number == orderNumber && x.UserId == loggedUserId).Any();

                    if (numberExist)
                    {
                        return GlobalConstants.OrderNumberExistsMessage;
                    }

                    var railRepo = this.RepoFactory.Get<RailRepository>();
                    var rail = railRepo.Get(proxy.BlindTypeId, (Color)proxy.RailColorId);

                    if (rail == null)
                    {
                        return GlobalConstants.GeneralRailError;
                    }

                    var fabricRepo = this.RepoFactory.Get<FabricAndLamelRepository>();
                    var fabricAndLamel = fabricRepo.Get(proxy.BlindTypeId, (Color)proxy.FabricAndLamelColorId, (Material)proxy.FabricAndLamelMaterialId);

                    if (fabricAndLamel == null)
                    {
                        return GlobalConstants.GeneralMaterialError;
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
                                totalArea += (blind.Width * blind.Height) / 1000000;

                                blinds.Add(newBlind);
                                blindrepo.SaveChanges();
                            }
                        }

                        var componentRepo = this.RepoFactory.Get<ComponentRepository>();
                        var components = componentRepo.GetByBlindType(proxy.BlindTypeId).ToList();

                        fabricAndLamel.Quantity -= totalArea;
                        rail.Quantity -= totalWidth;

                        entity.TotalPrice = this.GetComponentPrice(proxy, components) + (totalWidth * rail.Price) + (totalArea * fabricAndLamel.Price);
                        entity.ExpeditionDate = this.CalculateManufactireDays(components, rail.Quantity, fabricAndLamel.Quantity, totalArea);
                        entity.Blinds = blinds;

                        repo.SaveChanges();
                        railRepo.SaveChanges();
                        fabricRepo.SaveChanges();
                        componentRepo.SaveChanges();
                        entityId = entity.Id;
                        transaction.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    return this.HandleDbEntityValidationException(e);
                }

                return new
                {
                    Id = entityId
                };
            }
            else
            {
                return this.HandleErrors(modelState);
            }
        }

        private DateTime CalculateManufactireDays(List<Data.Models.Component> components, decimal railQuantity, decimal fabricAndLamelQuantity, decimal totalArea)
        {
            int totalDays = (int)(2 + 2 + (totalArea / 50));

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
                            var wide = blind.Width < 1000 ? 1000 : blind.Width;
                            expence = component.DefaultAmount * ((blind.Height * wide) / 1000000);
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