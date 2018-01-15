using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class Trace : IBaseEntity
    {
        public Guid Id { get; set; }
        public string CallerIdentity { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public string Verb { get; set; }
        public string Url { get; set; }
        public string RequestPayload { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponsePayload { get; set; }
        public string CallDuration { get; set; }
    }
}