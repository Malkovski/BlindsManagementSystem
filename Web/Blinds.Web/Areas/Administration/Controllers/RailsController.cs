namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class RailsController : AdminController
    {
        // GET: Administration/Rail
        public ActionResult Index()
        {
            var model = LoadModel<RailsModel, bool>(true);
            ViewBag.Colors = model.Colors;
            ViewBag.BlindTypes = model.BlindTypes;
            return this.View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]
                                 DataSourceRequest request)
        {
            var result = LoadModel<RailsModel, bool>(false).Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]
                                   DataSourceRequest request, RailsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<RailsModel, bool>(false).Save(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, RailsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<RailsModel, bool>(false).Delete(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }
    }
}