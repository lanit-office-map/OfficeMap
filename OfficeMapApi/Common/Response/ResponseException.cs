using System;
using System.Net;

namespace Common.Response
{
    public class ResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
