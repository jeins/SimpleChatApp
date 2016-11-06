

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PimSuite.ChatHelper.SignalRConfig))]
namespace PimSuite.ChatHelper
{
    public class SignalRConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}