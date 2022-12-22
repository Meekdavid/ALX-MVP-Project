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
using System.Runtime.Intrinsics.X86;

namespace catalogueService.Repositories
{
    public class typeRepository : IType
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public typeRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<type> AddTypeAsync(type typ)
        {
            await _dbcontext.Types.AddAsync(typ);
            await _dbcontext.SaveChangesAsync();
            return typ;
        }

        public async Task<(bool IsSuccess, IEnumerable<type> type)> GetTypeAsync()
        {
            try
            {
                var results = await _dbcontext.Types.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
