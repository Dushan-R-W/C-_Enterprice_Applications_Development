using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2
{
    public class UserDetails
    {
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private String password;
        public String Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
