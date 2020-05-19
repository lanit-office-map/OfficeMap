using System.Net;

namespace Common.Response
{
    public class Response<T> where T : class
    {
        public ResponseResult Status { get; set; }
        public ResponseException Error { get; set; }
        public T Result { get; set; }

        //Without object
        public Response()
        {
            Status = ResponseResult.Success;
        }

        public Response(HttpStatusCode statusCode, string message)
        {
            Status = ResponseResult.Error;
            Error = new ResponseException(statusCode, message);
        }

        //With object
        public Response(T result) : this()
        {
            Result = result;
        }

        public Response(T result, HttpStatusCode statusCode, string message) : this(statusCode, message)
        {
            Result = result;
        }
    }

    public enum ResponseResult
    {
        Success,
        Error
    }
}
