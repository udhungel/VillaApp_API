using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAllAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDTO dto);
        Task<T> UpdateAsync<T>(int id);
        Task<T> DeleteAsync<T>(int id);
    }
}
