// Interfaces/IInventoryRepository.cs
using InventoryService.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetAllAsync();
        Task<InventoryItem?> GetByIdAsync(int id);
        Task<InventoryItem> AddAsync(InventoryItem item);
        Task<InventoryItem?> UpdateAsync(InventoryItem item);
        Task<bool> DeleteAsync(int id);
    }
}
