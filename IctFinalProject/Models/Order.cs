using System;

namespace IctFinalProject.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OrderCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CartId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.InProgress;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}