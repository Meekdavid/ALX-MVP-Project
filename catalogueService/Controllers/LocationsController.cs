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
    public class LocationsController : Controller
    {
        private readonly ILocation _locationRep;
        private readonly IMapper _mapper;

        public LocationsController(ILocation locationRep, IMapper mapper)
        {
            this._locationRep = locationRep;
            this._mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _locationRep.GetLocationAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<locationModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddLocationAsync([FromBody] locationRequest Mboko)
        {
            try
            {
                var domainLocation = _mapper.Map<location>(Mboko);

                // Pass details to Repositpory
                domainLocation = await _locationRep.AddLocationAsync(domainLocation);
                if (domainLocation == null)
                {
                    return NotFound();
                }
                // Convert back to DTO
                var locationDTO = _mapper.Map<locationModel>(domainLocation);

                return Ok(new Response { response = $"Location added successfully with location ID of {locationDTO.locationId}" });
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
            var repoLocation = await _locationRep.GetByIdAsync(id);
            if (repoLocation == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var locationDTO = _mapper.Map<locationModel>(repoLocation);

            return Ok(locationDTO);
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
