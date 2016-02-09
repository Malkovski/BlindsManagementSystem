namespace Blinds.Web.Areas.Public.Controllers
{
    using Models;
    using System.Web.Mvc;
    using Web.Controllers;

    public class OrdersController : BaseController
    {
        public ActionResult Index()
        {
            var model = LoadModel<OrdersModel, bool>(true);
            return View(model);
        }

        public ActionResult NewBlindRow()
        {
            return PartialView("_NewRowPartial");
        }

        [HttpPost]
        public ActionResult Create(OrdersModel viewModel)
        {
            var error = LoadModel<OrdersModel, bool>(true).Save(viewModel, this.ModelState);

            if (error != null)
            {
                return Json(error);
            }

            return RedirectToAction("Details", new { id = viewModel.Id });
        }

        public ActionResult Details(int id)
        {
            var model = LoadModel<OrdersModel, bool>(true);
            return View(model);
        }
    }
}