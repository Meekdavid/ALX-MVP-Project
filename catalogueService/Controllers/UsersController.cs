using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catalogueService.Interfaces;
using catalogueService.Repositories;
using AutoMapper;
using catalogueService.Database;
using catalogueService.Models;
using catalogueService.requestETresponse;
using System;
using Microsoft.AspNetCore.Authorization;
using catalogueService.Database.DBContextFiles;
using catalogueService.Database;
using System.Security.Claims;

namespace catalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUser _userRep;
        private readonly IMapper _mapper;

        public UsersController(IUser userRep, IMapper mapper)
        {
            _userRep = userRep;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _userRep.GetUserAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<userModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] userRequest Mboko)
        {
            try
            {
                var domainUser = _mapper.Map<users>(Mboko);

                // Pass details to Repositpory
                domainUser = await _userRep.AddUserAsync(domainUser);
                if (domainUser == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var customerDTO = _mapper.Map<userModel>(domainUser);

                return Ok(new Response { response = $"New {customerDTO.role} added successfully" });
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Super Admin, Teacher, Student")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var repoUser = await _userRep.GetByIdAsync(id);
            if (repoUser == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var userDTO = _mapper.Map<userModel>(repoUser);

            return Ok(userDTO);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(int id, userRequest userRequest)
        {
            // Convert DTO to Domain Model
            var userDomainN = _mapper.Map<users>(userRequest);
            //Call repository to update
            userDomainN = await _userRep.UpdateAsync(id, userDomainN);
            if (userDomainN == null)
            {
                return NotFound();
            }
            //Convert Domain to DTO
            var userDTO = _mapper.Map<userModel>(userDomainN);

            //Return response
            return Ok(new Response { response = $"User info updated sucessfully" });
        }

        private users GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaim = identity.Claims;
                return new users
                {
                    userName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    //userName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    firstName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    lastName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    role = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };

            }
            return null;
        }
    }
}
