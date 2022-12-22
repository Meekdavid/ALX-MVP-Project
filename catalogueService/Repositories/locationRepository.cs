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
    public class locationRepository : ILocation
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public locationRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<location> AddLocationAsync(location loc)
        {
            await _dbcontext.Locations.AddAsync(loc);
            await _dbcontext.SaveChangesAsync();
            return loc;
        }

        public async Task<location> GetByIdAsync(int id)
        {
            return await _dbcontext.Locations.FirstOrDefaultAsync(x => x.locationId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<location> locations)> GetLocationAsync()
        {
            try
            {
                var results = await _dbcontext.Locations.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
