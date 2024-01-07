using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VillaApp_WebAPI.Data;

namespace MagicVilla_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDBContext db, IConfiguration _configuration, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _db = db;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
        }       

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUser.FirstOrDefault(x => x.UserName == username);
            if (user == null) 
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponsetDto> Login(LoginRequestDto loginRequestDto)
        {
            //we need a token to generate and send it back
            var user = _db.ApplicationUser.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);   


            if (user == null|| isValid == false)
            {
                return new LoginResponsetDto {
                    Token = string.Empty,
                    User = null
                
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponsetDto loginResponsetDto = new LoginResponsetDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDto>(user),  
                Role = roles.FirstOrDefault()

            };

            return loginResponsetDto;


        }

        public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto)
        {
            LocalUser user = new LocalUser()
            {
                Name = registrationRequestDto.Name,
                Password = registrationRequestDto.Password,
                Username = registrationRequestDto.UserName,
                Role = registrationRequestDto.Role
            };
            _db.LocalUser.Add(user); 
            await _db.SaveChangesAsync();
            user.Password = "";
            return user; 
        }
    }
}
