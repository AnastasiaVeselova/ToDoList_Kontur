using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Auth.Tokens
{
    //https://andrey.moveax.ru/post/asp-net-core-web-api-authentication-part-2-jwt-based-on-jws

    public class SigningSymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey
    {
        private readonly SymmetricSecurityKey _secretKey;

        public string SigningAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        public SigningSymmetricKey(string key)
        {
            this._secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public SecurityKey GetKey() => this._secretKey;
    }
}