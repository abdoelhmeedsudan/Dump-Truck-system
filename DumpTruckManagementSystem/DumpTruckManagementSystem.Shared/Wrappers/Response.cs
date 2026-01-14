using System.Net;

namespace DumpTruckManagementSystem.Shared.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public Response(HttpStatusCode httpStatusCode, string message)
        {
            Succeeded = false;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }

        public Response(HttpStatusCode httpStatusCode, T data, string message = null, List<string> errors = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            HttpStatusCode = httpStatusCode;
        }
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public IDictionary<string, string[]> ModelErrors { get; set; }
        public T Data { get; set; }

    }
}
