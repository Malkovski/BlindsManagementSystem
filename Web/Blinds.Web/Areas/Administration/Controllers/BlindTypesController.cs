namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using System.Collections.Generic;
    using System.Web;
    using System.Linq;

    public class BlindTypesController : AdminController
    {
        public ActionResult Index()
        {
            var model = LoadModel<BlindTypesModel>();
            return this.View(model);
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
            if (viewModel.HasImage)
            {
                viewModel.File = (HttpPostedFileBase)TempData["UploadedFile"];
            }

            var error = LoadModel<BlindTypesModel>().Save(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }

        public JsonResult UploadImage(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                TempData["UploadedFile"] = files.First();
            }

            return this.Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, BlindTypesModel viewModel)
        {
            var error = LoadModel<BlindTypesModel>().Delete(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}