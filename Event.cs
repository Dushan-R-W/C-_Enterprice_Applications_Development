using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2
{
    public class Event
    {
        private string eventName;
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        private String eventType;
        public String EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        private DateTime eventRepeat;
        public DateTime EventRepeat
        {
            get { return eventRepeat; }
            set { eventRepeat = value; }
        }

        private String location;
        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        private String contactName;
        public String ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}
