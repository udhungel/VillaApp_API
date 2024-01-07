using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using VillaApp_WebAPI.Models;

namespace MagicVilla_API.Controllers
{
    [Route("api/UsersAuth")] // does not have an attribute route exception 
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response; 

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User== null || string.IsNullOrEmpty(loginResponse.Token) )
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSucess = false;
                _response.ErrorMessage.Add("UserName or Password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSucess = true;
            _response.Result= loginResponse;    
            return Ok(_response);             
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSucess = false;
                _response.ErrorMessage.Add("UserName already exists");
                return BadRequest(_response);

            }
            var user = await _userRepository.Register(model);
            if (user == null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSucess = false;
                _response.ErrorMessage.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSucess = true;
            return Ok(_response);
        }
    }
}
