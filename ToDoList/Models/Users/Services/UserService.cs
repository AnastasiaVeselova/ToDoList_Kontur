using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ToDoTasksDB"));
            var database = client.GetDatabase("ToDoTasksDB");
            _users = database.GetCollection<User>("Users");
        }

        public Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var userDuplicate = _users.Find<User>(u => u.Login == creationInfo.Login).FirstOrDefault();

            if (userDuplicate != null)
            {
                throw new UserDuplicationException(creationInfo.Login);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = DateTime.UtcNow
            };

            _users.InsertOne(user);
            return Task.FromResult(user);
        }

        public Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _users.Find<User>(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return Task.FromResult(user);
        }

        public Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var user = _users.Find<User>(u => u.Login == login).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotFoundException(login);
            }

            return Task.FromResult(user);
        }
    }
}