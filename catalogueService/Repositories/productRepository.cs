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
    public class productRepository : IProduct
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;
        public productRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            this._dbcontext = dbcontext;
            this._mapper = mapper;
        }

        public async Task<product> AddProductAsync(product pro)
        {
            await _dbcontext.Products.AddAsync(pro);
            await _dbcontext.SaveChangesAsync();
            return pro;
        }

        public async Task<product> DeleteAsync(int id)
        {
            var walk = await _dbcontext.Products.FindAsync(id);
            if (walk == null)
            {
                return null;
            }

            //Delete the region
            _dbcontext.Products.Remove(walk);
            await _dbcontext.SaveChangesAsync();
            return walk;
        }

        public async Task<product> GetByIdAsync(int id)
        {
            return await _dbcontext.Products.FirstOrDefaultAsync(x => x.productId == id);
        }

        public async Task<(bool, IEnumerable<product>)> GetProductAsync()
        {
            try
            {
                var results = await _dbcontext.Products.ToListAsync();
                return (true, results);
            }
            catch(System.Exception e)
            {
                throw e;
            }
        }

        public async Task<product> UpdateAsync(int id, product walk)
        {
            var existingRegion = await _dbcontext.Products.FindAsync(id);
            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.price = walk.price;
            existingRegion.quantity = walk.quantity;
            existingRegion.description = walk.description;
            existingRegion.name = walk.name;
            existingRegion.categoryId = walk.categoryId;


            await _dbcontext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
