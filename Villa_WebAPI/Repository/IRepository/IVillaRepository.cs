using System.Linq.Expressions;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Repository.IRepository
{
    public interface IVillaRepository :IRepository<Villa>
    {   

        Task<Villa> UpdateAsync(Villa entity);




    }
}
