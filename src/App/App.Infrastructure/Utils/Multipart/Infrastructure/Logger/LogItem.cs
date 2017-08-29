using System.Collections.Generic;

namespace App.Infrastructure.Utils.Multipart.Infrastructure.Logger
{
    public class LogItem
    {
        public string ErrorPath { get; set; }
        public List<LogErrorInfo> Errors { get; set; }
    }
}