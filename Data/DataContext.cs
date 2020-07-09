using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Product> products { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<User> User { get; set; }
    }
}