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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace catalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _productRep;
        private readonly IMapper _mapper;
        //private readonly catalogueDBContext _dbcontext;

        public ProductsController(IMapper LampMap, IProduct productRep)
        {
            this._mapper = LampMap;
            this._productRep = productRep;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var currentuser = GetCurrentUser();
            if (currentuser != null)
            {
                var (IsSucess, mbokoDomain) = await _productRep.GetProductAsync();
                if (!IsSucess)
                {
                    return NotFound();
                }
                var mbokoDTO = _mapper.Map<IEnumerable<productModel>>(mbokoDomain);
                return Ok(mbokoDTO);
            }
            return Ok("User not Authorized");
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var walkDifficulty = await _productRep.GetByIdAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDifficultyDTO = _mapper.Map<productModel>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Teacher")]
        //[Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddProductAsync([FromBody] productRequest Mboko)
        {
            try
            {
                var walk = new product()
                {
                    name = Mboko.name,
                    quantity = Mboko.quantity,
                    categoryId = Mboko.categoryId,
                    price = Mboko.price,
                    description = Mboko.description,

                };

                // Pass details to Repositpory
                walk = await _productRep.AddProductAsync(walk);

                // Convert back to DTO
                var walkDTO = new productModel
                {
                    productId = walk.productId,
                    name = walk.name,
                    quantity = walk.quantity,
                    categoryId = walk.categoryId,
                    price = walk.price,
                    description = walk.description,
                };

                return Ok(new Response { response = $"Product added successfully with product ID of {walkDTO.productId}" });
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Super Admin, Teacher")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            //Get a region from database
            var walk = await _productRep.DeleteAsync(id);


            //if null NotFound
            if (walk == null)
            {
                return NotFound();
            }
            //Convert response back to DTO
            var walkDTO = new productModel
            {
                productId = walk.productId,
                name = walk.name,
                quantity = walk.quantity,
                categoryId = walk.categoryId,
                price = walk.price,
                description = walk.description,
            };

            return Ok(new Response { response = $"Product with the ID:{walkDTO.productId}, has been deleted sucessfully " });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Super Admin, Teacher")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(int id, productRequest productRequest)
        {

            // Convert DTO to Domain Model
            var walkDifficultyDomainN = _mapper.Map<product>(productRequest);
            //Call repository to update
            walkDifficultyDomainN = await _productRep.UpdateAsync(id, walkDifficultyDomainN);
            if (walkDifficultyDomainN == null)
            {
                return NotFound();
            }
            //Convert Domain to DTO
            var walkDifficultyDTO = _mapper.Map<productModel>(walkDifficultyDomainN);

            //Return response
            return Ok(new Response { response = $"Product with the ID:{walkDifficultyDTO.productId}, has been updated sucessfully " });
        }


    }
}
