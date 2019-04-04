using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Auth
{
    public class AuthTokenAnswer
    {
        public string Login { get; set; }

        public string AccessToken { get; set; }
    }
}
