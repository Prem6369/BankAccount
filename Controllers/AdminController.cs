using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllByAccount")]
        public async Task<IActionResult> GetAllByAccount()
        {
            return Ok();
        }

        [HttpGet]
        [Route("GetAllByCustomer")]
        public async Task<IActionResult> GetAllByCustomer()
        {
            return Ok();
        }
             

        [HttpPut]
        [Route("UpdateBankActiveState")]
        public async Task<IActionResult> UpdateBankActiveState()
        {
            return Ok();
        }

         [HttpPut]
        [Route("UpdateCardDetails")]
        public async Task<IActionResult> UpdateCardDetails()
        {
            return Ok();
        }



    }
}
