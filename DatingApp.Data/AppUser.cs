using DatingApp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AppUser : IDataRepository
    {
        private readonly DataDbContext dataDb;

        public AppUser(DataDbContext dataDb)
        {
            this.dataDb = dataDb;
        }
        public void Add<T>(T entity) where T : class
        {
            dataDb.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dataDb.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await dataDb.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await dataDb.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await dataDb.SaveChangesAsync() > 0;
        }
    }
}
