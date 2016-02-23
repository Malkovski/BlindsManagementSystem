namespace Blinds.Web.Areas.Public.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Blinds.Data.Models;
    using Blinds.Web.Infrastructure.Mapping;
    using Common;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Microsoft.AspNet.Identity;
    using Web.Models;

    public class UserOrdersModel : PublicModel, IModel<bool>, IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Number { get; set; }

        [DisplayName(GlobalConstants.OrderNumberDisplayText)]
        public string OrderNumber
        {
            get
            {
                return this.Number.Split('_')[0];
            }
        }

        public Color Color { get; set; }

        [DisplayName(GlobalConstants.ColorDisplay)]
        public string ColorName
        {
            get
            {
                return this.Color.GetDescription();
            }
        }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public string BlindTypeName { get; set; }

        [DisplayName(GlobalConstants.OrderBlindCountText)]
        public int BlindCount { get; set; }

        public void Init(bool data)
        {
            this.Init();
        }

        public IEnumerable<UserOrdersModel> GetMine()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return this.RepoFactory.Get<OrderRepository>().GetActive()
                .Where(x => x.UserId == userId)
                .Project()
                .To<UserOrdersModel>()
                .ToList();
        }

        public IEnumerable<UserOrdersModel> GetMineById(int id)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return this.RepoFactory.Get<OrderRepository>()
                .GetActive()
                .Where(x => x.Id == id)
                .Project()
                .To<UserOrdersModel>()
                .ToList();
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, UserOrdersModel>()
                .ForMember(x => x.Color, option => option.MapFrom(y => y.FabricAndLamel.Color));

            configuration.CreateMap<Order, UserOrdersModel>()
                .ForMember(x => x.BlindCount, opt => opt.MapFrom(z => z.Blinds.Count));

            configuration.CreateMap<Order, UserOrdersModel>()
                .ForMember(x => x.BlindTypeName, option => option.MapFrom(y => y.BlindType.Name));
        }
    }
}