using System.Linq.Expressions;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null ); //all record GET call 
        //async tracking
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool trackeed= true ); //GEt by id returns one record
        Task CreateAsync(Villa entity);
        Task RemoveAsync(Villa entity);
        Task SaveAsync();

        Task UpdateAsync(Villa entity);




    }
}
