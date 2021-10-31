using System;
using System.Collections.Generic;
using IctFinalProject.Models.Models;

namespace IctFinalProject.Models.DTOs
{
    public class OrderInfoViewModel
    {
        public Guid OrderId { get; set; }
        public int OrderCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<Product> Products { get; set; }
    }
}