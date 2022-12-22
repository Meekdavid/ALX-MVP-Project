using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface ICustomer
    {
        public Task<(bool IsSuccess, IEnumerable<customer> customers)> GetCustomerAsync();
        public Task<customer> GetByIdAsync(int id);
        public Task<customer> AddCustomerAsync(customer cus);
        //public Task<customer> DeleteAsync(int id);
        //public Task<customer> UpdateAsync(int id, customer walk);
    }
}
