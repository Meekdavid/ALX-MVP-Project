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
    public class employeeRepository : IEmployee
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public employeeRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<employees> AddEmployeeAsync(employees emp)
        {
            await _dbcontext.Employees.AddAsync(emp);
            await _dbcontext.SaveChangesAsync();
            return emp;
        }

        public async Task<employees> GetByIdAsync(int id)
        {
            return await _dbcontext.Employees.FirstOrDefaultAsync(x => x.employeeId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<employees> employees)> GetEmployeeAsync()
        {
            try
            {
                var results = await _dbcontext.Employees.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
 
            }
        }
    }
}
