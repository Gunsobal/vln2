using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeKingdom.Startup))]
namespace CodeKingdom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureSignalR(app);
        }
    }
}
