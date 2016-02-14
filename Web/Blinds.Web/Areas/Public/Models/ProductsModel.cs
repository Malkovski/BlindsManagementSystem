namespace Blinds.Web.Areas.Public.Models
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Web.Models;
    using Data.Repositories;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductsModel : MenuModel, IMapFrom<BlindType>, IModel<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Info { get; set; }

        public byte[] Content { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public void Init(int id)
        {
            base.Init();
            var entity = this.RepoFactory.Get<BlindTypeRepository>().GetById(id);
            // TODO........
            this.Info = entity.Info;
            this.Name = entity.Name;
            this.Price = entity.Price;
            this.Pictures = entity.Picures.Where(p => !p.Deleted).ToList();
            this.Content = entity.Content;
        }
    }
}