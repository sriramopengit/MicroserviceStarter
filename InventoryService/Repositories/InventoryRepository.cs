// Repositories/InventoryRepository.cs
using InventoryService.Data;
using InventoryService.Interfaces;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<InventoryItem?> GetByIdAsync(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }

        public async Task<InventoryItem> AddAsync(InventoryItem item)
        {
            _context.Inventories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<InventoryItem?> UpdateAsync(InventoryItem item)
        {
            var existing = await _context.Inventories.FindAsync(item.Id);
            if (existing == null) return null;

            existing.ProductName = item.ProductName;
            existing.Quantity = item.Quantity;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Inventories.FindAsync(id);
            if (item == null) return false;

            _context.Inventories.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
