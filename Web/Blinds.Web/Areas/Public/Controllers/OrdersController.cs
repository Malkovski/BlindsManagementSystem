namespace Blinds.Web.Areas.Public.Controllers
{
    using Models;
    using System.Web.Mvc;
    using Web.Controllers;

    public class OrdersController : BaseController
    {
        public ActionResult Index()
        {
            var model = LoadModel<BlindsModel, bool>(true);
            return View(model);
        }

        public ActionResult NewBlindRow()
        {
            return PartialView("_NewRowPartial");
        }
    }
}