namespace Blinds.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Data.Repositories;
    using AutoMapper.QueryableExtensions;
    using Areas.Public.Models;

    public class MenuModel : BaseModel, IModel
    {
        public ICollection<ProductsModel> BlindCategories { get; set; }

        public void Init()
        {
            this.BlindCategories = this.RepoFactory.Get<BlindTypeRepository>().GetActive()
                    .Project().To<ProductsModel>().ToList();
        }
    }
}