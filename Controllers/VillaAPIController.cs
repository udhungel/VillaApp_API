using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Logging;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Models.Dto;
using VillaApp_WebAPI.Repository.IRepository;

namespace VillaApp_WebAPI.Controllers
{
    //[Route("api/[controller]")] // not the best approach 
    [Route("api/VillaAPI")] // does not have an attribute route exception 
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly Ilogging _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(Ilogging logger, IVillaRepository dbVilla, IMapper mapper) 
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
        } 

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync(); //tolistAsync 
            _logger.LogError("Get all Villas", "");
            return  Ok( _mapper.Map<List<VillaDTO>>(villaList));
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
            var villa =await _dbVilla.GetAsync(x => x.Id == id); 
            if (villa == null) { return NotFound(); } //404 not found 
            return Ok(_mapper.Map<VillaDTO>(villa));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            //if (!(ModelState.IsValid))
            //{
            //    return BadRequest(ModelState); //400 not found 
            //}
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", " Villa Name Already Exists  ");
                return BadRequest(ModelState);//400 not found 

            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            Villa model = _mapper.Map<Villa>(createDTO);
           
            await _dbVilla.CreateAsync(model);

           
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);

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
            var villa = await _dbVilla.GetAsync(u=>u.Id == id); //GetAsyn does as FirstOrDefault
            if (villa == null)
            {
                return NotFound(); // 404 
            }
            await  _dbVilla.RemoveAsync(villa);
          
            return NoContent();  // when we delete we dont need to pass back anything Status for delete is 204 
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]   

        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO) // with IAction Result you do not define the returntype
        {
            if (updateDTO == null || id != updateDTO.Id) //checked if null and id is same if not 
            {
                return BadRequest();
            }
            Villa model = _mapper.Map<Villa>(updateDTO);           
            await _dbVilla.UpdateAsync(model);
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
            var villa = await _dbVilla.GetAsync(u => u.Id == id, false); // async firstorDefault

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
           
            if (villa == null )
            {
                return BadRequest();   // 400 

            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);            
            await _dbVilla.UpdateAsync(model);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();


        }
    }
}
