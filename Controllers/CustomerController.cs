using BankAccount.Model;
using BankAccount.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }

        #region Get Method
        [HttpGet]
        [Route("MyProfile/{CustomerId}")]
        public async Task<IActionResult> Myprofile(int CustomerId)
        {
            try
            {
                var customerProfile = await _customer.GetCustomerById(CustomerId);

                if (customerProfile != null && customerProfile.Any())
                {
                    return Ok(customerProfile);
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

        [HttpGet]
        [Route("MyAccount")]
        public async Task<IActionResult> MyAccount(int account_number)
        {
            try
            {
                var AccountDetails = await _customer.GetCustomerById(account_number);

                if (AccountDetails != null && AccountDetails.Any())
                {
                    return Ok(AccountDetails);
                }
                else
                {
                    return NotFound("No Account details found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("Transaction")]
        public async Task<IActionResult> Transaction()
        {
            return Ok();
        }

        [HttpGet]
        [Route("BalanceEnquriey")]
        public async Task<IActionResult> BalanceEnquriey()
        {
            return Ok();
        }

        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            return Ok();
        }


        #endregion
        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer(PostCustomer customer)
        {
            try
            {
                var resultMessage = await _customer.AddNewUser(customer);

                if (resultMessage.Contains("successfully"))
                {
                    return Ok(resultMessage);
                }
                else
                {
                    return BadRequest(resultMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Route("Withdraw")]
        public async Task<IActionResult> Withdraw()
        {
            return Ok();
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<IActionResult> Deposit()
        {
            return Ok();
        }



    }
}
