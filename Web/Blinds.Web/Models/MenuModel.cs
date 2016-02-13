namespace Blinds.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Data.Repositories;
    using Areas.Public.Models;
    using Infrastructure.Mapping;
    using Data.Models;
    using Kendo.Mvc.UI;
    using System.Web.Mvc;

    public class MenuModel : ManuModel, IModel, IMapFrom<BlindType>
    {
        public ICollection<ProductsModel> BlindCategories { get; set; }

        public void Init()
        {
            this.BlindCategories = this.RepoFactory.Get<BlindTypeRepository>()
                .GetActive()
                .To<ProductsModel>()
                .ToList();
        }

        protected DataSourceResult HandleErrors(ModelStateDictionary modelState)
        {
            var error = "Грешка с данните";

            foreach (var value in modelState.Values)
            {
                if (value.Errors.Count > 0)
                {
                    error = value.Errors.FirstOrDefault().ErrorMessage;
                    break;
                }
            }

            return new DataSourceResult
            {
                Errors = error
            };
        }
    }
}