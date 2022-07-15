using System.Collections.Generic;
using System.Security.Claims;
using test_wallet.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using test_wallet.HelperFunctions.Implementation;

namespace test_wallet.HelperFunctions
{
    public class TokenAuthorization : ITokenAuthorization
    {
        public IConfiguration _configuration;

        public TokenAuthorization(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string CreateToken(WalletModel user)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, user.FirstName));

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Secrets:TokenSecret"]));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.Now.AddDays(1)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
