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
using catalogueService.Models;
using catalogueService.requestETresponse;

namespace catalogueService.Repositories
{
    public class userRepository : IUser
    {
        private readonly catalogueDBContext _dbcontext;
        private readonly IMapper _mapper;

        public userRepository(catalogueDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<users> AddUserAsync(users use)
        {
            await _dbcontext.Users.AddAsync(use);
            await _dbcontext.SaveChangesAsync();
            return use;
        }

        public async Task<users> GetByIdAsync(int id)
        {
            return await _dbcontext.Users.FirstOrDefaultAsync(x => x.userId == id);
        }

        public async Task<(bool IsSuccess, IEnumerable<users> users)> GetUserAsync()
        {
            try
            {
                var results = await _dbcontext.Users.ToListAsync();
                return (true, results);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<users> UpdateAsync(int id, users walk)
        {
            var existingUser = await _dbcontext.Users.FindAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.firstName = existingUser.firstName;
            existingUser.lastName = existingUser.lastName;
            existingUser.phoneNumber = existingUser.phoneNumber;
            existingUser.typeId = existingUser.typeId;
            existingUser.role = walk.role;
            existingUser.wallet = walk.wallet;

            await _dbcontext.SaveChangesAsync();
            return existingUser;
        }

    }
}
