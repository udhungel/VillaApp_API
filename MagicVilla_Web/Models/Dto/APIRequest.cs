using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models.Dto
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.Get;

        public string Url { get; set; }

        public object Data { get; set; }       
      
    }
}
