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

namespace catalogueService.Repositories
{
    public class customerRepository : ICustomer
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public customerRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<customer> AddCustomerAsync(customer cus)
        {
            await _dbcontext.Customers.AddAsync(cus);
            await _dbcontext.SaveChangesAsync();
            return cus;
        }

        public async Task<customer> GetByIdAsync(int id)
        {
            return await _dbcontext.Customers.FirstOrDefaultAsync(x => x.customerId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<customer> customers)> GetCustomerAsync()
        {
            try
            {
                var results = await _dbcontext.Customers.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
