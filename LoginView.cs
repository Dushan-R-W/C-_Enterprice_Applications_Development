using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace cw2
{
    public partial class LoginView : Form
    {
        public LoginView()
        {
            InitializeComponent();
            //Console.WriteLine(userdetailsList.Count);
        }
        public string currentuser;
        private List<UserDetails> userdetailsList = new List<UserDetails>();
       
        private void LoginButton_Click(object sender, EventArgs e)
        {
            error1.Text = "";
            if (userdetailsList.Count != 0)
            {
                for (int i = 0; i < userdetailsList.Count; i++)
                {
                    if (usernameLog.Text == userdetailsList[i].UserName)
                    {
                        if (PasswordLog.Text == userdetailsList[i].Password)
                        {
                            
                            currentuser = userdetailsList[i].UserName;
                            this.DialogResult = DialogResult.OK;
                            this.Dispose();
                        }
                        else error1.Text = "Password do NOT match";
                    }
                    else error1.Text = "username NOT found";
                }
            }
            else error1.Text = "No users found";
        }

        private void Registerbutton_Click(object sender, EventArgs e)
        {
            error2.Text = "";

            if (userdetailsList.Count == 0)
            {
                if (!string.IsNullOrEmpty(usernameReg.Text) && !string.IsNullOrEmpty(passwordReg.Text))
                {
                    UserDetails ud = new UserDetails();
                    ud.UserName = usernameReg.Text;
                    ud.Password = passwordReg.Text;
                    userdetailsList.Add(ud);
                    writeFile();
                }
                else error2.Text = "Fields are empty";
            }
            else
            {
                for (int i = 0; i < userdetailsList.Count; i++)
                {
                    if (usernameReg.Text != userdetailsList[i].UserName && !string.IsNullOrEmpty(usernameReg.Text) && !string.IsNullOrEmpty(passwordReg.Text))
                    {
                        UserDetails ud = new UserDetails();
                        ud.UserName = usernameReg.Text;
                        ud.Password = passwordReg.Text;
                        userdetailsList.Add(ud);
                        writeFile();
                    }
                    else error2.Text = "username taken or empty fields";
                }
            }    
        }

        public void writeFile()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            String fullFile = Path.Combine(path, "userDetails.xml");
            XmlSerializer serialiser = new XmlSerializer(userdetailsList.GetType());
            TextWriter FileStream = new StreamWriter(fullFile);
            serialiser.Serialize(FileStream, userdetailsList);
            FileStream.Close();
            Console.WriteLine("writeFile");
        }


        private void readFile()
        {
            userdetailsList.Clear();
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            String fullFile = Path.Combine(path, "userDetails.xml");
            //XDocument xmldoment = XDocument.Load(fullFile);

            XmlSerializer serialiser = new XmlSerializer(userdetailsList.GetType());
            try
            {
                using (var stream = File.OpenRead(fullFile))
                {
                    List<UserDetails> details = (List<UserDetails>)(serialiser.Deserialize(stream));
                    userdetailsList.AddRange(details);
                    Console.WriteLine(userdetailsList.Count);
                }
            }
            catch
            {
                Console.WriteLine("error");
            }
        }

        private void LoginView_Load(object sender, EventArgs e)
        {
            readFile();
        }

        private void LoginView_FormClosed(object sender, FormClosedEventArgs e)
        {
            writeFile();
        }

        
    }
}
