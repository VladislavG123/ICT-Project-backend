using System;
using System.ComponentModel.DataAnnotations;

namespace IctFinalProject.DTOs
{
    public class OrderCreationDto
    {
        public long DeliveryTime { get; set; }
        public string PhoneNumber { get; set; }
    }
}