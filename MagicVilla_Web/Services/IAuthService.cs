using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginResponsetDto objToCreate);

        Task<T> RegisterAsync<T>(RegistrationRequestDto registrationRequestDto);

    }
}
