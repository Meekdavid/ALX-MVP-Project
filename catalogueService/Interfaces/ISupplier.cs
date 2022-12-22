using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface ISupplier
    {
        public Task<(bool IsSuccess, IEnumerable<supplier> suppliers)> GetSupplierAsync();
        public Task<supplier> GetByIdAsync(int id);
        public Task<supplier> AddSupplierAsync(supplier sup);
        //public Task<supplier> DeleteAsync(int id);
        //public Task<supplier> UpdateAsync(int id, supplier walk);
    }
}
