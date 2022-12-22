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
using catalogueService.Database.DBsets;

namespace catalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Super Admin")]
    public class SuppliersController : Controller
    {
        private readonly ISupplier _supplierRep;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplier supplierRep, IMapper mapper)
        {
            _supplierRep = supplierRep;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _supplierRep.GetSupplierAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<supplierModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] supplierRequest Mboko)
        {
            try
            {
                var domainSupplier = _mapper.Map<supplier>(Mboko);

                // Pass details to Repositpory
                domainSupplier = await _supplierRep.AddSupplierAsync(domainSupplier);
                if (domainSupplier == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var supplierDTO = _mapper.Map<supplierModel>(domainSupplier);

                return Ok(new Response { response = $"Supplier added successfully with supplier ID of {supplierDTO.supplierId}" });
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
            var repoSupplier = await _supplierRep.GetByIdAsync(id);
            if (repoSupplier == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var supplierDTO = _mapper.Map<supplierModel>(repoSupplier);

            return Ok(supplierDTO);
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
