namespace Blinds.Web.Areas.Administration.Controllers
{
    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class PicturesController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<PicturesModel, bool>(true);
            ViewBag.BlindTypes = model.BlindTypes;
            return View(model);
        }

        public JsonResult UploadPicture(IEnumerable<HttpPostedFileBase> pictures)
        {
            if (pictures != null)
            {
                TempData["UploadedFile"] = pictures.First();
            }

            return this.Json(true);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = LoadModel<PicturesModel, bool>(false).Get();
            var jsonResult = Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public PartialViewResult ReadByType(int id)
        {
            var result = this.LoadModel<PicturesModel, bool>(false).GetByType(id);
            return PartialView("_PicturesByTypePartial", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]DataSourceRequest request, PicturesModel viewModel)
        {
            if (viewModel.HasImage)
            {
                viewModel.File = (HttpPostedFileBase)TempData["UploadedFile"];
            }

            var error = this.LoadModel<PicturesModel, bool>(false).Save(viewModel, this.ModelState);

            if (error != null)
            {
                return Json(error);
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
                return Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}