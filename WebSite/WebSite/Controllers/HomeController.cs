using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebSite.DtoParams;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using WebSite.Infrastructure;
using WebSite.Model;

namespace WebSite.Controllers
{
    [Route("home/[Action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MhDbContext Client = MhDbContext.GetInstance();

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //GET Home/Index
        [HttpGet]
        public ActionResult Index()
        {
            //HttpContext.Session.SetString("Monster", "123");
            return Ok("hello world");
        }

        //GET Home/Error
        [AllowAnonymous]
        public ActionResult Error()
        {
            //var RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var Code = HttpContext.Response.StatusCode.ToString();
            return Ok("Error!!StatusCode:" + Code);
        }

        // POST Home/Login
        [HttpPost]
        public ActionResult Login([FromForm]string UserName, [FromForm]string PassWord)
        {
            var GetByUserInfo = Client.Db.Queryable<UserInfo>()
                                .Where(it => it.UserName == UserName || it.PassWord == PassWord);

            if (GetByUserInfo != null)
            {
                var liams = new[] {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                   issuer: "WebSite",
                   audience: "WebSite",
                   claims: liams,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return BadRequest("Not Acess");
        }
    }
}