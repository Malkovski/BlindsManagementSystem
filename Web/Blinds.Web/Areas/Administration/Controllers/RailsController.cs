namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class RailsController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<RailsModel, bool>(true);
            this.ViewBag.Colors = model.Colors;
            this.ViewBag.BlindTypes = model.BlindTypes;
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.LoadModel<RailsModel, bool>(false).Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]DataSourceRequest request, RailsModel viewModel)
        {
            var error = this.LoadModel<RailsModel, bool>(false).Save(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, RailsModel viewModel)
        {
            var error = this.LoadModel<RailsModel, bool>(false).Delete(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}