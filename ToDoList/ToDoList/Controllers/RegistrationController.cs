using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Users;
using Models.Users;
using Models.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Errors;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly IUserService users;

        public RegistrationController(IUserService users)
        {
            this.users = users;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] Client.Models.Users.UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            if (registrationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("RegistrationInfo");
                return BadRequest(error);
            }

            if (registrationInfo.Login == null || registrationInfo.Password == null)
            {
                var error = ServiceErrorResponses.NotEnoughUserData();
                return BadRequest(error);
            }

            var creationInfo = new UserCreationInfo(registrationInfo.Login, Auth.AuthHash.GetHashPassword(registrationInfo.Password));

            User user = null;
            try
            {
                user = await users.CreateAsync(creationInfo, cancellationToken);
            }
            catch (UserDuplicationException)
            {
                var error = ServiceErrorResponses.UserNameAlreadyExists(registrationInfo.Login);
                return BadRequest(error);
            }

            var clientUser = UserConverter.Convert(user);

            return Ok(clientUser);
        }
    }
}