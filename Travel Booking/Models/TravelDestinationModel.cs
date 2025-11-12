using System.ComponentModel.DataAnnotations;

namespace Travel_Booking.Models
{
    public class TravelDestinationModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Price (RM)")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Start Trip")]
        [Required]
        public DateTime StartTrip { get; set; }

        [Display(Name = "End Trip")]
        [Required]
        [EndAfterStart("StartTrip", ErrorMessage = "End Trip cannot be earlier than Start Trip")]
        public DateTime EndTrip { get; set; }

        [Display(Name = "Quantity")]
        [Range(0, 100)]
        public int Quantity { get; set; } = 0;

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;
        public Guid PublicId { get; set; } = Guid.NewGuid();
    }
}
