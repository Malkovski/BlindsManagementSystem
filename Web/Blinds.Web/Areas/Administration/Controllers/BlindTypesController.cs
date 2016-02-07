namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class BlindTypesController : AdminController
    {
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]
                                 DataSourceRequest request)
        {
            var result = LoadModel<BlindTypesModel>().Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]
                                   DataSourceRequest request, BlindTypesModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<BlindTypesModel>().Save(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, BlindTypesModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<BlindTypesModel>().Delete(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }
    }
}