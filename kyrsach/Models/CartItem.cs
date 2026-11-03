using System;
using kyrsach.Models; // <--- Вот этого не хватало

namespace kyrsach.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Total => Product.Price * Quantity;
    }
}