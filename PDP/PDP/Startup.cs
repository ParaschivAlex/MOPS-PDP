using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PDP.Startup))]
namespace PDP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
