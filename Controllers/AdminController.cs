using BankAccount.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdmin _admin;

        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }
        [HttpGet]
        [Route("GetAllByAccount")]
        public async Task<IActionResult> GetAllByAccount()
        {
            var accountDetails = await _admin.GetAllByAccountAsync();

            if (accountDetails == null || !accountDetails.Any())
            {
                return NotFound("No accounts found or invalid account number.");
            }

            return Ok(accountDetails);
        }

        [HttpGet]
        [Route("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById()
        {
            try
            {
                var customerProfiles = await _admin.GetAllCustomersAsync();

                if (customerProfiles != null && customerProfiles.Any())
                {
                    return Ok(customerProfiles);
                }
                else
                {
                    return NotFound("No customers found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut]
        [Route("UpdateBankActiveState")]
        public async Task<IActionResult> UpdateBankActiveState(int customerId)
        {
            try
            {
                bool success = await _admin.UpdateBankActiveStatusAsync(customerId);
                if (success)
                {
                    return Ok("Bank active status updated successfully.");
                }
                else
                {
                    return NotFound("No accounts found for the provided customer ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateCardDetails")]
        public async Task<IActionResult> UpdateCardDetails(CardDetails cardDetails)
        {
            try
            {
                string resultMessage = await _admin.UpdateCardDetailsAsync(cardDetails);
                return Ok(resultMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
