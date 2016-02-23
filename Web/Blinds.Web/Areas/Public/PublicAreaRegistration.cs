namespace Blinds.Web.Areas.Public
{
    using System.Web.Mvc;

    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Public";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Details",
                url: "Public/Details/{id}",
                defaults: new { controller = "Details", action = "Index" });

            context.MapRoute(
                "Public_default",
                "Public/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}