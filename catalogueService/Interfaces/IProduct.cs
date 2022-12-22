using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace catalogueService.Interfaces
{
    public interface IProduct
    {
        public Task<(bool IsSuccess, IEnumerable<product> products)> GetProductAsync();
        //public Task<(bool IsSuccess, IEnumerable<product> products)> GetProductByIdAsync(int id);
        public Task<product> GetByIdAsync(int id);
        public Task<product> AddProductAsync(product pro);
        public Task<product> DeleteAsync(int id);
        public Task<product> UpdateAsync(int id, product walk);
    }
}
