using ApartmentTrackingService.Models;

namespace ApartmentTrackingService.Services.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> FindByMailAsync(string mail);
    }
}
