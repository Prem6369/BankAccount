using BankAccount.Model;
using BankAccount.Repository.Interface;
using BankAccount.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;
        private readonly InvokeApi _invokeApi;
        private readonly IConfiguration _configuration;

        public CustomerController(ICustomer customer, InvokeApi invokeApi,IConfiguration configuration)
        {
            _customer = customer;
            _invokeApi = invokeApi;
            _configuration = configuration;
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




        #endregion



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

                var baseurl = _configuration["ApiClient"];
                string path = "CreateCard";
                string app = "CardTransaction";
                var (resultMessage, accountNumber) = await _customer.CreateAccountAsync(request);

                if (accountNumber.HasValue)
                {
                    var requestModel = new
                    {
                        CustomerId = request.customerId,
                        CardType = request.cardType
                    };
                    _invokeApi.SendToClient(baseurl,app, path, requestModel);
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

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer(PostCustomer customer)
        {
            try
            {
                var baseurl = _configuration["ApiClient"];
                string path = "CreateCustomer";
                string app = "CardTransaction";
               
                (string resultMessage, int customerId) = await _customer.AddNewUser(customer);

                if (resultMessage.Contains("successfully"))
                {
                    var requestModel = new
                    {
                        CustomerId = customerId,
                        Name = customer.CustomerName,
                        Age = customer.Age,
                        Address = customer.Address,
                        Email = customer.Email,
                        PhoneNumber = customer.PhoneNumber
                    };

                    await _invokeApi.SendToClient(baseurl,app,path, requestModel);
                    return Ok(new { Message = resultMessage, CustomerId = customerId });
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
