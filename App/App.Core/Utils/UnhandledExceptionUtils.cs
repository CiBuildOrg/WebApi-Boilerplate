using System;
using System.Threading.Tasks;

namespace App.Core.Utils
{
    public static class UnhandledExceptionUtils
    {
        public static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var isTerminating = e.IsTerminating;
            var exception = (Exception)e.ExceptionObject;
            string additional = "From AppDomain unhandled exceptions. ";
            if (isTerminating)
            {
                additional += "Exception is terminal.";
            }

            EventLogger.WriteError(exception, additional);
        }

        /// <summary>
        /// Subscribe to task unobserved exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            var exception = e.Exception.Flatten();
            EventLogger.WriteError(exception, "Unboserved task exception");
        }
    }
}