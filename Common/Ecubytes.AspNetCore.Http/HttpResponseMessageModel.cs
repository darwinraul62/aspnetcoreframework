using System;
using System.Net.Http;

namespace Ecubytes.AspNetCore.Http
{
    public class HttpResponseMessageModel
    {
        public HttpResponseMessage HttpMessage { get; set; }        
    }

    public class HttpResponseMessageModel<T> : HttpResponseMessageModel
    {        
        public T Data { get; set; }
    }
    
}
