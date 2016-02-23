namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class OrderController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<OrderModel, bool>(true);
            this.ViewBag.Colors = model.Colors;
            this.ViewBag.InstalationTypes = model.InstalationTypes;
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.LoadModel<OrderModel, bool>(true).Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, OrderModel viewModel)
        {
            var error = this.LoadModel<OrderModel, bool>(false).Delete(viewModel);

            if (error != null)
            {
                return this.Json(error);
            }

            return this.GridOperation(viewModel, request);
        }
    }
}