﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public ActionResult GenerateToken([FromBody]Client.Models.Users.UserRegistrationInfo userInfo, [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (userInfo == null)
            {
                //изменить статусы ответа при неудачах в юзерах и туду
                return BadRequest();
            }

            var user = users.GetAsync(userInfo.Login, cancellationToken);
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
                //new Claim(ClaimTypes, userInfo.Login),
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