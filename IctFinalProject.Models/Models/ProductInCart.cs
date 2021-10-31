using System;

namespace IctFinalProject.Models.Models
{
    public class ProductInCart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }
}