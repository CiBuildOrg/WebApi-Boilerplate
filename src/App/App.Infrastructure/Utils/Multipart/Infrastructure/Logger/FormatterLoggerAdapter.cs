using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Formatting;

namespace App.Infrastructure.Utils.Multipart.Infrastructure.Logger
{
    internal class FormatterLoggerAdapter : IFormDataConverterLogger
    {
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        private IFormatterLogger FormatterLogger { get; set; }

        public FormatterLoggerAdapter(IFormatterLogger formatterLogger)
        {
            FormatterLogger = formatterLogger ?? throw new ArgumentNullException(nameof(formatterLogger));
        }

        public void LogError(string errorPath, Exception exception)
        {
            FormatterLogger.LogError(errorPath, exception);
        }

        public void LogError(string errorPath, string errorMessage)
        {
            FormatterLogger.LogError(errorPath, errorMessage);
        }

        public void EnsureNoErrors() 
        {
            //nothing to do
        }
    }
}
