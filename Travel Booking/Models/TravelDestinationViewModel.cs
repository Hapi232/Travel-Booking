using System.ComponentModel.DataAnnotations;

namespace Travel_Booking.Models
{
    public class TravelDestinationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Price (RM)")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string StartTripString { get; set; }
        public string EndTripString { get; set; }
        public int Quantity { get; set; } = 0;

        public bool IsAvailable { get; set; } = true;
    }
}
