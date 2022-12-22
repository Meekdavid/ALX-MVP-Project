using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface ILocation
    {
        public Task<(bool IsSuccess, IEnumerable<location> locations)> GetLocationAsync();
        public Task<location> GetByIdAsync(int id);
        public Task<location> AddLocationAsync(location loc);
        //public Task<location> DeleteAsync(int id);
        //public Task<location> UpdateAsync(int id, location walk);
    }
}
