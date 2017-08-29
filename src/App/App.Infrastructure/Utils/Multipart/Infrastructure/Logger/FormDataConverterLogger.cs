using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace App.Infrastructure.Utils.Multipart.Infrastructure.Logger
{
    public class FormDataConverterLogger : IFormDataConverterLogger
    {
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        private Dictionary<string, List<LogErrorInfo>> Errors { get; set; }

        public FormDataConverterLogger()
        {
            Errors = new Dictionary<string, List<LogErrorInfo>>();
        }

        public void LogError(string errorPath, System.Exception exception)
        {
            AddError(errorPath, new LogErrorInfo(exception));
        }

        public void LogError(string errorPath, string errorMessage)
        {
            AddError(errorPath, new LogErrorInfo(errorMessage));
        }

        public List<LogItem> GetErrors()
        {
            return Errors.Select(m => new LogItem
            {
                ErrorPath = m.Key,
                Errors = m.Value.Select(t => t).ToList()
            }).ToList();
        }
                
        public void EnsureNoErrors()
        {
            if (!Errors.Any()) return;
            
            var errors = Errors
                .SelectMany(m => m.Value)
                .Select(m => m.ErrorMessage ?? (m.Exception?.Message ?? ""))
                .ToList();

            var errorMessage = string.Join(" ", errors);

            throw new System.Exception(errorMessage);
        }

        private void AddError(string errorPath, LogErrorInfo info)
        {
            List<LogErrorInfo> listErrors;
            if (!Errors.TryGetValue(errorPath, out listErrors))
            {
                listErrors = new List<LogErrorInfo>();
                Errors.Add(errorPath, listErrors);
            }
            listErrors.Add(info);
        }

       
    }
}
