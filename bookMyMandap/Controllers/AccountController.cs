using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookMyMandap.Models;
using bookMyMandap.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bookMyMandap.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
           // ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           // _logger = logger;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!model.Email.Contains("@gmail.com"))
            {return BadRequest("You r not a gmail user.");
            }

                
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    return Ok("User created a new account with password.");
                }
            

            // If we got this far, something failed, redisplay form
            return BadRequest(result.Errors);
        }
    }
}