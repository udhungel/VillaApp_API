namespace MagicVilla_API.Models.Dto
{
    public class LoginResponsetDto
    {
        public UserDto User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
