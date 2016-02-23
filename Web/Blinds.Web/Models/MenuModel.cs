namespace Blinds.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Data.Repositories;
    using Areas.Public.Models;
    using Infrastructure.Mapping;
    using Data.Models;
    using System.Data.Entity.Validation;
    using System.Text;
    using AutoMapper.QueryableExtensions;

    public class MenuModel : BaseModel, IModel, IMapFrom<BlindType>
    {
        public ICollection<ProductsModel> BlindCategories { get; set; }

        public void Init()
        {
            this.BlindCategories = this.RepoFactory.Get<BlindTypeRepository>().GetActive()
                     .Project().To<ProductsModel>().ToList();
        }

        protected string HandleDbEntityValidationException(DbEntityValidationException e)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var eve in e.EntityValidationErrors)
            {
                builder.AppendLine(string.Format(
                    "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name,
                    eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    builder.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
            }

            return builder.ToString();
        }
    }
}