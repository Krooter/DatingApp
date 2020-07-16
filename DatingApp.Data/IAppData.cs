using DatingApp.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IAppData
    {
        Task<IEnumerable<Values>> GetValues();
        Task<Values> GetById(int id);
    }
}