namespace Blinds.Web.Areas.Public.Proxies
{
    using Data.Models;
    using Infrastructure.Mapping;
    using System.Collections.Generic;
    using AutoMapper;
    using System;

    public class OrderProxy : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public int BlindTypeId { get; set; }

        public int RailColorId { get; set; }

        public int FabricAndLamelColorId { get; set; }

        public int FabricAndLamelMaterialId { get; set; }

        public int InstalationTypeId { get; set; }

        public List<BlindProxy> Blinds { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderProxy>()
                .ForMember(x => x.OrderNumber, option => option.MapFrom(y => y.Number));
        }
    }
}