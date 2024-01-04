using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
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
        public UserRepository(ApplicationDBContext db, IConfiguration _configuration)
        {
            _db = db;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");

        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUser.FirstOrDefault(x => x.Username == username);
            if (user == null) 
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponsetDto> Login(LoginRequestDto loginRequestDto)
        {
            //we need a token to generate and send it back
            var user = _db.LocalUser.FirstOrDefault(u => u.Username.ToLower() == loginRequestDto.UserName.ToLower()
             && u.Password == loginRequestDto.Password);
            if (user == null)
            {
                return new LoginResponsetDto {
                    Token = string.Empty,
                    User = null
                
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponsetDto loginResponsetDto = new LoginResponsetDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user

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
