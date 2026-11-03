using System;

namespace kyrsach.Models
{
    public enum ProductStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Pending;
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}