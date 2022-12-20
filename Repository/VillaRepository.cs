using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Repository.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VillaApp_WebAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaRepository(ApplicationDBContext db) 
        {
            _db = db;
        }
        public async Task CreateAsync(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await SaveAsync();
        }

        // get individual Villa
        public async Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter !=null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync(); // defered execution     

        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas; // it does not get executed right away         
            
            if (filter !=null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync(); // defered execution                                            
        }

        public async Task RemoveAsync(Villa entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            _db.Villas.Update(entity);
            await SaveAsync();
        }


    }
}
