namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Blinds.Web.Areas.Administration.Controllers.Base;
    using Models;

    public class MenuController : AdminController
    {
        // GET: Administration/Menu
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult BlindTypes()
        {
            return this.View();
        }

        public ActionResult Rails()
        {
            var model = LoadModel<RailsModel, bool>(true);
            ViewBag.Colors = model.Colors;
            ViewBag.BlindTypes = model.BlindTypes;
            return this.View(model);
        }

        public ActionResult FabricAndLamels()
        {
            var model = LoadModel<FabricAndLamelsModel, bool>(true);
            ViewBag.Colors = model.Colors;
            ViewBag.BlindTypes = model.BlindTypes;
            ViewBag.Materials = model.Materials;
            return this.View(model);
        }
    }
}