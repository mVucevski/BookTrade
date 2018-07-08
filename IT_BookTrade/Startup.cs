using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IT_BookTrade.Startup))]
namespace IT_BookTrade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
