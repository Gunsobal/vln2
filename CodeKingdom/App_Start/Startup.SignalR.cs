using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(CodeKingdom.Startup))]

namespace CodeKingdom
{
    public partial class Startup
    {
        public void ConfigureSignalR(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}