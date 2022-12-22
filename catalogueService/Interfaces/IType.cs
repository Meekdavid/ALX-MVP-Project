using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface IType
    {
        public Task<type> AddTypeAsync(type typ);
        public Task<(bool IsSuccess, IEnumerable<type> type)> GetTypeAsync();
    }
}
