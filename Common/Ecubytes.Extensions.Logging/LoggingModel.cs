using System;

namespace Ecubytes.Extensions.Logging
{
    public class LoggingModel
    {
        public Guid LogId {get; set;}
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string Stack { get; set; }
        public string LogLevel { get; set; }
    }
}
