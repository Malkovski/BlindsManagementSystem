namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class FabricAndLamelsController : AdminController
    {
        public ActionResult Index()
        {
            var model = LoadModel<FabricAndLamelsModel, bool>(true);
            ViewBag.Colors = model.Colors;
            ViewBag.BlindTypes = model.BlindTypes;
            ViewBag.Materials = model.Materials;
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]
                                 DataSourceRequest request)
        {
            var result = LoadModel<FabricAndLamelsModel, bool>(false).Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]
                                   DataSourceRequest request, FabricAndLamelsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<FabricAndLamelsModel, bool>(false).Save(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, FabricAndLamelsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<FabricAndLamelsModel, bool>(false).Delete(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }
    }
}