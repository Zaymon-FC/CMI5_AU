using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMI5_AU.Startup))]
namespace CMI5_AU
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
