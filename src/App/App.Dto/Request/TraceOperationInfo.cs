namespace App.Dto.Request
{
    public class TraceOperationInfo : TraceMessageBase
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Metadata { get; set; }
    }
}