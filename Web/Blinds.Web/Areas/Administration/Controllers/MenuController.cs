namespace Blinds.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Blinds.Web.Areas.Administration.Controllers.Base;
    using Models;

    public class MenuController : AdminController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}