using DatingApp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AppData : IAppData
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
    }
}
