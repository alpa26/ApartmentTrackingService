using ApartmentTrackingService.Data;
using ApartmentTrackingService.Models;
using ApartmentTrackingService.Models.Interfaces;
using ApartmentTrackingService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApartmentTrackingService.Services
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly AppDbContext _database;

        public ApartmentRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<Apartment> FindByUrlAsync(string url)
        {
            return await _database.Apartments.FirstOrDefaultAsync(x => x.URL == url);
        }

        public async Task<bool> CreateAsync(Apartment item)
        {
            try
            {
                var result = await _database.Apartments.AddAsync(item);

                if (result.State != EntityState.Added)
                    return false;
                await _database.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
