using BankAccount.Model;
using BankAccount.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BankAccount.Model.AdminModel;

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
        [Route("MyAccount/{AccountNumber}")]
        public async Task<IActionResult> MyAccount(long AccountNumber)
        {
            try
            {
                var AccountDetails = await _customer.GetAccountById(AccountNumber);

                if (AccountDetails != null && AccountDetails.Any())
                {
                    return Ok(AccountDetails);
                }
                else
                {
                    return NotFound("No Account details found or Not Activate your account.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTransactions/{accountNumber}")]
        public async Task<IActionResult> GetTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _customer.GetTransactionsByAccountNumber(accountNumber);

                if (transactions != null && transactions.Count > 0)
                {
                    return Ok(transactions);
                }
                else
                {
                    return NotFound("No transactions found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("BalanceEnquriey/{AccountNumber}")]
        public async Task<IActionResult> BalanceEnquriey(long AccountNumber)
        {
            var balance = await _customer.GetAccountBalanceAsync(AccountNumber);
            if (balance == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(new { Balance = balance });
        }

        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            try
            {
                var (resultMessage, accountNumber) = await _customer.CreateAccountAsync(request);

                if (accountNumber.HasValue)
                {
                    return Ok(new { Message = resultMessage, AccountNumber = accountNumber.Value });
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
        public async Task<IActionResult> Withdraw( WithdrawalRequest request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid request data.");
            }

            var resultMessage = await _customer.WithdrawAsync(request);

            if (resultMessage == "Withdrawal successful.")
            {
                return Ok(resultMessage);
            }

            return BadRequest(resultMessage);
        }


        [HttpPost]
        [Route("Deposit")]
        public async Task<IActionResult> Deposit([FromBody] WithdrawalRequest request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid request data.");
            }

            var resultMessage = await _customer.DepositAsync(request);

            if (resultMessage == "Deposit successful.")
            {
                return Ok(resultMessage);
            }

            return BadRequest(resultMessage);
        }



    }
}
