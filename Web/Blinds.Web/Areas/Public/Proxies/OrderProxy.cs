namespace Blinds.Web.Areas.Public.Proxies
{
    using Data.Models;
    using Infrastructure.Mapping;
    using System.Collections.Generic;
    using AutoMapper;
    using Common;
    using System.ComponentModel;
    using Data.Models.Enumerations;

    public class OrderProxy : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public bool HasAgreedTerms { get; set; }

        public string OrderNumber { get; set; }

        public int BlindTypeId { get; set; }

        public int RailColorId { get; set; }

        public int FabricAndLamelColorId { get; set; }

        public int FabricAndLamelMaterialId { get; set; }

        public int InstalationTypeId { get; set; }

        public List<BlindProxy> Blinds { get; set; }

        public string BlindTypeName { get; set; }

        public int BlindsCount { get; set; }

        public Color Color { get; set; }

        [DisplayName(GlobalConstants.ColorDisplay)]
        public string ColorName
        {
            get
            {
                return this.Color.GetDescription();
            }
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderProxy>()
                .ForMember(x => x.OrderNumber, option => option.MapFrom(y => y.Number));

            configuration.CreateMap<Order, OrderProxy>()
                .ForMember(x => x.BlindTypeName, option => option.MapFrom(y => y.BlindType.Name));

            configuration.CreateMap<Order, OrderProxy>()
                .ForMember(x => x.Color, option => option.MapFrom(y => y.FabricAndLamel.Color));

            configuration.CreateMap<Order, OrderProxy>()
                .ForMember(x => x.BlindsCount, option => option.MapFrom(y => y.Blinds.Count));
        }
    }
}