﻿namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Base;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;

    public class ComponentsController : AdminController
    {
        public ActionResult Index()
        {
            var model = LoadModel<ComponentsModel, bool>(true);
            ViewBag.BlindTypes = model.BlindTypes;
            return this.View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]
                                 DataSourceRequest request)
        {
            var result = LoadModel<ComponentsModel, bool>(false).Get();
            return this.Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([DataSourceRequest]
                                   DataSourceRequest request, ComponentsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<ComponentsModel, bool>(false).Save(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, ComponentsModel viewModel)
        {
            if (viewModel != null && this.ModelState.IsValid)
            {
                LoadModel<ComponentsModel, bool>(false).Delete(viewModel);
                return this.GridOperation(viewModel, request);
            }

            return null;
        }
    }
}