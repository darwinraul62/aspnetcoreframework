using System;
using Ecubytes.Data;

namespace Microsoft.Extensions.Logging
{
    public static class LogExtensions
    {
        public static void Log(this ILogger logger, MessageCollection messages)
        {
            foreach (var m in messages)
            {
                logger.Log(m.Type.ToLogLevel(), m.Text);
            }
        }
    }
}
