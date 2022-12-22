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
    [Authorize(Roles = "Super Admin")]
    public class EmployeesController : Controller
    {
        private readonly IEmployee _employeeRep;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployee employeeRep, IMapper mapper)
        {
            this._employeeRep= employeeRep;
            this._mapper= mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _employeeRep.GetEmployeeAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<employeeModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] employeeRequest Mboko)
        {
            try
            {
                var domainEmployee = _mapper.Map<employees>(Mboko);

                // Pass details to Repositpory
                domainEmployee = await _employeeRep.AddEmployeeAsync(domainEmployee);
                if (domainEmployee == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var employeeDTO = _mapper.Map<employeeModel>(domainEmployee);

                return Ok(new Response { response = $"Employee added successfully with job ID of {employeeDTO.jobId}" });
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var repoEmployee = await _employeeRep.GetByIdAsync(id);
            if (repoEmployee == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var employeeDTO = _mapper.Map<employeeModel>(repoEmployee);

            return Ok(employeeDTO);
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
