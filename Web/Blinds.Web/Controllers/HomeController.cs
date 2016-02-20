namespace Blinds.Web.Controllers
{
    using System.Web.Mvc;
    using Blinds.Web.Models;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = this.LoadModel<MenuModel>();
            return this.View(model);
        }
    }
}