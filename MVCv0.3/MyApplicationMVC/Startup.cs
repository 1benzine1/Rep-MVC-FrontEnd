using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyApplicationMVC.Startup))]
namespace MyApplicationMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
