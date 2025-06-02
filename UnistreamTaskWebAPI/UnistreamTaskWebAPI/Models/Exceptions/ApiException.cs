using System.ComponentModel.DataAnnotations;

namespace UnistreamTaskWebAPI.Models.Exceptions
{
    public class ApiException : ValidationException
    {
        public int StatusCode { get; }

        public ApiException(string message, int statusCode = 400)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
