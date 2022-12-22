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
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace catalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrder orderRep;
        private readonly IUser userRep;
        private readonly IMapper _mapper;
        private readonly IProduct _productRep;
        private readonly IConfiguration _configuration;
        private readonly ICustomer _customerRep;

        public OrdersController(IOrder orderRep, IMapper mapper, IUser userRep, IProduct productRep, IConfiguration config, ICustomer customerRep)
        {
            this.orderRep = orderRep;
            this._mapper = mapper;
            this.userRep = userRep;
            this._productRep = productRep;
            this._configuration = config;
            this._customerRep = customerRep;
        }

        [HttpPost("Place Order")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] orderRequest Mboko)
        {
            try
            {
                var customer = await _customerRep.GetByIdAsync(Mboko.customerId);
                if (customer == null)
                {
                    return Ok($"No such customer with ID: {Mboko.customerId}");
                }
                var customerFirstName = customer.firstName;
                var customerLastName = customer.lastName;
                var customerPhoneNumber = customer.phoneNumber;
                var customerUserID = customer.userId;
                var user = await userRep.GetByIdAsync(customerUserID);
                if (user == null)
                {
                    return Ok($"No such user with ID: {customerUserID}");
                }
                if (customerFirstName == user.firstName && customerLastName == user.lastName && customerPhoneNumber == user.phoneNumber)
                {
                    var productID = Mboko.productId;
                    var thisProduct = await _productRep.GetByIdAsync(productID);
                    if (thisProduct == null)
                    {
                        return Ok($"No such product with ID: {productID}");
                    }
                    var productCost = (Mboko.quantity) * (thisProduct.price);
                    var inputedCost = (Mboko.quantity) * (Mboko.amount);
                    //var inputedCost = Mboko.amount;
                    if (inputedCost == productCost)
                    {
                        Mboko.amount = inputedCost;
                        var domainOrder = _mapper.Map<orders>(Mboko);

                        // Pass details to Repositpory
                        domainOrder = await orderRep.AddOrderAsync(domainOrder);
                        if (domainOrder == null)
                        {
                            return NotFound();
                        }

                        var orderStatus = "Not Paid";
                        var orderID = domainOrder.orderID;
                        //var orderID2 = domainOrder.orderID;
                        using var connection3 = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                        var query3 = "Update orders set orderStatus = @orderStatus where orderID = @orderID";

                        using var command3 = new SqlCommand(query3, connection3);

                        command3.Parameters.AddWithValue("@orderStatus", orderStatus);
                        command3.Parameters.AddWithValue("@orderID", orderID);

                        try
                        {
                            if (connection3.State != ConnectionState.Open)
                            {
                                await connection3.OpenAsync();
                            }
                            var rowsAffected = await command3.ExecuteNonQueryAsync();
                            await connection3.CloseAsync();
                            if (rowsAffected < 1) return Ok("Unable to execute sql command on updating order status");
                        }
                        catch (Exception e)
                        {
                            var errorMessage = e.Message;
                            return Ok(errorMessage);
                        }
                        finally
                        {
                            if (connection3.State != ConnectionState.Closed)
                            {
                                await connection3.CloseAsync();
                            }
                        }

                        // Convert back to DTO
                        var orderDTO = _mapper.Map<orderModel>(domainOrder);

                        return Ok(new Response { response = $"Order successfully placed with order ID of {orderDTO.orderID}" });
                    }
                    else
                    {
                        return Ok($"Wrong Amount for the product '{thisProduct.name}'");
                    }
                }
                else
                {
                    return Ok("Wrong customer ID, Kindly retry with your valid Customer Id");
                }                
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpGet("View Orders")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await orderRep.GetOrdersAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<orderModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpGet("View Order by ID")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var repoOrder = await orderRep.GetByIdAsync(id);
            if (repoOrder == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var orderDTO = _mapper.Map<supplierModel>(repoOrder);

            return Ok(orderDTO);
        }
        

        [HttpPost("Checkout")]
        public async Task<IActionResult> PurchaseAsync(int orderID)
        {
            try
            {
                var domainOrder = await orderRep.PurchaseAsync(orderID);
                if (domainOrder != null)
                {
                    var orderAmount = domainOrder.amount;
                    var userId = GetCurrentUser().userId;
                    var thisUser = await userRep.GetByIdAsync(userId);
                    if (thisUser == null)
                    {
                        return Ok($"No such user with ID: {userId}");
                    }
                    if (thisUser.wallet == null)
                    {
                        return Ok("Your wallet balance is empty, Kindly fund your wallet");
                    }
                    var availableBalanace = decimal.Parse(thisUser.wallet);

                    if (domainOrder.orderStatus.ToString().ToUpper() != "PAID")
                    {

                        if (availableBalanace < orderAmount)
                        {
                            return Ok("Insufficient funds, kindly fund your wallet");
                        }
                        else
                        {
                            var productID = domainOrder.productId;
                            var thisProduct = await _productRep.GetByIdAsync(productID);
                            var datePaid = DateTime.Now;
                            var salesType = thisProduct.name;
                            var amount = domainOrder.amount;
                            var customerID = domainOrder.customerId;
                           
                            if (thisProduct.quantity > domainOrder.quantity)
                            {
                                using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                                var query = "Insert Into sales(datePaid, salesType, amount, customerID) Values(@datePaid, @salesType, @amount, @customerID)";

                                using var command = new SqlCommand(query, connection);

                                command.Parameters.AddWithValue("@datePaid", datePaid);
                                command.Parameters.AddWithValue("@salesType", salesType);
                                command.Parameters.AddWithValue("@amount", amount);
                                command.Parameters.AddWithValue("@customerID", customerID);

                                try
                                {
                                    if (connection.State != ConnectionState.Open)
                                    {
                                        await connection.OpenAsync();
                                    }
                                    var rowsAffected = await command.ExecuteNonQueryAsync();
                                    await connection.CloseAsync();
                                    if (rowsAffected < 1) return Ok("Unable to execute sql command on inserting to sales table");
                                    //return rowsAffected;
                                }
                                catch (Exception e)
                                {
                                    var exceptionMessage = e.Message;
                                    return Ok(exceptionMessage);
                                }
                                finally
                                {
                                    if (connection.State != ConnectionState.Closed)
                                    {
                                        await connection.CloseAsync();
                                    }
                                }

                                var wallet = (availableBalanace - orderAmount).ToString();
                                using var connection2 = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                                var query2 = "Update users set wallet = @wallet where userId = @userId";

                                using var command2 = new SqlCommand(query2, connection2);

                                command2.Parameters.AddWithValue("@wallet", wallet);
                                command2.Parameters.AddWithValue("@userId", userId);

                                try
                                {
                                    if (connection2.State != ConnectionState.Open)
                                    {
                                        await connection2.OpenAsync();
                                    }
                                    var rowsAffected = await command2.ExecuteNonQueryAsync();
                                    await connection2.CloseAsync();
                                    if (rowsAffected < 1) return Ok("Unable to execute sql command on updating user's balance");
                                }
                                catch (Exception e)
                                {
                                    var exceptionMessage = e.Message;
                                    return Ok(exceptionMessage);
                                }
                                finally
                                {
                                    if (connection2.State != ConnectionState.Closed)
                                    {
                                        await connection2.CloseAsync();
                                    }
                                }

                                var orderStatus = "Paid";
                                //var orderID2 = domainOrder.orderID;
                                using var connection3 = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                                var query3 = "Update orders set orderStatus = @orderStatus where orderID = @orderID";

                                using var command3 = new SqlCommand(query3, connection3);

                                command3.Parameters.AddWithValue("@orderStatus", orderStatus);
                                command3.Parameters.AddWithValue("@orderID", orderID);

                                try
                                {
                                    if (connection3.State != ConnectionState.Open)
                                    {
                                        await connection3.OpenAsync();
                                    }
                                    var rowsAffected = await command3.ExecuteNonQueryAsync();
                                    await connection3.CloseAsync();
                                    if (rowsAffected < 1) return Ok("Unable to execute sql command on updating order status");
                                }
                                catch (Exception e)
                                {
                                    var errorMessage = e.Message;
                                    return Ok(errorMessage);
                                }
                                finally
                                {
                                    if (connection3.State != ConnectionState.Closed)
                                    {
                                        await connection3.CloseAsync();
                                    }
                                }

                                var quantity = (thisProduct.quantity) - ((await orderRep.GetByIdAsync(orderID)).quantity);
                                using var connection4 = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                                var query4 = "Update products set quantity = @quantity where productID = @productID";

                                using var command4 = new SqlCommand(query4, connection4);

                                command4.Parameters.AddWithValue("@quantity", quantity);
                                command4.Parameters.AddWithValue("@productID", productID);

                                try
                                {
                                    if (connection4.State != ConnectionState.Open)
                                    {
                                        await connection4.OpenAsync();
                                    }
                                    var rowsAffected = await command4.ExecuteNonQueryAsync();
                                    await connection4.CloseAsync();
                                    if (rowsAffected < 1) return Ok("Unable to execute sql command on updating new quantity of products");
                                }
                                catch (Exception e)
                                {
                                    var errorMessage = e.Message;
                                    return Ok(errorMessage);
                                }
                                finally
                                {
                                    if (connection4.State != ConnectionState.Closed)
                                    {
                                        await connection4.CloseAsync();
                                    }
                                }

                                return Ok("Payment Successful");
                            }

                            return Ok($"Only {thisProduct.quantity} slots are available for this product");
                        }
                    }
                    return Ok("Product has already been purchased, Kindly place a new order");
                }
                return Ok($"No existing order for the order ID: {orderID}");
            }
            catch (Exception oxg)
            {
                return BadRequest(oxg.Message);
            }
        }

        [HttpPost("Cancel Order")]
        public async Task<IActionResult> CancelOrderAsync(int orderID)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            var query = "delete from orders where orderID = @orderID";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@orderID", orderID);        

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }
                var rowsAffected = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                if (rowsAffected < 1) return Ok("Unable to execute sql command on deleting an order");
                //return rowsAffected;
            }
            catch (Exception e)
            {
                var exceptionMessage = e.Message;
                return Ok(exceptionMessage);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    await connection.CloseAsync();
                }
            }
            return Ok("Order cancelled successfully");
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
                    userId = Convert.ToInt16(userClaim.FirstOrDefault(o => o.Type == ClaimTypes.UserData)?.Value),
                    firstName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    lastName = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    role = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };

            }
            return null;
        }
    }
}
