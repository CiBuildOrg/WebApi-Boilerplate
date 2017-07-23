using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace App.Core.Utils
{
    public static class EventLogger
    {
        private const string StackTraceFormatting = "File {0} line {1} column {2}";
        private const string EventLogName = "AppEventLog";

        private static void TryInitializeSource(string source = EventLogName)
        {
            if (!EventLog.SourceExists(source))
            {
                var eventSourceData = new EventSourceCreationData(source, source);
                EventLog.CreateEventSource(eventSourceData);
            }
        }

        public static void WriteError(Exception exception, string additionalInfo, string source = EventLogName)
        {
            try
            {
                TryInitializeSource(source);
                string message = string.Empty;
                var exceptionMessage = GetAllMessages(exception);

                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    message += $"Error Happened. Additional info {additionalInfo} Exception: {Environment.NewLine}";
                }

                message += exceptionMessage;

                using (var tastierLogger = new EventLog(source, ".", source))
                {
                    tastierLogger.WriteEntry(message, EventLogEntryType.Error);
                }
            }
            catch
            {

            }
        }

        public static void WriteInfo(string information, string source = EventLogName)
        {
            try
            {
                TryInitializeSource(source);
                using (var tastierLogger = new EventLog(source, ".", source))
                {
                    tastierLogger.WriteEntry($"Information: {information}", EventLogEntryType.Information);
                }
            }
            catch
            {

            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        private static string GetAllMessages(Exception exception)
        {
            var messages = FromHierarchy(exception, ex => ex.InnerException)
                .Select(ex =>
                {
                    var exceptionName = ex.GetType().Name;
                    var stackTrace = ex.StackTrace;
                    var st = new StackTrace(ex, true);
                    var frames = st.GetFrames();

                    var builder = new StringBuilder();
                    builder.AppendLine(
                        $"===================== Exception body for {exceptionName} ===================== ");

                    // append exception message
                    builder.AppendLine("Message");
                    builder.AppendLine(ex.Message);
                    builder.AppendLine("End of message");

                    // append stacktrace
                    builder.AppendLine("Original stack trace");
                    builder.AppendLine(stackTrace);
                    builder.AppendLine("End of original stack trace");

                    if (frames != null)
                    {
                        builder.AppendLine("Original stack trace (with line number)");
                        // process stacktrace
                        foreach (var frame in frames)
                        {
                            var frameLineNumber = frame.GetFileLineNumber();
                            var frameColumnNumber = frame.GetFileColumnNumber();

                            if (frameLineNumber != 0 || frameColumnNumber != 0)
                            {
                                builder.AppendLine(string.Format(StackTraceFormatting, frame.GetFileName(), frameLineNumber, frameColumnNumber));
                            }
                        }

                        builder.AppendLine("End of original stack trace (with line number)");
                    }

                    builder.AppendLine(
                        $"===================== End Exception body for {exceptionName} ===================== ");
                    return builder.ToString();
                });

            return string.Join(Environment.NewLine, messages);
        }
    }
}