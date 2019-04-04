﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Auth.Tokens
{
    // Ключ для проверки подписи (публичный)
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
