using Microsoft.AspNetCore.Mvc;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Models.Dto;

namespace VillaApp_WebAPI.Controllers
{
    //[Route("api/[controller]")] // not the best approach 
    [Route("api/VillaAPI")] // does not have an attribute route exception 
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        public IEnumerable<VillaDTO> GetVillas()
        {
            return new List<VillaDTO>() { new VillaDTO() { Id = 1, Name = "Pool View " },
                                          new VillaDTO() { Id = 2, Name = "Beach View " }
                                        };
        }   


    }
}
