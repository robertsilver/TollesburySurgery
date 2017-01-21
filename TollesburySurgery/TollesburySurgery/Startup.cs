using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TollesburySurgery.Startup))]
namespace TollesburySurgery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
