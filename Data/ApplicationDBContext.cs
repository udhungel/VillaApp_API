using Microsoft.EntityFrameworkCore;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options) { }
       
        public DbSet<Villa> Villas  { get; set; }


    }
}
