using System;

namespace IctFinalProject.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }

        /// <summary>
        /// Is this cart is an active for user
        /// It means that user can get this cart or add some products there
        /// If it false, this cart is a part of order
        /// </summary>
        public bool IsActive { get; set; } = true;
 
    }
}