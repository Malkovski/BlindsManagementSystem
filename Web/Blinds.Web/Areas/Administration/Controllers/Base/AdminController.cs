namespace Blinds.Web.Areas.Administration.Controllers.Base
{
    using System.Web.Mvc;

    using Common;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public abstract class AdminController : BaseController
    {
        public AdminController()
        {
        }

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}