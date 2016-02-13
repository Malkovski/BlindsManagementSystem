﻿namespace Blinds.Web.Areas.Public.Models
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
    using AutoMapper;
    using Microsoft.AspNet.Identity;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Text;

    public class OrdersModel : MenuModel, IModel<bool>, IMapFrom<Order>, IMapTo<Order>
    {
        public OrdersModel()
        {
            this.Blinds = new HashSet<BlindsModel>();
        }

        public int Id { get; set; }

        public int BlindTypeId { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required( ErrorMessage = GlobalConstants.OrderNumberRequireText)]
        [RegularExpression("^[0-9]+$", ErrorMessage = GlobalConstants.OrderNumberRegex)]
        [MaxLength(40, ErrorMessage = GlobalConstants.OrderNumberMaxLength)]
        public string Number { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public int BlindsCount { get; set; }

        public Color Color { get; set; }

        public Control Control { get; set; }

        public InstalationType InstalationType { get; set; }

        public string ColorName { get { return this.Color.GetDescription(); } }

        public string InstalationName { get { return this.InstalationType.GetDescription(); } }

        public DateTime OrderDate { get; set; }

        public int ManufactureDays { get; set; }

        public DateTime ExpeditionDate { get; set; }

        public bool RailInStock { get; set; }

        public bool FabricAndLamelInStock { get; set; }

        public bool ComponentsInStock { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<BlindsModel> Blinds { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public void Init(bool init)
        {
            base.Init();

            if (init)
            {
                this.BlindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                this.Colors = Enum.GetValues(typeof(Color)).Cast<Color>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();

                this.InstalationTypes = Enum.GetValues(typeof(InstalationType)).Cast<InstalationType>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();
            }
        }

        public OrdersModel GetById(int id)
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

        public IEnumerable<OrdersModel> GetByUserId(string userId)
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

        public DataSourceResult Save(OrdersModel viewModel, ModelStateDictionary modelState)
        {
            var available = this.SupplyCheck(viewModel.BlindTypeId);
            if (!available)
            {
                return new DataSourceResult
                {
                    Errors = GlobalConstants.OrderAvailableMaterialsErrorMessage
                };
            }

            if (viewModel.BlindsCount < 1)
            {
                return new DataSourceResult
                {
                    Errors = GlobalConstants.OrderBlindCountErrorMessage
                };
            }

            var loggedUserId = HttpContext.Current.User.Identity.GetUserId();
            var numberPostfix = "__" + loggedUserId.Substring(0, 12);

            if (viewModel != null && modelState.IsValid)
            {
                try
                {
                    var repo = this.RepoFactory.Get<OrderRepository>();
                    var numberExist = repo.GetActive().Where(x => x.Number == (viewModel.Number + numberPostfix) && x.UserId == loggedUserId).Any();

                    if (numberExist)
                    {
                        return new DataSourceResult
                        {
                            Errors = GlobalConstants.OrderNumberExistsMessage
                        };
                    }

                    var entity = new Order();
                    repo.Add(entity);

                    entity.Number = viewModel.Number + numberPostfix;
                    entity.OrderDate = DateTime.UtcNow;
                    entity.ExpeditionDate = DateTime.UtcNow;
                    entity.UserId = loggedUserId;
                    entity.Price = 0;

                    repo.SaveChanges();

                    viewModel.Id = entity.Id;
                    viewModel.RailInStock = true;
                    viewModel.FabricAndLamelInStock = true;
                    viewModel.ComponentsInStock = true;

                    //create the blinds needed
                    ICollection<Blind> blinds = this.AssembleBlinds(viewModel);

                    this.CalculateManufactireDays(viewModel);

                    decimal blindsPrice = this.DefineOrderPrice(blinds);
                    entity.ExpeditionDate = DateTime.UtcNow.AddDays(viewModel.ManufactureDays);
                    // Here is where you get rich!!!!!!!!!!!!!!
                    // + 100% + 200% - CHOOSEE!!!!!!!!!
                    entity.Price = blindsPrice;

                    repo.SaveChanges();
                    viewModel.Id = entity.Id;
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

        private bool SupplyCheck(int blindTypeId)
        {
            var railRepo = this.RepoFactory.Get<RailRepository>();
            var hasRail = railRepo.All().Any(x => x.BlindTypeId == blindTypeId);
            var componentRepo = this.RepoFactory.Get<ComponentRepository>();
            var hasComponent = componentRepo.All().Any(x => x.BlindTypeId == blindTypeId);
            var fabricAndLamelsRepo = this.RepoFactory.Get<FabricAndLamelRepository>();
            var hasMaterial = fabricAndLamelsRepo.All().Any(x => x.BlindTypeId == blindTypeId);

            return hasRail && hasComponent && hasMaterial;
        }

        private ICollection<Blind> AssembleBlinds(OrdersModel viewModel)
        {
            ICollection<Blind> blinds = new List<Blind>();
            var blindRepo = this.RepoFactory.Get<BlindRepository>();
            var blindExpenceRepo = this.RepoFactory.Get<ConsumedMaterialsRepository>();

            for (int i = 0; i < viewModel.BlindsCount; i++)
            {
                Blind blind = new Blind();

                var supply = Supply(viewModel);

                blind.BlindTypeId = viewModel.BlindTypeId;
                blind.Color = viewModel.Color;
                blind.Height = viewModel.Height;
                blind.Width = viewModel.Width;
                blind.Control = viewModel.Control;
                blind.RailId = supply.RailId;
                blind.FabricAndLamelId = supply.FabricAndLamelId;
                blind.ConsumedMaterials = supply;
                blind.OrderId = viewModel.Id;
                blind.Price = this.DefineBlindPrice(supply);
                blind.Id = supply.Id;
                supply.Blind = blind;

                blindRepo.Add(blind);
                blindRepo.SaveChanges();
                blindExpenceRepo.SaveChanges();

                blinds.Add(blind);
            }

            return blinds;
        }

        private ConsumedMaterials Supply(OrdersModel viewModel)
        {
            var blindExpenceRepo = this.RepoFactory.Get<ConsumedMaterialsRepository>();
            ConsumedMaterials consumedMaterials = new ConsumedMaterials();


            var currentRailId = this.RailSupply(viewModel, consumedMaterials);
            var currentFabricAndLamelsId = this.FabricAndLamelSupply(viewModel, consumedMaterials);

            blindExpenceRepo.Add(consumedMaterials);
            blindExpenceRepo.SaveChanges();

            var currentComponents = this.ComponentSupply(viewModel, consumedMaterials);
            consumedMaterials.ComponentsExpence = currentComponents;

            blindExpenceRepo.SaveChanges();
            return consumedMaterials;
        }

        private int RailSupply(OrdersModel viewModel, ConsumedMaterials consumedMaterials)
        {
            var railRepo = this.RepoFactory.Get<RailRepository>();
            var currentRail = railRepo.All().Where(x => x.Color == viewModel.Color && x.BlindTypeId == viewModel.BlindTypeId).FirstOrDefault();

            decimal expence = (viewModel.Width * viewModel.BlindsCount) / 1000;

            if (currentRail.Quantity < expence)
            {
                viewModel.RailInStock = false;
            }

            currentRail.Quantity -= expence;
            railRepo.SaveChanges();
            railRepo.Detach(currentRail);

            consumedMaterials.RailId = currentRail.Id;
            consumedMaterials.RailColor = currentRail.Color;
            consumedMaterials.RailExpence = expence;
            consumedMaterials.RailCost = (viewModel.Width * currentRail.Price) / 1000;

            return currentRail.Id;
        }

        private int FabricAndLamelSupply(OrdersModel viewModel, ConsumedMaterials consumedMaterials)
        {
            var fabricAndLamelsRepo = this.RepoFactory.Get<FabricAndLamelRepository>();
            var currentFabricAndLamels = fabricAndLamelsRepo.All().Where(x => x.Color == viewModel.Color && x.BlindTypeId == viewModel.BlindTypeId).FirstOrDefault();
            decimal expence = ((viewModel.Width * viewModel.Height) * viewModel.BlindsCount) / 1000000;

            if (currentFabricAndLamels.Quantity < expence)
            {
                viewModel.FabricAndLamelInStock = false;
            }

            currentFabricAndLamels.Quantity -= expence;
            fabricAndLamelsRepo.SaveChanges();
            fabricAndLamelsRepo.Detach(currentFabricAndLamels);

            consumedMaterials.FabricAndLamelId = currentFabricAndLamels.Id;
            consumedMaterials.FabricAndLamelColor = currentFabricAndLamels.Color;
            consumedMaterials.FabricAndLamelExpence = expence;
            consumedMaterials.FabricAndLamelCost = (viewModel.Width * viewModel.Height * currentFabricAndLamels.Price) / 1000000;

            return currentFabricAndLamels.Id;
        }

        private ICollection<ConsumedComponent> ComponentSupply(OrdersModel viewModel, ConsumedMaterials consumedMaterials)
        {
            ICollection<ConsumedComponent> components = new List<ConsumedComponent>();

            var repo = this.RepoFactory.Get<ComponentRepository>();
            var consumedRepo = this.RepoFactory.Get<ConsumedComponentRepository>();

            var parts = repo.SearchFor(x => x.BlindTypeId == viewModel.BlindTypeId).ToList();

            foreach (var part in parts)
            {
                ConsumedComponent current = new ConsumedComponent();
                current.ConsumedMaterialsId = consumedMaterials.Id;
                current.Name = part.Name;

                decimal expence = 0;
                if (part.HeigthBased && part.WidthBased)
                {
                    var wide = viewModel.Width < 1000 ? 1000 : (viewModel.Width / 1000);
                    expence = (viewModel.Height * (part.DefaultAmount * wide)) / 1000;
                    part.Quantity -= expence;

                    if (part.Quantity < expence)
                    {
                        viewModel.ComponentsInStock = false;
                    }
                    
                    current.Expence = expence;
                    current.Price = expence * part.Price;
                    components.Add(current);
                    consumedRepo.Add(current);
                }
                else if (part.HeigthBased)
                {
                    expence = (part.DefaultAmount * viewModel.Height) / 1000;
                    part.Quantity -= expence;

                    if (part.Quantity < expence)
                    {
                        viewModel.ComponentsInStock = false;
                    }
                    
                    current.Expence = expence;
                    current.Price = expence * part.Price;
                    components.Add(current);
                    consumedRepo.Add(current);
                }
                else if (part.WidthBased)
                {
                    expence = (part.DefaultAmount * viewModel.Width) / 1000;
                    part.Quantity -= expence;

                    if (part.Quantity < expence)
                    {
                        viewModel.ComponentsInStock = false;
                    }
                    
                    current.Expence = expence;
                    current.Price = expence * part.Price;
                    components.Add(current);
                    consumedRepo.Add(current);
                }
                else
                {
                    expence = part.DefaultAmount;
                    part.Quantity -= expence;

                    if (part.Quantity < expence)
                    {
                        viewModel.ComponentsInStock = false;
                    }
                    
                    current.Expence = expence;
                    current.Price = expence * part.Price;
                    components.Add(current);
                    consumedRepo.Add(current);
                }

                repo.SaveChanges();
                consumedRepo.SaveChanges();
            }

            return components;
        }


        private decimal DefineBlindPrice(ConsumedMaterials blindPartsExpence)
        {
            var bodyCost = blindPartsExpence.FabricAndLamelCost + blindPartsExpence.RailCost;
            decimal componentsCost = 0;

            foreach (var component in blindPartsExpence.ComponentsExpence)
            {
                componentsCost += (component.Price * component.Expence);
            }

            return bodyCost + componentsCost;
        }

        private decimal DefineOrderPrice(ICollection<Blind> blinds)
        {
            decimal totalCost = 0;
            foreach (var blind in blinds)
            {
                totalCost += blind.Price;
            }
            return totalCost;
        }

        private void CalculateManufactireDays(OrdersModel viewModel)
        {
            var squareMeters = ((int)(viewModel.BlindsCount * (viewModel.Height * viewModel.Width) / 1000000));
            viewModel.ManufactureDays = 2 + 2 + squareMeters / 50;

            if (!viewModel.ComponentsInStock)
            {
                viewModel.ManufactureDays += 3;
            }

            if (!viewModel.RailInStock)
            {
                viewModel.ManufactureDays += 5;
            }

            if (!viewModel.FabricAndLamelInStock)
            {
                viewModel.ManufactureDays += 7;
            }
        }
    }
}