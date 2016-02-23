namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    using Web.Models;

    public class OrderModel : AdminModel, IModel<bool>, IMapFrom<Order>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsTitle)]
        public string Number { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsDate)]
        public DateTime OrderDate { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsExpeditionDate)]
        public DateTime ExpeditionDate { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsInstalation)]
        [UIHint("EnumTemplate")]
        public InstalationType InstalationType { get; set; }

        [Required(ErrorMessage = GlobalConstants.ColorRequireText)]
        [DisplayName(GlobalConstants.ColorDisplay)]
        [UIHint("EnumTemplate")]
        public Color Color { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal TotalPrice { get; set; }

        [DisplayName(GlobalConstants.OrderBlindCountText)]
        public int BlindsCount { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsOwner)]
        public string UserName { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

        public void Init(bool init)
        {
            this.Init();

            if (init)
            {
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

        public IEnumerable<OrderModel> Get()
        {
            return this.RepoFactory.Get<OrderRepository>().GetActive()
                .Project()
                .To<OrderModel>()
                .ToList();
        }

        public DataSourceResult Delete(OrderModel viewModel)
        {
            var repo = this.RepoFactory.Get<OrderRepository>();
            var entity = repo.GetById(viewModel.Id);

            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;

            repo.SaveChanges();
            return null;
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(z => z.User.FirstName + " " + z.User.LastName));

            configuration.CreateMap<Order, OrderModel>()
                .ForMember(x => x.BlindsCount, opt => opt.MapFrom(z => z.Blinds.Count));
        }
    }
}