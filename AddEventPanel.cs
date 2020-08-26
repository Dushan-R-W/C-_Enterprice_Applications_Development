using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cw2
{
    public class AddEventPanel
    {
        private Label name = new Label();
        private Label location = new Label();
        private Label dateTime = new Label();
        private Label eventType = new Label();
        private Label eventRepeat = new Label();
        private Label selectUsers = new Label();
        private Label duration = new Label();
        private TextBox nameInput = new TextBox();
        private TextBox locationInput = new TextBox();
        private DateTimePicker dateTimeInput = new DateTimePicker();
        private ComboBox eventTypeInput = new ComboBox();
        private DateTimePicker eventrepeatInput = new DateTimePicker();
        private ComboBox ContactInput = new ComboBox();
        private NumericUpDown durationInput = new NumericUpDown();
        private TableLayoutPanel panel = new TableLayoutPanel();
        public TableLayoutPanel addingElements()
        {
            //creating panel to hold elements
            panel = new TableLayoutPanel();
            panel.RowCount = 7;
            panel.ColumnCount = 2;
            panel.MaximumSize = new System.Drawing.Size(350, 195);
            panel.MinimumSize = new System.Drawing.Size(350, 195);
            //panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)14.3));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            name.Name = "name";
            name.Text = "Name";
            this.name.AutoSize = false;
            this.name.MinimumSize = new System.Drawing.Size(110, 30);
            this.name.MaximumSize = new System.Drawing.Size(110, 30);
            panel.Controls.Add(name, 0, 0);

            location.Name = "location";
            location.Text = "Location";
            this.location.MinimumSize = new System.Drawing.Size(110, 30);
            this.location.MaximumSize = new System.Drawing.Size(110, 30);
            panel.Controls.Add(location, 0, 1);

            dateTime.Name = "dateTime";
            dateTime.Text = "Date time";
            this.dateTime.MinimumSize = new System.Drawing.Size(110, 30);
            this.dateTime.MaximumSize = new System.Drawing.Size(110, 30);
            panel.Controls.Add(dateTime, 0, 2);

            eventType.Name = "eventType";
            eventType.Text = "Event type";
            this.eventType.MinimumSize = new System.Drawing.Size(120, 30);
            this.eventType.MaximumSize = new System.Drawing.Size(120, 30);
            panel.Controls.Add(eventType, 0, 3);

            eventRepeat.Name = "eventRepeat";
            eventRepeat.Text = "Event Repeat";
            this.eventRepeat.MinimumSize = new System.Drawing.Size(135, 30);
            this.eventRepeat.MaximumSize = new System.Drawing.Size(135, 30);
            panel.Controls.Add(eventRepeat, 0, 4);


            selectUsers.Name = "selectUsers";
            selectUsers.Text = "Select users";
            this.selectUsers.MinimumSize = new System.Drawing.Size(110, 30);
            this.selectUsers.MaximumSize = new System.Drawing.Size(110, 30);
            panel.Controls.Add(selectUsers, 0, 5);

            duration.Name = "duration";
            duration.Text = "Duration(mins)";
            this.duration.MinimumSize = new System.Drawing.Size(110, 30);
            this.duration.MaximumSize = new System.Drawing.Size(110, 30);
            panel.Controls.Add(duration, 0, 6);

            nameInput.Name = "nameInput";
            nameInput.Text = "nameInput";
            this.nameInput.MinimumSize = new System.Drawing.Size(150, 20);
            this.nameInput.MaximumSize = new System.Drawing.Size(150, 20);
            panel.Controls.Add(nameInput, 1, 0);

            locationInput.Name = "locationInput";
            locationInput.Text = "locationInput";
            this.locationInput.MinimumSize = new System.Drawing.Size(150, 20);
            this.locationInput.MaximumSize = new System.Drawing.Size(150, 20);
            panel.Controls.Add(locationInput, 1, 1);

            dateTimeInput.Name = "dateTimeInput";
            this.dateTimeInput.MinimumSize = new System.Drawing.Size(150, 20);
            this.dateTimeInput.MaximumSize = new System.Drawing.Size(150, 20);
            panel.Controls.Add(dateTimeInput, 1, 2);

            eventTypeInput.Name = "eventTypeInput";
            eventTypeInput.Items.Add("Appointments");
            eventTypeInput.Items.Add("Tasks");
            this.eventTypeInput.MinimumSize = new System.Drawing.Size(150, 18);
            this.eventTypeInput.MaximumSize = new System.Drawing.Size(150, 18);
            panel.Controls.Add(eventTypeInput, 1, 3);

            eventrepeatInput.Name = "eventRepeat";
            this.eventrepeatInput.MinimumSize = new System.Drawing.Size(150, 20);
            this.eventrepeatInput.MaximumSize = new System.Drawing.Size(150, 20);
            panel.Controls.Add(eventrepeatInput, 1, 4);

            ContactInput.Name = "ContactInput";
            this.ContactInput.MinimumSize = new System.Drawing.Size(150, 18);
            this.ContactInput.MaximumSize = new System.Drawing.Size(150, 18);
            ContactInput.Items.Add("None");
            panel.Controls.Add(ContactInput, 1, 5);

            durationInput.Name = "durationInput";
            this.durationInput.MinimumSize = new System.Drawing.Size(150, 20);
            this.durationInput.MaximumSize = new System.Drawing.Size(150, 20);
            panel.Controls.Add(durationInput, 1, 6);


            return panel;
        }

        

        ////getting & setting the inputdata
        //String inputName;
        //String inputLocation;
        //DateTime inputDateTime;
        //String inputEventType;
        //String inputContact;
        //public string InputName
        //{
        //    get { return inputName; }
        //    set { inputName = value; }
        //}
        //public string InputLocation
        //{
        //    get { return inputLocation; }
        //    set { inputLocation = value; }
        //}
        //public DateTime InputDateTime
        //{
        //    get { return inputDateTime; }
        //    set { inputDateTime = value; }
        //}
        //public String InputEventType
        //{
        //    get { return inputEventType; }
        //    set { inputEventType = value; }
        //}
        //public String InputContact
        //{
        //    get { return inputContact; }
        //    set { inputContact = value; }
        //}
    }
}
