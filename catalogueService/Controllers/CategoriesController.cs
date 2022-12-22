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
    public class CategoriesController : Controller
    {
        private readonly ICategory _categoryRep;
        private readonly IMapper _mapper;

        public CategoriesController(ICategory _categoryRep, IMapper _mapper)
        {
            this._categoryRep= _categoryRep;
            this._mapper= _mapper;
        }

        [HttpGet("All Categories")]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                if (currentUser.role.ToString().ToUpper() == "SUPER ADMIN")
                {
                    var (IsSucess, mbokoDomain) = await _categoryRep.GetCategoryAsync();
                    if (!IsSucess)
                    {
                        return NotFound();
                    }
                    var mbokoDTO = _mapper.Map<IEnumerable<categoryModel>>(mbokoDomain);
                    return Ok(mbokoDTO);
                }
                else
                {
                    return Ok("This can only be accessed by an Administrator");
                }
            }
            return Ok("Kindly register/login for access");
        }

        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] categoryRequest Mboko)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser != null)
                {
                    if (currentUser.role.ToString().ToUpper() == "SUPER ADMIN")
                    {
                        var domainCategory = _mapper.Map<category>(Mboko);

                        // Pass details to Repositpory
                        domainCategory = await _categoryRep.AddCategoryAsync(domainCategory);
                        if (domainCategory == null)
                        {
                            return NotFound();
                        }
                        // Convert back to DTO
                        var categoryDTO = _mapper.Map<categoryModel>(domainCategory);

                        return Ok(new Response { response = $"Category added successfully with category ID of {categoryDTO.categoryId}" });
                    }
                    else
                    {
                        return Ok("This can only be accessed by an Administrator");
                    }
                }
                return Ok("Kindly register/login for access");
                
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                if (currentUser.role.ToString().ToUpper() == "SUPER ADMIN")
                {
                    var repoCategory = await _categoryRep.GetByIdAsync(id);
                    if (repoCategory == null)
                    {
                        return NotFound();
                    }

                    //Convert Domain to DTO
                    var categoryDTO = _mapper.Map<categoryModel>(repoCategory);

                    return Ok(categoryDTO);
                }
                else
                {
                    return Ok("This can only be accessed by an Administrator");
                }
            }
            return Ok("Kindly register/login for access");
           
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
