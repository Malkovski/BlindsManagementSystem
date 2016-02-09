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
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using System.Web;

    public class OrdersModel : MenuModel, IModel<bool>, IMapFrom<Order>
    {
        public OrdersModel()
        {
            this.Blinds = new HashSet<BlindsModel>();
        }

        public int Id { get; set; }

        public int BlindTypeId { get; set; }

        public virtual BlindType BlindType { get; set; }

        public string Number { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public int BlindsCount { get; set; }

        public Color Color { get; set; }

        public Control Control { get; set; }

        public InstalationType InstalationType { get; set; }

        public string ColorName
        {
            get
            {
                return this.Color.GetDescription();
            }
        }

        public string InstalationName
        {
            get
            {
                return this.InstalationType.GetDescription();
            }
        }

        public DateTime OrderDate { get; set; }

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
           
            return repo.GetActive().Project().To<OrdersModel>().FirstOrDefault(x => x.Id == id);
        }

        public DataSourceResult Save(OrdersModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<OrderRepository>();

                var railRepo = this.RepoFactory.Get<RailRepository>();
                var currentRail = railRepo.All().Where(x => x.Color == viewModel.Color && x.BlindTypeId == viewModel.BlindTypeId).FirstOrDefault();
               
                var fabricAndLamelsRepo = this.RepoFactory.Get<FabricAndLamelRepository>();
                var currentFabricAndLamels = fabricAndLamelsRepo.All().Where(x => x.Color == viewModel.Color && x.BlindTypeId == viewModel.BlindTypeId).FirstOrDefault();

                var entity = new Order();

                repo.Add(entity);
                Mapper.Map(viewModel, entity);

                entity.Number = HttpContext.Current.User.Identity.GetUserName() + entity.Id;
                entity.OrderDate = DateTime.UtcNow;
                entity.Price = 0;
                entity.UserId = HttpContext.Current.User.Identity.GetUserId();

                for (int i = 0; i < viewModel.BlindsCount; i++)
                {
                    var blind = new Blind
                    {
                        BlindTypeId = viewModel.BlindTypeId,
                        Color = viewModel.Color,
                        Height = viewModel.Height,
                        Width = viewModel.Width,
                        Control = viewModel.Control,
                        OrderId = viewModel.Id,
                        RailId = currentRail.Id,
                        FabricAndLamelId = currentFabricAndLamels.Id
                    };

                    entity.Blinds.Add(blind);
                    entity.Price += DefinePrice(blind.Width, blind.Height, currentRail.Price, currentFabricAndLamels.Price);
                    currentRail.Quantity -= (long)blind.Width / 1000;
                    currentFabricAndLamels.Quantity -= ((long)blind.Width * (long)blind.Height) / 1000000; 
                }

                repo.SaveChanges();
                railRepo.SaveChanges();
                fabricAndLamelsRepo.SaveChanges();
                viewModel.Id = entity.Id;
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        private decimal DefinePrice(decimal width, decimal heigth, decimal railPrice, decimal materialPrice)
        {
            var railCost = (width * railPrice) / 1000;
            var materialCost = (width * heigth * materialPrice) / 1000000;

            return railCost + materialCost;
        }
    }
}