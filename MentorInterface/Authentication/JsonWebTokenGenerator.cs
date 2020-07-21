using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MentorInterface.Authentication
{
    public interface IJsonWebTokenGenerator
    {
        string GenerateJSONWebToken(ApplicationUser user);
    }

    public class JsonWebTokenGenerator : IJsonWebTokenGenerator
    {
        private ILogger<JsonWebTokenGenerator> _logger;

        private string _issuer;

        private string _audience;

        private string _signingKey;

        private TimeSpan _tokenValidity;


        public JsonWebTokenGenerator(
            string signingKey,
            string issuer,
            string audience,
            TimeSpan tokenValidity,
            ILogger<JsonWebTokenGenerator> logger
        )
        {
            _issuer = issuer;
            _audience = audience;
            _signingKey = signingKey;
            _tokenValidity = tokenValidity;
            _logger = logger;

        }
        public string GenerateJSONWebToken(ApplicationUser user)
        {
            // https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
            _logger.LogInformation($"Creating Token for User [ {user.SteamId} ]");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                    new Claim("steamId", user.SteamId.ToString()),
                    GetUserIdClaim(user)
                };

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now + _tokenValidity,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Return the UserIdClaim
        /// With this Claim present, _userMananger.GetUserAsync() functions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Claim GetUserIdClaim(ApplicationUser user)
        {
                IdentityOptions options = new IdentityOptions();
                return new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString());
        }
    }
}