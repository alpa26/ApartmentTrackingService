using System.ComponentModel.DataAnnotations;
using ApartmentTrackingService.Models.Interfaces;

namespace ApartmentTrackingService.Models
{
    public class Apartment : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string URL { get; set; } = "null";
        public List<User> Users { get; set; } = new List<User>();

    }
}
