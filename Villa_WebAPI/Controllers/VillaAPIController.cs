using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
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
        private APIResponse _response;
        private readonly Ilogging _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(Ilogging logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync(); //tolistAsync 
                _logger.LogError("Get all Villas", "");
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode =HttpStatusCode.BadRequest;
                    _logger.LogError("Get Villa with Id Error {0}" + id, "error");
                    return BadRequest(_response); //400 not found
                }
                var villa = await _dbVilla.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response); //404 not found 
                }    
                
                _logger.LogError("Get all Villas", "");
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;

        }

        [HttpPost]       
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            try
            {   
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", " Villa Name Already Exists  ");
                    return BadRequest(ModelState);//400 not found 

                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }               
                Villa villa = _mapper.Map<Villa>(createDTO);

                await _dbVilla.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Custom")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();// 400 

                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id); //GetAsyn does as FirstOrDefault
                if (villa == null)
                {
                    return NotFound(); // 404 
                }
                await _dbVilla.RemoveAsync(villa);

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                return Ok(_response);

                //return NoContent();  // when we delete we dont need to pass back anything Status for delete is 204 
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO) // with IAction Result you do not define the returntype
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id) //checked if null and id is same if not 
                {
                    return BadRequest();
                }
                Villa model = _mapper.Map<Villa>(updateDTO);
                await _dbVilla.UpdateAsync(model);

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSucess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;
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

            if (villa == null)
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
