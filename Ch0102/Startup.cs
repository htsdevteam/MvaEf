using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ch0102.Startup))]
namespace Ch0102
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
