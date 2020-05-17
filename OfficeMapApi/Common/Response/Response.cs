using System.Net;

namespace Common.Response
{
    public class Response<T> where T : class
    {
        public ResponseResult Status { get; set; }
        public T Result { get; set; }
        public ResponseException Error { get; set; }

        public Response(T result)
        {
            Result = result;
            Status = ResponseResult.Success;
        }

        public Response(HttpStatusCode statusCode, string message)
        {
            Status = ResponseResult.Error;
            Error = new ResponseException(statusCode, message);
        }

        public Response(T result, HttpStatusCode statusCode, string message)
        {
            Result = result;
            Status = ResponseResult.Error;
            Error = new ResponseException(statusCode, message);
        }
    }

    public enum ResponseResult
    {
        Success,
        Error
    }
}
