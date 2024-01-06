using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAllAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaCreateDTO dto, string token );
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
