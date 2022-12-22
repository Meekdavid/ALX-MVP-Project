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
    public class saleRepository : ISale
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public saleRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<sales>> GenerateReportAsync()
        {
            var results = await _dbcontext.sales.ToListAsync();
            return results;
        }

        public async Task<sales> GetByIdAsync(int saleId)
        {
            return await _dbcontext.sales.FirstOrDefaultAsync(x => x.saleId == saleId);
        }

        public async Task<(bool IsSuccess, IEnumerable<sales> sales)> GetSalesAsync()
        {
            var results = await _dbcontext.sales.ToListAsync();
            return (true, results);
        }
    }
}
