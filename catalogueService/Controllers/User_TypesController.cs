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
    public class User_TypesController : Controller
    {
        private readonly IType _typeRep;
        private readonly IMapper _mapper;

        public User_TypesController(IType typeRep, IMapper mapper)
        {
            _typeRep = typeRep;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _typeRep.GetTypeAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<typeModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] typeRequest Mboko)
        {
            try
            {
                var domaintype = _mapper.Map<type>(Mboko);

                // Pass details to Repositpory
                domaintype = await _typeRep.AddTypeAsync(domaintype);
                if (domaintype == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var customerDTO = _mapper.Map<typeModel>(domaintype);

                return Ok(new Response { response = $"New user type added successfully" });
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
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
