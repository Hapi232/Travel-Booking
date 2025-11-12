using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_Booking.Models
{
    public class FlavourModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey("TravelDestination")]
        public Guid TravelDestinationId { get; set; }

        public virtual TravelDestinationModel? TravelDestination { get; set; }
    }
}
