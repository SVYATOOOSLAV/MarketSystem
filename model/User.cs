using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class User
    {
        public String login { get; set; }
        public String password { get; set; }
        public String role { get; set; }

        public User(String login, String password, String role)
        {
            this.login = login;
            this.password = password;
            this.role = role;
        }

    }
}
