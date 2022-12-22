using System.Collections.Generic;
using System.Threading.Tasks;
using catalogueService.Database;
using catalogueService.Interfaces;
using catalogueService.Database.DBContextFiles;
using AutoMapper;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using catalogueService.Database.DBsets;

namespace catalogueService.Repositories
{
    public class orderRepository : IOrder
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public orderRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<orders> AddOrderAsync(orders ord)
        {
            await _dbcontext.orders.AddAsync(ord);
            await _dbcontext.SaveChangesAsync();
            return ord;
        }

        public async Task<IEnumerable<orders>> GenerateReceiptAsync()
        {
            var results = await _dbcontext.orders.ToListAsync();
            return results;
        }

        public async Task<orders> GetByIdAsync(int orderId)
        {
            return await _dbcontext.orders.FirstOrDefaultAsync(x => x.orderID == orderId);
        }

        public async Task<(bool IsSuccess, IEnumerable<orders> orders)> GetOrdersAsync()
        {
            var results = await _dbcontext.orders.ToListAsync();
            return (true, results);
        }

        public async Task<orders> PurchaseAsync(int OrderID)
        {
            var availableOrder = await _dbcontext.orders.FirstOrDefaultAsync(x => x.orderID == OrderID);
            if (availableOrder != null)
            {
                return availableOrder;
            }
            else
            {
                return null;
            }
        }
    }
}
