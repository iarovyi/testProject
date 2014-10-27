using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestructuringTool.Web.Startup))]
namespace RestructuringTool.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
