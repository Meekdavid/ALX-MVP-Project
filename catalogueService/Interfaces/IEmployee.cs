using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface IEmployee
    {
        public Task<(bool IsSuccess, IEnumerable<employees> employees)> GetEmployeeAsync();
        public Task<employees> GetByIdAsync(int id);
        public Task<employees> AddEmployeeAsync(employees emp);
        //public Task<employees> DeleteAsync(int id);
        //public Task<employees> UpdateAsync(int id, employees walk);
    }
}
