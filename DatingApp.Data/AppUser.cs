using DatingApp.Core;
using DatingApp.Data.Helpers;
using DatingApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Photo> GetMainPhoto(int userId)
        {
            var curentMainPhoto = await dataDb.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(x => x.IsMain);
            return curentMainPhoto;
        }


        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await dataDb.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await dataDb.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = dataDb.Users.Include(p => p.Photos).OrderByDescending( u=> u.LastOnline ).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);


            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDateofBirth = DateTime.Now.AddYears(-userParams.MaxAge - 1);
                var maxDateofBirth = DateTime.Now.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateBirth >= minDateofBirth && u.DateBirth <= maxDateofBirth);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastOnline);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await dataDb.SaveChangesAsync() > 0;
        }
    }
}
