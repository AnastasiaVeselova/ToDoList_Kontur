using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Converters.Users;
using Models.Users;
using Models.Users.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Auth;
using ToDoList.Auth.Tokens;
using ToDoList.Errors;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService users;

        public AuthenticationController(IUserService users)
        {
            this.users = users;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody]Client.Models.Users.UserRegistrationInfo userInfo, [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (userInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("UserInfo");
                return BadRequest(error);
            }

            if (userInfo.Login == null || userInfo.Password == null)
            {
                var error = ServiceErrorResponses.NotEnoughUserData();
                return BadRequest(error);
            }

            User user;

            try
            {
                user = await users.GetAsync(userInfo.Login, cancellationToken);
            }
            catch(UserNotFoundException)
            {
                var error = ServiceErrorResponses.UserNotFound(userInfo.Login);
                return BadRequest(error);
            }

            if (user.PasswordHash != Auth.AuthHash.GetHashPassword(userInfo.Password))
            {
                var error = ServiceErrorResponses.IncorrectPassword();
                return BadRequest(error);
            }

            var clientUser = UserConverter.Convert(user);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, clientUser.Login),

                new Claim(ClaimTypes.NameIdentifier, clientUser.Id),
        };

            var token = new JwtSecurityToken(
                //issuer: "ToDoTasksApp",
                //audience: "ToDoTasksClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(signingEncodingKey.GetKey(), signingEncodingKey.SigningAlgorithm)
            );

            var encodedJwt =  new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthTokenAnswer
            {
                Login = userInfo.Login,

                AccessToken = encodedJwt
            });
        }
    }
}