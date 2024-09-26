using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using ApartmentTrackingService.Models.Interfaces;

namespace ApartmentTrackingService.Models;

public class User: IEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; } = "null";
    public List<Apartment> Apartments { get; set; } = new List<Apartment>();

}
