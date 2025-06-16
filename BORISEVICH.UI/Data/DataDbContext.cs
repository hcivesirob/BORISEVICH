using Microsoft.EntityFrameworkCore;
using BORISEVICH.Domain.Entities;

namespace BORISEVICH.UI.Data
{
    public class DataDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
