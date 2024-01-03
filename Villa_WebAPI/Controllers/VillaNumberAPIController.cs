using AutoMapper;
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
using VillaAppWebAPI.Migrations;

namespace VillaApp_WebAPI.Controllers
{
    //[Route("api/[controller]")] // not the best approach 
    [Route("api/VillaNumberAPI")] // does not have an attribute route exception 
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private APIResponse _response;
        private readonly Ilogging _logger;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaNumberAPIController(Ilogging logger, IVillaNumberRepository dbVillaNumber, IMapper mapper,IVillaRepository dbVilla)
        {
            _logger = logger;
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            this._response = new();
            _dbVilla = dbVilla;
        }

        [HttpGet] //failed to lod API defination. Endpoint we add needs to be defined HTTP get or POST
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasNumber()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa"); //tolistAsync 
                _logger.LogError("Get all Villas", "");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode =HttpStatusCode.BadRequest;
                    _logger.LogError("Get Villa with Id Error {0}" + id, "error");
                    return BadRequest(_response); 
                }
                var villaNumber = await _dbVillaNumber.GetAsync(x => x.VillaNo== id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }    
                
                _logger.LogError("Get all Villas", "");
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {   
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessage", " VillaNumber Already Exists  ");
                    return BadRequest(ModelState);//400 not found 

                }
                if (await _dbVilla.GetAsync(db=>db.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessage", " VillaId is Invalid.");
                    return BadRequest(ModelState);

                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };


            }
            return _response;

        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();// 400 

                }
                var villa = await _dbVillaNumber.GetAsync(u => u.VillaNo == id); //GetAsyn does as FirstOrDefault
                if (villa == null)
                {
                    return NotFound(); // 404 
                }
                await _dbVillaNumber.RemoveAsync(villa);

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


        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO) // with IAction Result you do not define the returntype
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo) //checked if null and id is same if not 
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(db => db.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessage", " VillaId is Invalid.");
                    return BadRequest(ModelState);

                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);
                await _dbVillaNumber.UpdateAsync(model);

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

        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) //checked if null and id is same if not 
            {
                return BadRequest();  // 400 
            }
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, false); // async firstorDefault

            VillaNumberUpdateDTO villaNumberDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            if (villaNumber == null)
            {
                return BadRequest();   // 400 

            }
            patchDTO.ApplyTo(villaNumberDTO, ModelState);
            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDTO);
            await _dbVillaNumber.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();


        }
    }
}
