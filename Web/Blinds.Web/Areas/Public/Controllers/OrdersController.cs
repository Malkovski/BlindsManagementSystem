namespace Blinds.Web.Areas.Public.Controllers
{
    using Models;
    using Proxies;
    using System.Web.Mvc;
    using Web.Controllers;

    [Authorize]
    public class OrdersController : BaseController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<OrdersModel, bool>(true);
            return this.View(model);
        }

        public ActionResult NewSizeRow()
        {
            return this.PartialView("_SizeRowPartial");
        }

        [HttpPost]
        public JsonResult Save(OrderProxy proxy)
        {
            var result = this.LoadModel<OrdersModel, bool>(true).Save(proxy, this.ModelState);

            return this.Json(result);
        }

        public ActionResult MyOrders(string userId)
        {
            var model = this.LoadModel<OrdersModel, bool>(true).GetMyOrders(userId);
            return this.View(model);
        }

        [HttpGet]
        public JsonResult GetRailColors(int blindTypeId)
        {
            var result = this.LoadModel<OrdersModel, bool>(true).GetRailColors(blindTypeId);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFabricAndLamelColors(int blindTypeId)
        {
            var result = this.LoadModel<OrdersModel, bool>(true).GetFabricAndLamelColors(blindTypeId);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFabricAndLamelMaterials(int colorId, int blindTypeId)
        {
            var result = this.LoadModel<OrdersModel, bool>(true).GetFabricAndLamelMaterials(colorId, blindTypeId);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}