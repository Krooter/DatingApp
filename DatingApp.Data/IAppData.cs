using DatingApp.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IAppData
    {
        public Task<IEnumerable<Values>> GetValues();
        public Task<Values> GetById(int id);
    }
}