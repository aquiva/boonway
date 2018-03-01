using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Boonwayy.Startup))]
namespace Boonwayy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
