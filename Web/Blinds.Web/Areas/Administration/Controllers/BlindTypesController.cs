namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class BlindTypesController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<BlindTypesModel>();
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.LoadModel<BlindTypesModel>().Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]DataSourceRequest request, BlindTypesModel viewModel)
        {
            if (viewModel.HasImage)
            {
                viewModel.File = (HttpPostedFileBase)this.TempData["UploadedFile"];
            }

            var error = this.LoadModel<BlindTypesModel>().Save(viewModel, this.ModelState);

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
                this.TempData["UploadedFile"] = files.First();
            }

            return this.Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, BlindTypesModel viewModel)
        {
            var error = this.LoadModel<BlindTypesModel>().Delete(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}