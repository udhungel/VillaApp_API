using System.Linq.Expressions;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Repository.IRepository
{
    public interface IVillaNumberRepository :IRepository<VillaNumber>
    {   

        Task<VillaNumber> UpdateAsync(VillaNumber entity);




    }
}
