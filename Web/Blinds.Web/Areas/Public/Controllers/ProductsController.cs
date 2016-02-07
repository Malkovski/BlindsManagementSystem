namespace Blinds.Web.Areas.Public.Controllers
{
    using Web.Controllers;
    using System.Web.Mvc;
    using Models;

    public class ProductsController : BaseController
    {
        public ActionResult Index(int id)
        {
            var model = LoadModel<ProductsModel, int>(id);
            return View(model);
        }
    }
}