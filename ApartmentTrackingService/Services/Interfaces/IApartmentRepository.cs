using ApartmentTrackingService.Models;

namespace ApartmentTrackingService.Services.Interfaces
{
    public interface IApartmentRepository
    {
        public Task<Apartment> FindByUrlAsync(string url);
    }
}
