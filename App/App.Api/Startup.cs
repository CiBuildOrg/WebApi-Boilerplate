using App.Infrastructure.Logging.Owin;
using Owin;

namespace App.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCommonLogging();
        }
    }
}