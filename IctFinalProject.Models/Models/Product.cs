using System;

namespace IctFinalProject.Models.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        
        /// <summary>
        /// Can user order this product
        /// </summary>
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}