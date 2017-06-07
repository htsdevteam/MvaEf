using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ch0201MusicStore.Startup))]
namespace Ch0201MusicStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
