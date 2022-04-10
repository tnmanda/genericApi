using Api.Service.CustomTags;
using Api.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Service.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfigurationRoot _config;

        const string FailedToGenerateTokenError = "Failed to generate a token";
        const string InvalidLoginError = "Invalid Login";

        public AuthController(IConfigurationRoot config)
        {
            _config = config;
        }

        [HttpPost("token")]
        [ValidateModel]
        public IActionResult Token([FromBody] Login login)
        {
            try
            {
                var apitoken = CreateApiToken(login.usr, login.pwd);
                if (apitoken == null)
                {
                    return BadRequest(FailedToGenerateTokenError);
                }
                return Ok(new
                {
                    apitoken
                });
            }
            catch (Exception e)
            {
                // TODO logthis
            }

            return BadRequest(InvalidLoginError);
        }

        private bool ValidUser(string usr, string pwd)
        {
            string validUser = _config["Identity:usr"];
            string validPwd = _config["Identity:pwd"];

            if(usr == validUser && pwd == validPwd)
            {
                return true;
            }

            return false;
        }

        private TokenData CreateApiToken(string usr, string pwd)
        {
            if (!ValidUser(usr, pwd))
            {
                return null;
            }

            string TokenKey = _config["TokenValues:key"];
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(TokenKey));
            var signInCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string userName = usr;
            string password = pwd;
            string GivenName = _config["Identity:GivenName"];
            string Email = _config["Identity:email"];
            
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usr), 
                    new Claim(JwtRegisteredClaimNames.GivenName, GivenName),
                    new Claim("eml", Email) 
                };

                var token = new JwtSecurityToken(
                    issuer: _config["TokenValues:Issuer"],
                    audience: _config["TokenValues:Audience"],
                            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["TokenValues:expiresInMinutes"])),
                            claims: claims,
                            signingCredentials: signInCreds
                            );

                var ApiToken = new JwtSecurityTokenHandler().WriteToken(token);
                var ApiTokenExpiration = token.ValidTo;

                var apitokendata = new TokenData { token = ApiToken, expiration = ApiTokenExpiration };
                return apitokendata;
            }

            catch (Exception e)
            {
                // TODO log this
            }

            return null;
        }
    }
}