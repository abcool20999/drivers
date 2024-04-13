using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(driveSync.Startup))]
namespace driveSync
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
