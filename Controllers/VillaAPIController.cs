using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Logging;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Models.Dto;

namespace VillaApp_WebAPI.Controllers
{
    //[Route("api/[controller]")] // not the best approach 
    [Route("api/VillaAPI")] // does not have an attribute route exception 
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly Ilogging _logger;
        private readonly ApplicationDBContext _db; 

        public VillaAPIController(Ilogging logger, ApplicationDBContext db1) 
        {
            _logger = logger;
            _db = db1;
        } 

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogError("Get all Villas", "");
            return  Ok( await _db.Villas.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Villa with Id Error {0}" + id , "error");
                return BadRequest(); //400 not found

            }
            var villa =await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null) { return NotFound(); } //404 not found 
            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            //if (!(ModelState.IsValid))
            //{
            //    return BadRequest(ModelState); //400 not found 
            //}
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", " Villa Name Already Exists  ");
                return BadRequest(ModelState);//400 not found 

            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                //Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupany = villaDTO.Occupany,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();

           
            return CreatedAtRoute("GetVilla", new { id = model.Id }, villaDTO);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla (int id) 
        {
            if (id == 0)
            {
                return BadRequest();// 400 

            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u=>u.Id == id);
            if (villa == null)
            {
                return NotFound(); // 404 
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();  // when we delete we dont need to pass back anything Status for delete is 204 
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]   

        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO villaDTO) // with IAction Result you do not define the returntype
        {
            if (villaDTO == null || id != villaDTO.Id) //checked if null and id is same if not 
            {
                return BadRequest();
            }

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupany = villaDTO.Occupany,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) //checked if null and id is same if not 
            {
                return BadRequest();  // 400 
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            VillaUpdateDTO villaUpdateDTO = new()
                {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupany = villa.Occupany,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };
            if (villa == null )
            {
                return BadRequest();   // 400 

            }
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);
            Villa model = new Villa()
            {
                Amenity = villaUpdateDTO.Amenity,
                Details = villaUpdateDTO.Details,
                Id = villaUpdateDTO.Id,
                ImageUrl = villaUpdateDTO.ImageUrl,
                Name = villaUpdateDTO.Name,
                Occupany = villaUpdateDTO.Occupany,
                Rate = villaUpdateDTO.Rate,
                Sqft = villaUpdateDTO.Sqft,

            };
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();


        }
    }
}
