using DatingApp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AppData : IAppData, IAuthRepository
    {
        private readonly DataDbContext db;

        public AppData(DataDbContext db)
        {
            this.db = db;
        }
        public async Task<Values> GetById(int id)
        {
            return await db.Values.FindAsync(id);
        }

        public async Task<IEnumerable<Values>> GetValues()
        {
            var query = await db.Values.ToListAsync();
            return query;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await db.Users.FirstOrDefaultAsync(r => r.Username == username);

            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
                
        }

        public async Task<bool> UserExists(string username)
        {
            if(await db.Users.AnyAsync(r => r.Username == username))
            {
                return true;
            }
            return false;
        }
    }
}
