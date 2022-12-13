using Microsoft.AspNetCore.Mvc;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Controllers
{

    [Route("api/VillaAPI")] // does not have an attribute route exception 
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>() { new Villa() { Id = 1, Name = "Pool View " },
                                       new Villa() { Id = 2, Name = "Beach View " }
                                     };
        }   


    }
}
