using System;
using System.ComponentModel.DataAnnotations;

namespace IctFinalProject.DTOs
{
    public class OrderCreationDto
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy HH:mm:ss}")]
        public DateTime DeliveryTime { get; set; }
        public string PhoneNumber { get; set; }
    }
}