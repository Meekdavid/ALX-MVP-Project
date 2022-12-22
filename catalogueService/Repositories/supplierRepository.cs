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
    public class supplierRepository : ISupplier
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public supplierRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<supplier> AddSupplierAsync(supplier sup)
        {
            await _dbcontext.Suppliers.AddAsync(sup);
            await _dbcontext.SaveChangesAsync();
            return sup;
        }

        public async Task<supplier> GetByIdAsync(int id)
        {
            return await _dbcontext.Suppliers.FirstOrDefaultAsync(x => x.supplierId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<supplier> suppliers)> GetSupplierAsync()
        {
            try
            {
                var results = await _dbcontext.Suppliers.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
