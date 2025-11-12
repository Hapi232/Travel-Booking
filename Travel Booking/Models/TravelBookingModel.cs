using System;
using System.ComponentModel.DataAnnotations;

namespace Travel_Booking.Models
{
    public class TravelBookingModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public Guid TravelDestinationId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Paid";

        [Required]
        public DateTime BookedAt { get; set; } = DateTime.Now;

    }
}
