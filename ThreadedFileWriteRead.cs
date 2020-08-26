using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cw2
{
    public class ThreadedFileWriteRead
    {
        string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        internal List<Event> eventsData = new List<Event>();
        internal  List<Contact> contactData = new List<Contact>();

        public Tuple<List<Event>, List<Contact>> fileReader(string user) 
        {
            Console.WriteLine(user);
            Boolean tempbool = false;
            Object lockingObject = ""; // Lock-Object
            //Console.WriteLine("entered fileReader");

            //reading contacts
            lock (lockingObject)
            {
                //Console.WriteLine("reading contacts started");
                List<Contact> n = readContacts(user);
                contactData = n;
                tempbool = true;
                Monitor.Pulse(lockingObject);
                //Console.WriteLine("reading contacts ended");
            }

            //reading events
            lock (lockingObject)
            {
                while (tempbool == false) //signaling condition
                {
                    Monitor.Wait(lockingObject); //blocks until a notification is received form methodtwo
                }
                //Console.WriteLine("reading events started");
                List<Event> n = readEvents(user);
                eventsData = n;
                //Console.WriteLine("reading events ended");
            }
            return Tuple.Create(eventsData, contactData);
        }

        //Write Contacts
        public void writeContacts(List<Contact> contactList, String user)
        {
            XmlSerializer serialiser = new XmlSerializer(contactList.GetType());
            TextWriter FileStream = new StreamWriter(Path.Combine(path, user + "_contacts.xml"));
            //Console.WriteLine(path);
            serialiser.Serialize(FileStream, contactList);
            FileStream.Close();
        }

        //Write Events
        public void writeEvents(List<Event> eventList, String user)
        {
            XmlSerializer serialiser = new XmlSerializer(eventList.GetType());
            TextWriter FileStream = new StreamWriter(Path.Combine(path, user + "_events.xml"));
            serialiser.Serialize(FileStream, eventList);
            FileStream.Close();
        }

        //Read Contacts
        public List<Contact> readContacts(String user)
        {
            List<Contact> contactList = new List<Contact>();
            XmlSerializer serialiser = new XmlSerializer(contactList.GetType());
         
            try
            {
                using (var stream = File.OpenRead(Path.Combine(path, user + "_contacts.xml")))
                {
                    List<Contact> contacts = (List<Contact>)(serialiser.Deserialize(stream));
                    contactList.Clear();
                    contactList.AddRange(contacts);
                    return contactList;
                }
            }
            catch
            {
                return contactList;
            }
        }

        //Read Events
        public List<Event> readEvents(String user)
        {
            List<Event> eventList = new List<Event>();
            XmlSerializer serialiser = new XmlSerializer(eventList.GetType());
    
            try
            {
                using (var stream = File.OpenRead(Path.Combine(path, user + "_events.xml")))
                {
                    List<Event> events = (List<Event>)(serialiser.Deserialize(stream));
                    eventList.Clear();
                    eventList.AddRange(events);
                    return eventList;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                return eventList;
            }
        }
    }
}