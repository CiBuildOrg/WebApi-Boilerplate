namespace App.Dto.Request
{
    public class TraceExceptionInfo : TraceMessageBase
    {
        public string Exception { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}