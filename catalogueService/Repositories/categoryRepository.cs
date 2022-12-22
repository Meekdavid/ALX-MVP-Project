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
    public class categoryRepository : ICategory
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;
        public categoryRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }


        public async Task<category> AddCategoryAsync(category cat)
        {
            await _dbcontext.Categories.AddAsync(cat);
            await _dbcontext.SaveChangesAsync();
            return cat;
        }

        public async Task<category> GetByIdAsync(int id)
        {
            return await _dbcontext.Categories.FirstOrDefaultAsync(x => x.categoryId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<category> categories)> GetCategoryAsync()
        {
            try
            {
                var results = await _dbcontext.Categories.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
