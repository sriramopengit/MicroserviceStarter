// Services/InventoryService.cs
using InventoryService.Interfaces;
using InventoryService.Models;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;

        public InventoryService(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<InventoryItem>> GetAllAsync() => _repo.GetAllAsync();

        public Task<InventoryItem?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task<InventoryItem> AddAsync(InventoryItem item) => _repo.AddAsync(item);

        public Task<InventoryItem?> UpdateAsync(InventoryItem item) => _repo.UpdateAsync(item);

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
