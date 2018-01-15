using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Optimization;
using App.Infrastructure.Extensions;
using App.Infrastructure.Tracing;

namespace App.Api
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // monitor unhandled exceptions
            this.EnableMonitoring();
        }
    }
}