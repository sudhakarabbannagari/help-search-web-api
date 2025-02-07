using Help.Search.Heroku.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Help.Search.Heroku.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<SearchdData> Data { get; set; }
    }
}
