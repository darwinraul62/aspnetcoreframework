using System;
using Ecubytes.Data;

namespace Microsoft.Extensions.Logging
{
    public static class MessageTypeExtensions
    {
        public static LogLevel ToLogLevel(this MessageType messageType)
        {
            switch(messageType)
            {
                case MessageType.Error:
                    return LogLevel.Error;
                case MessageType.Warning:                    
                    return LogLevel.Warning;
                default:
                    return LogLevel.Debug;
            }
        }
    }
}
