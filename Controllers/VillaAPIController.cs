using Microsoft.AspNetCore.Mvc;
using VillaApp_WebAPI.Data;
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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}")]
        public ActionResult<VillaDTO> GetVillabyId(int id) 
        {
            if (id == 0)
            {
                return BadRequest();

            }
            var villa = VillaStore.villaList.Where(x => x.Id == id).FirstOrDefault();
            if (villa == null) { return NotFound(); } //404 not found 
            return Ok();  

        }
    }
}
