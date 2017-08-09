namespace App.Dto.Request
{
    public class TraceSearchRequest
    {
        public int Offset { get; set; }

        public int Limit { get; set; }

        public string Search { get; set; }

        public string Sort { get; set; }

        public string Order { get; set; }
    }
}
