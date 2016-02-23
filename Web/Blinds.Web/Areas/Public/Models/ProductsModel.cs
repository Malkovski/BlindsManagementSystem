namespace Blinds.Web.Areas.Public.Models
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Web.Models;
    using Data.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;

    public class ProductsModel : PublicModel, IMapFrom<BlindType>, IModel<int>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Info { get; set; }

        public byte[] Content { get; set; }

        public int PicturesCount { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<Picture> AllPictures { get { return this.Pictures.Where(x => x.Deleted == false).ToList(); } }

        public void Init(int id)
        {
            this.Init();
            var entity = this.RepoFactory.Get<BlindTypeRepository>().GetById(id);
            Mapper.Map(entity, this);
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<BlindType, ProductsModel>()
                .ForMember(x => x.PicturesCount, opt => opt.MapFrom(x => x.Pictures.Count()));
        }
    }
}