namespace MagicVilla_API.Models.Dto
{
    public class LoginRequestDto
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
