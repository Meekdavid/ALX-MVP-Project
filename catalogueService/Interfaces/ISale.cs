using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface ISale
    {
        public Task<(bool IsSuccess, IEnumerable<sales> sales)> GetSalesAsync();
        public Task<IEnumerable<sales>> GenerateReportAsync();
        public Task<sales> GetByIdAsync(int saleId);
    }
}
