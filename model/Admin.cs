using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    internal class Admin
    {
        public String login { get; set; }
        public Admin(String login)
        {
            this.login = login;
        }
    }
}
