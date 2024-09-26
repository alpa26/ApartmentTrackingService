using ApartmentTrackingService.Data;
using ApartmentTrackingService.Models;
using ApartmentTrackingService.Models.Interfaces;
using ApartmentTrackingService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApartmentTrackingService.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _database;

        public UserRepository(AppDbContext database)
        {
            _database = database;
        }


        public async Task<User> FindByMailAsync(string mail)
        {
            return
                await _database.Users.Include(x => x.Apartments)
                                     .FirstOrDefaultAsync(x => x.Email.Equals(mail));
        }

        public async Task<bool> CreateAsync(User item)
        {
            try
            {
                var result = await _database.Users.AddAsync(item);

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

        public async Task<bool> ChangeAsync(User item)
        {
            try
            {
                _database.Users.Update(item);
                await _database.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
