namespace Blinds.Web.Models.Base
{
    using Data.RepoFactory;
    using Kendo.Mvc.UI;
    using Microsoft.Practices.Unity;
    using System.Linq;
    using System.Web.Mvc;

    public abstract class BaseModel
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }

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