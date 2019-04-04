using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "ToDoListApi"; // издатель токена
        public const string AUDIENCE = "http://localhost:5001/"; // потребитель токена
        public const string KEY = "j74utrghi94kcdf69604clfdfdslokiuprne56k";   // ключ для шифрации
        public const int LIFETIME = 20; // время жизни токена - 1 минута
    }
}
