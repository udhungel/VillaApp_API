using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]     
        public ActionResult<VillaDTO> GetVilla(int id) 
        {
            if (id == 0)
            {
                return BadRequest(); //400 not found

            }
            var villa = VillaStore.villaList.Where(x => x.Id == id).FirstOrDefault();
            if (villa == null) { return NotFound(); } //404 not found 
            return Ok(villa);  

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
       {
            //if (!(ModelState.IsValid))
            //{
            //    return BadRequest(ModelState); //400 not found 
            //}
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", " Villa Name Already Exists  "); 
                return BadRequest(ModelState);//400 not found 

            }
            if (villaDTO == null) 
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;

            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id },villaDTO);
           
        }
    }
}
