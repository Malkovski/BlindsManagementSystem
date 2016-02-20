namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Blinds.Web.Areas.Administration.Controllers.Base;
    using Web.Models;

    public class MenuController : AdminController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<MenuModel>();
            return this.View(model);
        }
    }
}