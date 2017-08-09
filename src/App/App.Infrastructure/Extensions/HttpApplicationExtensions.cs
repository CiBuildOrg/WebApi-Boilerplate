using System;
using System.Threading.Tasks;
using System.Web;
using App.Core.Utils;

namespace App.Infrastructure.Extensions
{
    public static class HttpApplicationExtensions
    {
        public static void EnableMonitoring(this HttpApplication app)
        {
            TaskScheduler.UnobservedTaskException += UnhandledExceptionUtils.TaskSchedulerUnobservedTaskException;
            AppDomain.MonitoringIsEnabled = true;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionUtils.CurrentDomainUnhandledException;
        }
    }
}
