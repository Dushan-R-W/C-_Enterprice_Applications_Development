using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2
{
    public class Contact
    {
        private string contactName;
        public string ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }

        private int phoneNumber;
        public int PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private String email;
        public String Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}

