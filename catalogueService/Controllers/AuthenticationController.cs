using catalogueService.Authentication;
using catalogueService.Database.DBContextFiles;
using catalogueService.Database.DBsets;
using catalogueService.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using catalogueService.requestETresponse;
using catalogueService.Interfaces;
using catalogueService.Repositories;
using AutoMapper;
using System.Data;
using System.Data.SqlClient;
using catalogueService.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    //[Authorize(Roles = "Super Admin")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly catalogueDBContext _dbcontext;
        private readonly IUser _registerUser;
        private readonly IMapper _mapper;
        private readonly ICustomer _customerRep;

        public AuthenticateController(IConfiguration _configuration, catalogueDBContext _dbcontext, IUser _registerUser, IMapper mapper, ICustomer customerRep)
        {
            this._configuration = _configuration;
            this._dbcontext = _dbcontext;
            this._registerUser = _registerUser;
            this._mapper = mapper;
            _customerRep = customerRep;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("User Not Registered");

            string Generate(users user)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.userName),
                new Claim(ClaimTypes.Email, user.userName),
                new Claim(ClaimTypes.GivenName, user.firstName),
                new Claim(ClaimTypes.Surname, user.lastName),
                new Claim(ClaimTypes.Role, user.role),
                new Claim(ClaimTypes.UserData, user.userId.ToString())};

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            users Authenticate(LoginModel userLogin)
            {
                var currentUser = _dbcontext.Users.FirstOrDefault(o => o.userName.ToLower() == userLogin.userName.ToLower() && o.password == userLogin.password);
                if (currentUser != null)
                {
                    return currentUser;
                }

                return null;
            }
        }
        
        //[AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] authUserRequest Mboko)
        {
            try
            {
                var users = await _dbcontext.Users.ToListAsync();
                foreach (var user in users)
                {
                    if (user.userName == Mboko.userName)
                    {
                        return Ok("Username already taken");
                    }
                }
                var domainUser = _mapper.Map<users>(Mboko);

                // Pass details to Repositpory
                domainUser = await _registerUser.AddUserAsync(domainUser);
                if (domainUser == null)
                {
                    return NotFound();
                }

                //var firstName = domainUser.firstName;
                //var lastName = domainUser.lastName;
                //var phoneNumber = domainUser.phoneNumber;
                //using var connection3 = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                //var query3 = "Insert Into customer(firstName, lastName, phoneNumber) Values(@firstName, @lastName, @phoneNumber)";

                //using var command3 = new SqlCommand(query3, connection3);

                //command3.Parameters.AddWithValue("@firstName", firstName);
                //command3.Parameters.AddWithValue("@lastName", lastName);
                //command3.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                //try
                //{
                //    if (connection3.State != ConnectionState.Open)
                //    {
                //        await connection3.OpenAsync();
                //    }
                //    var rowsAffected = await command3.ExecuteNonQueryAsync();
                //    await connection3.CloseAsync();
                //    if (rowsAffected < 1) return Ok("Unable to execute sql command on updating user info into customer's table");
                //}
                //catch (Exception e)
                //{
                //    var errorMessage = e.Message;
                //    return Ok(errorMessage);
                //}
                //finally
                //{
                //    if (connection3.State != ConnectionState.Closed)
                //    {
                //        await connection3.CloseAsync();
                //    }
                //}
                var newCustomerRequest = new customer()
                {
                    firstName = domainUser.firstName,
                    lastName = domainUser.lastName,
                    phoneNumber = domainUser.phoneNumber,
                    userId = domainUser.userId,
                };
                var addCustomer = await _customerRep.AddCustomerAsync(newCustomerRequest);
                var customerDTO = _mapper.Map<customerModel>(addCustomer);

                return Ok(new registrationResponse { response = $"Registration Sucessful", message = $"Your user ID is {customerDTO.userId}, and your customer ID is {customerDTO.customerId}" });
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await userManager.FindByNameAsync(model.userName);
        //    if (user != null && await userManager.CheckPasswordAsync(user, model.password))
        //    {
        //        var userRoles = await userManager.GetRolesAsync(user);

        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["JWT:ValidIssuer"],
        //            audience: _configuration["JWT:ValidAudience"],
        //            expires: DateTime.Now.AddHours(3),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //            );

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }
        //    return Unauthorized();
        //}

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] users model)
        //{
        //    var userExists = await userManager.FindByNameAsync(model.userName);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticationResponse { Status = "Error", Message = "User already exists!" });

        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        Email = model.userName,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.userName                
        //    };
        //    var result = await userManager.CreateAsync(user, model.password);
        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticationResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    return Ok(new AuthenticationResponse { Status = "Success", Message = "User created successfully!" });
        //}

    }
}