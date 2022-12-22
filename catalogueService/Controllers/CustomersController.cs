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
    public class CustomersController : Controller
    {
        private readonly ICustomer _customerRep;
        private readonly IMapper _mapper;

        public CustomersController(ICustomer customerRep, IMapper mapper)
        {
            _customerRep = customerRep;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var currentUser = GetCurrentUser();
            if (currentUser.role.ToString().ToUpper() == "SUPER ADMIN")
            {
                var (IsSucess, mbokoDomain) = await _customerRep.GetCustomerAsync();
                if (!IsSucess)
                {
                    return NotFound();
                }
                var mbokoDTO = _mapper.Map<IEnumerable<customerModel>>(mbokoDomain);
                return Ok(mbokoDTO);
            }
            else
            {
                return Ok("This can only be accessed by an Administrator");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync([FromBody] customerRequest Mboko)
        {
            try
            {
                var domainCustomer = _mapper.Map<customer>(Mboko);

                // Pass details to Repositpory
                domainCustomer = await _customerRep.AddCustomerAsync(domainCustomer);
                if (domainCustomer == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var customerDTO = _mapper.Map<customerModel>(domainCustomer);

                return Ok(new Response { response = $"Customer added successfully with customer ID of {customerDTO.customerId}" });
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
            var repoCuustomer = await _customerRep.GetByIdAsync(id);
            if (repoCuustomer == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var customerDTO = _mapper.Map<customerModel>(repoCuustomer);

            return Ok(customerDTO);
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
