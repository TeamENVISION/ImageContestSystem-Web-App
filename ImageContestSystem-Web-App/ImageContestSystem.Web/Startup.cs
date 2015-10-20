using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageContestSystem.Web.Startup))]
namespace ImageContestSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
