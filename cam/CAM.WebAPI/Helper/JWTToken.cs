using CAM.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CAM.WebAPI.Helper
{
    public class TokenObject
    {
        public string Token { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
        public int ExpirationPeriod { get; set; }
    }
    public static class JwtTokenParameters
    {
        public static TokenValidationParameters RetrieveTokenParamters()
        {
            var key = Encoding.Default.GetBytes("ProEMLh5e_qnzdNUQrqdHPgp");
            const string sec1 = "ProEMLh5e_qnzdNU";
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1)),
                // Allow to use seconds for expiration of toke
                // Required only when token lifetime less than 5 minutes
                // THIS ONE
                ClockSkew = TimeSpan.Zero,


            };
        }
    }
    public static class JWTToken
    {

        public static TokenObject GenerateJwtToken(UserInfoModel userInfoModel)
        {
            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";
            const string sec1 = "ProEMLh5e_qnzdNU";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));
            var signingCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha512);

            var tokenHandler = new JwtSecurityTokenHandler();
            var Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", userInfoModel.UserID.ToString()) ,
                    new Claim("Email", userInfoModel.Email.ToString()) ,
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });
            var ep = new EncryptingCredentials(
                    securityKey1,
                    SecurityAlgorithms.Aes128KW,
                    SecurityAlgorithms.Aes128CbcHmacSha256);
            foreach (var item in userInfoModel.RolesName)
            {
                Subject.AddClaim(new Claim("Role", item));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Subject,
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = "http://localhost:5000",
                Audience = "http://localhost:5000",
                SigningCredentials = signingCredentials,
                EncryptingCredentials = ep,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            var refreshToken = new RefreshTokenModel()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = userInfoModel.UserID,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(60),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()
            };


            return new TokenObject() { RefreshToken = refreshToken, Token = result, ExpirationPeriod = 60 };
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static async Task<string> GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            await System.Threading.Tasks.Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
        public static int? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

                // return account id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }


}
