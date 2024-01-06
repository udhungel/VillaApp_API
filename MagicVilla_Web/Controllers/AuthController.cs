using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services;
using Microsoft.AspNetCore.Mvc;


namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto obj = new();
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {           
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            APIResponse apiResponse = await _authService.RegisterAsync<APIResponse>(registrationRequestDto);
            if (apiResponse !=null && apiResponse.IsSucess)
            {
                return RedirectToAction("Login");

            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
