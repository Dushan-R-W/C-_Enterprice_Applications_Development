using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace cw2
{
    public partial class MainView : Form
    {
        private string user;

        private List<TableLayoutPanel> eventPanelsList = new List<TableLayoutPanel>();
        private List<AddEventPanel> eventListPanels = new List<AddEventPanel>();
        private List<Event> eventsData = new List<Event>();
        private List<Contact> contactData = new List<Contact>();

        public MainView()
        {
            InitializeComponent();
            //Console.WriteLine("Finished calling fileReader");
        }

        //getting the contact details from user
        private void button1_Click(object sender, EventArgs e)
        {
            Boolean validation = false;
            Contact cont = new Contact();
            cont.ContactName = ContactNameInput.Text;
            cont.Email = EmailInput.Text;
            if (int.TryParse(phoneNumberInput.Text, out int result))
            {
                cont.PhoneNumber = int.Parse(phoneNumberInput.Text);
            }
            else AddContactError.Text = "Phone number should only have numbers";


            //if there are any contacts saved, check if contact name already exists.
            if (contactData.Count > 0)
            {
                for (int i = 0; i < contactData.Count; i++)
                {
                    if (ContactNameInput.Text == contactData[i].ContactName.ToString())
                    {
                        AddContactError.Text = "Contact name already exists";
                    }else validation = true;
                }
            }
            else validation = true;//no contacts saved means don't check for dublicates

            if (validation == true) //only add contacts if validation is true
            {
                contactData.Add(cont); //adding Contacts Objects to the List
                displayContactList(); //displaying contats in listbox
                AddContactError.Text = "";
                ThreadedFile("write"); 
            }        
        }

        //display contact list in the add contact page
        private void displayContactList()
        {
            ContactListView.Items.Clear(); //to avoid showing 1 contact twice

            for (int i = 0; i < contactData.Count; i++)
            {
                Contact tempCont = new Contact();
                tempCont = contactData[i];
                ListViewItem listViewItem = new ListViewItem(new string[] {tempCont.ContactName, tempCont.PhoneNumber.ToString(), tempCont.Email});

                ContactListView.Items.Add(listViewItem);
            }
        }

        private void deleteConButton_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem rowData = ContactListView.SelectedItems[0]; //gets the whole row
                String name = rowData.SubItems[0].Text; //extract name from whole row
                String email = rowData.SubItems[2].Text;//extract email from whole row
                //Console.WriteLine($"name-{name}  email-{email}");

                for (int i=0; i<contactData.Count; i++)
                {
                    if(name == contactData[i].ContactName && email == contactData[i].Email)
                    {
                        contactData.RemoveAt(i);
                    }
                }
                displayContactList(); //list the contacts again (refresh)
            }
            catch (System.NullReferenceException) 
            { }
            catch (System.ArgumentOutOfRangeException)
            { }
        }

        //validating the phone number input in contactView form
        private void phoneNumberInput_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(phoneNumberInput.Text, "^[0-9]*$"))
            {
                errorLabel.Visible = false;
            }
            else
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Phone number can ONLY contain numbers";
                errorLabel.ForeColor = Color.Red;
            }
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            this.user = Program.getUser();
            welcomeUser.Text = "Hello " + Program.getUser();
            ThreadedFile("read");
        }

        private void numberOfPanels_TextChanged(object sender, EventArgs e)
        {
            errorEvent.Visible = false;
            if (int.TryParse(numberOfPanels.Text, out int result))
            {
                dynamicEvents(Int32.Parse(numberOfPanels.Text));
            }
            else errorEvent.Visible = true;
        }

        //adding panels dynamically
        private void dynamicEvents(int numberOfPanels)
        {
            tableLayoutPanel1.Controls.Clear();
            eventListPanels.Clear();
            eventPanelsList.Clear();

            //creating sets of objects of class AddEventPanel
            for (int i = 0; i < numberOfPanels; i++)
            {
                AddEventPanel av = new AddEventPanel();
                TableLayoutPanel eachPanel = av.addingElements();//tableLayoutPanel1, Int32.Parse(numberOfPanels.ToString()));
                eachPanel.Name = "eachPanel" + i.ToString();
                eventListPanels.Add(av); //adding object of class AddEventPanel to a List
                eventPanelsList.Add(eachPanel); //adding object of class AddEventPanel to a List
            }

            //loop for displaying input fields
            int row = 0;
            for (int i = 0; i < numberOfPanels; i++)
            {
                //adding a title to each sets of events dynamically
                Label title = new Label();
                title.Name = "Event " + (i + 1);
                title.Text = "Event " + (i + 1);
                title.Font = new Font("Arial", 15);

                //adding panel title & the panel itself
                tableLayoutPanel1.Controls.Add(title, 0, row);
                row++;
                tableLayoutPanel1.Controls.Add(eventPanelsList[i], 0, (row));
                row++;
                tableLayoutPanel1.RowStyles.Clear();
            }

            //Search for ComboBox in the panel to display contacts
            Contact tempCont = new Contact();
            for (int i = 0; i < eventPanelsList.Count; i++)
            {
                TableLayoutPanel panels = eventPanelsList[i];
                //search for a combobox in each sets of panels
                ComboBox contactInput = panels.Controls.OfType<ComboBox>().FirstOrDefault(b => b.Name.Equals("ContactInput")); 

                //List the contacts in each ComboBox
                for (int n = 0; n < contactData.Count; n++)
                {
                    tempCont = contactData[n];
                    contactInput.Items.Add(tempCont.ContactName);
                    //contactInput.Items.Add(tempCont.ToString());
                    //Console.WriteLine(tempCont.ContactName);
                }
            }
        }

        private void AddEventsButton_Click(object sender, EventArgs e)
        {
            getInputEvents();
        }

        //getting the event inputs
        private void getInputEvents()
        {
            List<Event> tempEvents = new List<Event>();
            List<Event> extrarepeatEvents = new List<Event>();
            Boolean error = false;

            for (int i = 0; i < eventPanelsList.Count; i++)
            {
                //String panelName = eventPanels[i].Name;
                TableLayoutPanel panels = eventPanelsList[i];
                Event ev = new Event();
                //Boolean nulls = true;
                //searching for each input boxes
                TextBox nameInput = panels.Controls.OfType<TextBox>().FirstOrDefault(b => b.Name.Equals("nameInput"));
                TextBox locationInput = panels.Controls.OfType<TextBox>().FirstOrDefault(b => b.Name.Equals("locationInput"));
                DateTimePicker dateTimeInput = panels.Controls.OfType<DateTimePicker>().FirstOrDefault(b => b.Name.Equals("dateTimeInput"));
                ComboBox eventTypeInput = panels.Controls.OfType<ComboBox>().FirstOrDefault(b => b.Name.Equals("eventTypeInput"));
                DateTimePicker eventRepeat = panels.Controls.OfType<DateTimePicker>().FirstOrDefault(b => b.Name.Equals("eventRepeat"));
                ComboBox contactInput = panels.Controls.OfType<ComboBox>().FirstOrDefault(b => b.Name.Equals("ContactInput"));
                NumericUpDown duration = panels.Controls.OfType<NumericUpDown>().FirstOrDefault(b => b.Name.Equals("durationInput"));

                //adding input details to ev object
                //dealing with nulls in inputs
                ev.DateTime = dateTimeInput.Value; //no need to check date for nulls
                ev.Duration = (int)duration.Value;
                ev.EventRepeat = eventRepeat.Value;

                if (string.IsNullOrEmpty(nameInput.Text))
                {
                    errorDisplay.Visible = true;
                    //nulls = true;
                    error = true;
                }else ev.EventName = nameInput.Text;

                if(string.IsNullOrEmpty(locationInput.Text))
                {
                    ev.Location = "none";
                }else ev.Location = locationInput.Text;

                if (eventTypeInput.SelectedIndex == -1)
                {
                    ev.EventType = "none";
                }else ev.EventType = eventTypeInput.SelectedItem.ToString();

                if (contactInput.SelectedIndex == -1)
                {
                    ev.ContactName = "none";
                }else ev.ContactName = contactInput.SelectedItem.ToString();

                tempEvents.Add(ev); //save ev data to a tempList

                if(error == true) 
                {
                    errorDisplay.Visible = true;
                }
            }

            //add events from temp only if no errors
            if(error == false)
            {
                errorDisplay.Visible = false;
                for (int i = 0; i < eventPanelsList.Count; i++)
                {
                    eventsData.Add(tempEvents[i]);
                }
                ThreadedFile("write"); //write to file after
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimePicker1.Checked == true)
            {
                dateTimePicker2.Checked = false;
                dateTimePicker3.Checked = false;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimePicker2.Checked == true)
            {
                dateTimePicker1.Checked = false;
                dateTimePicker3.Checked = true;
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimePicker3.Checked == true)
            {
                dateTimePicker1.Checked = false;
                dateTimePicker2.Checked = true;
            }
        }

        //view weekly events or select a custom date range
        private void selectedWeekButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DateTime weekSelected = dateTimePicker1.Value.Date;
            DateTime mondayOfselectedWeek = weekSelected;
            Boolean monday = false;
            while (monday == false)
            {
                if (weekSelected.Date.DayOfWeek == DayOfWeek.Monday)
                {
                    Console.WriteLine($"Monday found - {weekSelected.Date}");
                    mondayOfselectedWeek = weekSelected.Date;
                    monday = true;
                }
                else
                {
                    Console.WriteLine($"Monday not found - {weekSelected.Date}");
                    weekSelected = weekSelected.AddDays(-1);
                }
            }

            //var weekEnd = weekStart.AddDays(7).AddSeconds(-1);
            //Console.WriteLine(mondayOfselectedWeek);
            for (int n = 0; n < 7; n++)
            {
                var currentDate = mondayOfselectedWeek.AddDays(n); //looping through all 7 days accourding to the selected date
                for (int i = 0; i < eventsData.Count; i++)
                {
                    if (eventsData[i].DateTime.Date == currentDate.Date) //only comparing date without time
                    {
                        //add to grid
                        dataGridView1.Rows.Add(eventsData[i].EventName, eventsData[i].DateTime, eventsData[i].Location, eventsData[i].EventType, eventsData[i].EventRepeat, eventsData[i].ContactName, eventsData[i].Duration);
                    }
                }
            }
        }

        private void customDateButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DateTime date1 = dateTimePicker2.Value.Date;
            DateTime date2 = dateTimePicker3.Value.Date.AddDays(1); //adding 1 date as loop dont cover last date

            //looping through all the dates between two selected dates
            for (var current=date1; current<date2; current=current.AddDays(1))
            {
                //Console.WriteLine($"date1-{date1} date2-{date2} current-{current}");
                for (int i = 0; i < eventsData.Count; i++)//for each date above, looping through whole eventData List
                {
                    if (eventsData[i].DateTime.Date == current)//check and add to gridview
                    {
                        dataGridView1.Rows.Add(eventsData[i].EventName, eventsData[i].DateTime, eventsData[i].Location, eventsData[i].EventType, eventsData[i].EventRepeat, eventsData[i].ContactName, eventsData[i].Duration);
                    }
                }
            }
        }

        private void clearEventsButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        //deleting events from events view
        private void deleteEventsButton_Click(object sender, EventArgs e)
        {
            try
            {
                String EventName = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                DateTime datetime = (DateTime)dataGridView1.CurrentRow.Cells[1].Value;

                for (int i = 0; i < eventsData.Count; i++)
                {
                    if (eventsData[i].EventName == EventName && eventsData[i].DateTime == datetime)
                    {
                        eventsData.RemoveAt(i);
                    }
                }
                ThreadedFile("write"); //write to file 

                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            catch (System.NullReferenceException) { }
        }

        private void viewAllEvents_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            
            for(int i=0; i<eventsData.Count; i++)
            {
                dataGridView1.Rows.Add(eventsData[i].EventName, eventsData[i].DateTime, eventsData[i].Location, eventsData[i].EventType, eventsData[i].EventRepeat, eventsData[i].ContactName, eventsData[i].Duration);
            }
        }

        private void ThreadedFile(String option) //running the read/write in a new thread
        {
            if(option == "read") 
            {
                //Console.WriteLine("entering file reader thread");
                Thread readFiles = new Thread(new ThreadStart(fileread));
                readFiles.Start();
                //Console.WriteLine("file reader thread started");
            }

            if (option == "write")
            {
                //Console.WriteLine("entering file writer thread");
                Thread readFiles = new Thread(new ThreadStart(filewirter));
                readFiles.Start();
                //Console.WriteLine("file write thread started");
            } 
        }

        private void fileread()
        {
            ThreadedFileWriteRead writeRead = new ThreadedFileWriteRead();
            Console.WriteLine(user);
            Tuple<List<Event>, List<Contact>> returnedList  = writeRead.fileReader(user);
            eventsData = returnedList.Item1;
            contactData = returnedList.Item2;
        }

        private void filewirter()
        {
            Boolean tempbool = false;
            Object lockingObject = ""; // Lock-Objecs
            //writing events
            lock (lockingObject)
            {
                //code
                //Console.WriteLine("writing events");
                ThreadedFileWriteRead writeRead = new ThreadedFileWriteRead();
                writeRead.writeEvents(eventsData, user);

                tempbool = true;
                Monitor.Pulse(lockingObject);
            }

            //writing contacts
            lock (lockingObject)
            {
                while (tempbool == false) //signaling condition
                {
                    Monitor.Wait(lockingObject); //blocks until a notification is received form above method
                }
                //code
                //Console.WriteLine("writing contatcs");
                ThreadedFileWriteRead writeRead = new ThreadedFileWriteRead();
                writeRead.writeContacts(contactData, user);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == tabPage4)
            {
                displayContactList();
            }
        }

        private void MainView_FormClosed(object sender, FormClosedEventArgs e)
        {
            ThreadedFile("write"); //write to file
        }

        private void predictButton_Click(object sender, EventArgs e)
        {
            chart2.Series["Total"].Points.Clear();

            PredictUsage pu = new PredictUsage();
            double[] totalMean = pu.TotalCalculations(eventsData);

            for (int i = 0; i < totalMean.Length; i++)
            {
                chart2.Series["Total"].Points.AddXY(i, totalMean[i]);
            }
        }

        private void extraChart_Click(object sender, EventArgs e)
        {
            PredictUsageView2 newWindow = new PredictUsageView2();
            newWindow.Owner = this;
            newWindow.ShowInTaskbar = false;
            newWindow.Show();

            PredictUsage meanCal = new PredictUsage();
            Tuple<double[], double[]> extraCals = meanCal.ExtraCalculations(eventsData);
            double[] appointMean = extraCals.Item1;
            double[] tasksMean = extraCals.Item2;

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(appointMean[i]);
                newWindow.chart1.Series["Appointments"].Points.AddXY(i, appointMean[i]);
                newWindow.chart1.Series["Tasks"].Points.AddXY(i, tasksMean[i]);
            }
        }
    }
}