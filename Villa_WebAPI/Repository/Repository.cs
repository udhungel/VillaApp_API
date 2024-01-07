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
           //_db.VillaNumbers.Include(u=>u.Villa).ToList()
            this.dbSet = _db.Set<T>();

        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        // get individual Villa
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
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
            if (includeProperties !=null)
            {
               foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
               {
                    query = query.Include(includeProp);
               }

            }
            return await query.FirstOrDefaultAsync(); // defered execution     

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null , int pageSize = 3, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet; // it does not get executed right away         

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                //Dry Run
                //pageSize = 5 , pageNumber = 1
                //skip(5 * (2-1).Take(5)
                //Skip(0).Take(5)
               
                //page 2 
                //pageSize = 5 , pageNumber = 2
                //skip(5 * (2-1).Take(5)
                //Skip(5).Take(5)  skips first five and take next 5
              
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }

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
