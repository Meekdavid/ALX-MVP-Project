using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalogueService.Interfaces
{
    public interface IOrder
    {
        public Task<(bool IsSuccess, IEnumerable<orders> orders)> GetOrdersAsync();
        public Task<orders> GetByIdAsync(int orderId);
        public Task<orders> PurchaseAsync(int orderID);
        public Task <IEnumerable<orders>> GenerateReceiptAsync();
        public Task<orders> AddOrderAsync(orders ord);
    }
}
