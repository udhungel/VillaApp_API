using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Repository.IRepository;

namespace VillaApp_WebAPI.Repository
{
    //generic repository 
    public class Repository<T> :IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet; 
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        // get individual Villa
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync(); // defered execution     

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet; // it does not get executed right away         

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync(); // defered execution                                            
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
