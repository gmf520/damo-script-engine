using System;

namespace Liuliu.ScriptEngine.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string NickName { get; set; }

        public DateTime RegistedDate { get; set; }

        public DateTime LastLoginedDate { get; set; }
    }
}
