using System.Collections.Generic;

namespace App.Dto.Response
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static RegistrationResult Ok => new RegistrationResult {Success = true};
    }   
}
