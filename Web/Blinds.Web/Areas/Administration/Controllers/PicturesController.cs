namespace Blinds.Web.Areas.Administration.Controllers
{
    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public class PicturesController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<PicturesModel, bool>(true);
            this.ViewBag.BlindTypes = model.BlindTypes;
            return this.View(model);
        }

        public JsonResult UploadPicture(IEnumerable<HttpPostedFileBase> pictures)
        {
            if (pictures != null)
            {
                var currentFiles = (HttpPostedFileBase[])this.TempData["UploadedFile"];

                if (currentFiles != null)
                {
                    var combinedFiles = new List<HttpPostedFileBase>();
                    combinedFiles.AddRange(currentFiles);
                    combinedFiles.AddRange(pictures);
                    this.TempData["UploadedFile"] = combinedFiles.ToArray();
                }
                else
                {
                    this.TempData["UploadedFile"] = pictures;
                }
            }

            return this.Json(true);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.LoadModel<PicturesModel, bool>(false).Get();
            var jsonResult = this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]DataSourceRequest request, PicturesModel viewModel)
        {
            if (viewModel.HasImage)
            {
                viewModel.Files = (HttpPostedFileBase[])this.TempData["UploadedFile"];
                this.TempData["UploadedFile"] = null;
            }

            var error = this.LoadModel<PicturesModel, bool>(false).Save(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, PicturesModel viewModel)
        {

            var error = this.LoadModel<PicturesModel, bool>(false).Destroy(viewModel, this.ModelState);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}