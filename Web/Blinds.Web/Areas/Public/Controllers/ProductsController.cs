namespace Blinds.Web.Areas.Public.Controllers
{
    using Web.Controllers;
    using System.Web.Mvc;
    using Models;
    using Administration.Models;

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
    }
}