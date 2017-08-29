using System.Collections.Generic;

namespace App.Dto.Logs
{
    public class LogItem
    {
        public string ErrorPath { get; set; }
        public List<LogErrorInfo> Errors { get; set; }
    }
}