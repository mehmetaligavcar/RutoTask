using Microsoft.EntityFrameworkCore;
using Task.Models.Domain;

namespace Task.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Currency> Currencies { get; set; }
    }
}
