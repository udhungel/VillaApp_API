using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using VillaApp_WebAPI.Data;

namespace MagicVilla_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        public UserRepository(ApplicationDBContext db)
        {
            _db = db;
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
            throw new NotImplementedException();
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
