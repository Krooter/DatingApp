using DatingApp.Core;
using DatingApp.Data.Helpers;
using DatingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IDataRepository
    {
        public void Add<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhoto(int userId);
        Task<Like> GetLike(int userId, int recepientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams message);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recepientId);

    }
}
