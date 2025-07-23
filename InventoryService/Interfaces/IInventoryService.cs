// Interfaces/IInventoryService.cs
using InventoryService.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItem>> GetAllAsync();
        Task<InventoryItem?> GetByIdAsync(int id);
        Task<InventoryItem> AddAsync(InventoryItem item);
        Task<InventoryItem?> UpdateAsync(InventoryItem item);
        Task<bool> DeleteAsync(int id);
    }
}
