namespace Blinds.Web.Areas.Administration.Models
{
    using System.Linq;
    using System.Web.Mvc;

    using Blinds.Web.Models;
    using Common;
    using Kendo.Mvc.UI;

    public class AdminModel : MenuModel
    {
        protected DataSourceResult HandleErrors(ModelStateDictionary modelState)
        {
            var error = GlobalConstants.GeneralDataError;

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