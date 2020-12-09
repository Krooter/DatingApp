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

        public async Task<Like> GetLike(int userId, int recepientId)
        {
            return await dataDb.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikedId == recepientId);
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
            var users = dataDb.Users.Include(p => p.Photos).OrderByDescending(u => u.LastOnline).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);


            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likeds)
            {
                var userLikeds = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikeds.Contains(u.Id));
            }

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

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await dataDb.Users.Include(x => x.Likers).Include(x => x.Likeds).FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                return user.Likers.Where(u => u.LikedId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likeds.Where(u => u.LikerId == id).Select(i => i.LikedId);
            }
        }

        public async Task<bool> SaveAll()
        {
            return await dataDb.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await dataDb.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = dataDb.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await dataDb.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId || m.RecipientId == recipientId && m.SenderId == userId)
                .OrderByDescending(m => m.MessageSent).ToListAsync();

            return messages;
        }
    }
}
