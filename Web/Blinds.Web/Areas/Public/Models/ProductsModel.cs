namespace Blinds.Web.Areas.Public.Models
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Web.Models;
    using Data.Repositories;
    using AutoMapper;

    public class ProductsModel : MenuModel, IMapFrom<BlindType>, IModel<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Info { get; set; }

        public byte[] Content { get; set; }

        public void Init(int id)
        {
            base.Init();
            var entity = this.RepoFactory.Get<BlindTypeRepository>().GetById(id);
            Mapper.Map(entity, this);
        }
    }
}