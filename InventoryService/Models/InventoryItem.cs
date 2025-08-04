﻿namespace InventoryService.Models
{
    public class InventoryItem
    {
        public string Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
