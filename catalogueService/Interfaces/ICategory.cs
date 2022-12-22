using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface ICategory
    {
        public Task<(bool IsSuccess, IEnumerable<category> categories)> GetCategoryAsync();
        public Task<category> GetByIdAsync(int id);
        public Task<category> AddCategoryAsync(category cat);
        //public Task<category> DeleteAsync(int id);
        //public Task<category> UpdateAsync(int id, category walk);
    }
}
