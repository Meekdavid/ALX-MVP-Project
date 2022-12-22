using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalogueService.requestETresponse;

namespace catalogueService.Interfaces
{
    public interface IUser
    {
        public Task<(bool IsSuccess, IEnumerable<users> users)> GetUserAsync();
        public Task<users> GetByIdAsync(int id);
        public Task<users> AddUserAsync(users use);
        //public Task<users> DeleteAsync(int id);
        public Task<users> UpdateAsync(int id, users walk);
    }
}
