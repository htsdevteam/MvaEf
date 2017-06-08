using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ch0401.Startup))]
namespace Ch0401
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
