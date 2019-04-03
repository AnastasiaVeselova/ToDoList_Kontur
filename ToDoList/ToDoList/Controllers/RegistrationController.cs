using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Models.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RegistrationController : Controller
    {
        private readonly IUserService _users;

        public RegistrationController(IUserService _users)
        {
            this._users = _users;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Client.Models.Users.UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            if (registrationInfo == null)
            {
                return BadRequest();
            }

            var creationInfo = new UserCreationInfo(registrationInfo.Login, Auth.AuthHash.GetHashPassword(registrationInfo.Password));

            User user = null;
            try
            {
                user = await _users.CreateAsync(creationInfo, cancellationToken);
            }
            catch (UserDuplicationException exception)
            {
                return BadRequest(exception);
            }

            return Ok(user);
        }
    }
}
