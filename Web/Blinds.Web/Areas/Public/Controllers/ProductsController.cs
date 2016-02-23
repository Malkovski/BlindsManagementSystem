namespace Blinds.Web.Areas.Public.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    using Administration.Models;
    using Models;
    using Web.Controllers;

    public class ProductsController : BaseController
    {
        public ActionResult Index(int id)
        {
            var model = this.LoadModel<ProductsModel, int>(id);
            return this.View(model);
        }

        public PartialViewResult Details(int id)
        {
            var model = this.LoadModel<PicturesModel, bool>(false).GetById(id);
            return this.PartialView("_DetailedPicturePartial", model);
        }

        public ActionResult DownloadInstruction(string filename)
        {
            if (Path.GetExtension(filename) == ".pdf")
            {
                string fullPath = Path.Combine(this.Server.MapPath("~/Areas/Public/Files"), filename);
                return this.File(fullPath, "application/pdf");
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}