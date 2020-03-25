using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebSite.DtoParams;

namespace WebSite.Aunth
{
    public class JwtTokenUtil
    {
        private readonly IConfiguration Configuration;

        public JwtTokenUtil(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //获取Token
        public string GetToken(TokenRequest request)
        {
            var liams = new[] {
                    new Claim(ClaimTypes.Name, request.UserName),
                    new Claim(JwtRegisteredClaimNames.Nbf,
                    $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,
                    $"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}")
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                   issuer: "WebSite",
                   audience: "WebSite",
                   claims: liams,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: creds);

            string TokenText = new JwtSecurityTokenHandler().WriteToken(token);
            return TokenText;
        }
    }
}
