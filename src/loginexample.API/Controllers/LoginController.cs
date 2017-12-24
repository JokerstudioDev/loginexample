using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using loginexample.API.DAC;
using loginexample.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace loginexample.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IAccountDAC accountDac;

        public LoginController(IAccountDAC accountDac)
        {
            this.accountDac = accountDac;
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            var requestIsInvalid = string.IsNullOrEmpty(user.Username)
                                   || string.IsNullOrEmpty(user.Password);
            if (requestIsInvalid) return BadRequest();

            var userInfo = accountDac.GetUser(user.Username, user.Password);
            if (userInfo != null)
            {
                return new OkObjectResult(userInfo);
            }
            else
            {
                return new UnauthorizedResult();
            }

        }
    }
}
