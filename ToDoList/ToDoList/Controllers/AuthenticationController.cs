using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Users.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Auth.JWT;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserService _users;

        public AuthenticationController(UserService _users)
        {
            this._users = _users;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Post([FromBody]Client.Models.Users.UserRegistrationInfo userInfo, [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (userInfo == null)
            {
                return BadRequest();
            }

            var user = _users.GetAsync(userInfo.Login, cancellationToken);
            if (user.Result == null)
            {
                return BadRequest();
            }

            if (user.Result.PasswordHash != Auth.AuthHash.GetHashPassword(userInfo.Password))
            {
                return BadRequest();
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Login),
            };

            var token = new JwtSecurityToken(
                issuer: "ToDoTasksApp",
                audience: "ToDoTasksClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}