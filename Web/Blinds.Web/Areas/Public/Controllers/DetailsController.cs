namespace Blinds.Web.Areas.Public.Controllers
{
    using System.Web.Mvc;

    using Blinds.Web.Controllers;
    using Models;

    public class DetailsController : BaseController
    {
        public ActionResult Index(int id)
        {
            var model = this.LoadModel<DetailsModel, int>(id);
            return this.View(model);
        }
    }
}