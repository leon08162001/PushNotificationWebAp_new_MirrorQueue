using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PushNotificationWebAp.Startup))]
namespace PushNotificationWebAp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
