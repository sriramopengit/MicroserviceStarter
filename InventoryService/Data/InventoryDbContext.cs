using Microsoft.EntityFrameworkCore;
using InventoryService.Models;
namespace InventoryService.Data
{
    public class InventoryDbContext : DbContext // Fix CS0311 by inheriting from DbContext  
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options): base(options)
        {
        }

        public DbSet<InventoryItem> Inventories { get; set; }
    }
}
