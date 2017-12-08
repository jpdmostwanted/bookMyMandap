using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bookMyMandap.Models;
using bookMyMandap.Models.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bookMyMandap.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;


        public TokenController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Super secret content, I hope you've got clearance for this...");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestTokenAsync([FromBody] TokenRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == request.Email);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "bookmymandap.com",
                    audience: "bookmymandap.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            //Demo Code dont remove
            //if (request.Username == "jpdmw" && request.Password == "password")
            //{
            //    var claims = new[]
            //    {
            //        new Claim(ClaimTypes.Name, request.Username)
            //    };

            //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //    var token = new JwtSecurityToken(
            //        issuer: "bookmymandap.com",
            //        audience: "bookmymandap.com",
            //        claims: claims,
            //        expires: DateTime.Now.AddMinutes(30),
            //        signingCredentials: creds);

            //    return Ok(new
            //    {
            //        token = new JwtSecurityTokenHandler().WriteToken(token)
            //    });
            //}

            return BadRequest("Could not verify username and password");
        }
    }
}