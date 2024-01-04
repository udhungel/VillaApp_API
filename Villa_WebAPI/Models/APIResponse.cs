using System.Net;

namespace VillaApp_WebAPI.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessage = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessage { get; set; }

        public bool IsSucess { get; set; } = true;

        public object Result { get; set; }
    }
}
