namespace Blinds.Web.Areas.Public.Proxies
{
    using System;

    using AutoMapper;
    using Blinds.Data.Models;
    using Blinds.Web.Infrastructure.Mapping;

    public class BlindProxy : IMapFrom<Blind>, IHaveCustomMappings
    {
        public decimal Height { get; set; }

        public decimal Width { get; set; }

        public int Control { get; set; }

        public string ControlName { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Blind, BlindProxy>()
                .ForMember(x => x.Control, option => option.MapFrom(y => (int)y.Control));
        }
    }
}