using System;
using System.Net.Http;

namespace Ecubytes.AspNetCore.Http
{
    public static class GlobalHandlers
    {
        private static SocketsHttpHandler globalSocketsHttpHandler;

        /// <summary>Alternative to IHttpClientFactory using SocketsHttpHandler for application Lifetime</summary>
        public static SocketsHttpHandler GlobalSocketsHttpHandler
        {
            get
            {
                if(globalSocketsHttpHandler == null)
                    globalSocketsHttpHandler = new SocketsHttpHandler() 
                        { PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2) } ;
                
                return globalSocketsHttpHandler;
            }
        }
    }
}
