using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blinds.Web.Startup))]
namespace Blinds.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
