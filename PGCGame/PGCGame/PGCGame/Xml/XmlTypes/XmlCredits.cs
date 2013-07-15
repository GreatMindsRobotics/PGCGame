using System.Collections.Generic;
using System.Xml.Linq;
using Glib;

namespace PGCGame.Xml.XmlTypes
{
    public class XmlCredits : XmlBaseLoader
    {
        public struct Student
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Week
        {
            /// <summary>
            /// Week ID
            /// </summary>
            public int ID { get; set; }
            
            /// <summary>
            /// List of topics; int ID, string Description
            /// </summary>
            public List<KeyValuePair<int, string>> Topics = new List<KeyValuePair<int, string>>();
        }

        public struct Helper
        {
            public int ID;

            public string LastName;
            public string FirstName;

            public string FullName
            {
                get
                {
                    return string.Format("{0} {1}", FirstName, LastName);
                }
            }

            public string Job;

            public Helper(XElement helperElement)
            {
                ID = helperElement.Attribute(XName.Get("id")).Value.ToInt();
                Job = helperElement.Element(XName.Get("Job")).Value;
                FirstName = helperElement.Element(XName.Get("FirstName")).Value;
                LastName = helperElement.Element(XName.Get("FullLastName")).Value;
            }
        }

        /// <summary>
        /// List of Key-Value pairs; int WeekID, struct Student
        /// </summary>
        public List<KeyValuePair<Week, Student>> Students = new List<KeyValuePair<Week, Student>>();

        public List<Helper> AllHelpers = new List<Helper>();

        public void LoadData()
        {
            XElement rootElement = _xml.Element(XName.Get("Credits"));

            foreach (XElement xmlHelper in rootElement.Element(XName.Get("UnderlyingHelpers")).Descendants(XName.Get("Helper")))
            {
                AllHelpers.Add(new Helper(xmlHelper));
            }

            foreach (XElement xmlWeek in rootElement.Descendants(XName.Get("Week")))
            {
                Week week = new Week();
                week.ID = xmlWeek.Attribute(XName.Get("id")).Value.ToInt();

                foreach (XElement xmlTopic in xmlWeek.Descendants(XName.Get("Topic")))
                {
                    int topicID = xmlTopic.Attribute(XName.Get("id")).Value.ToInt();
                    string topicDescription = xmlTopic.Value;

                    KeyValuePair<int, string> topic = new KeyValuePair<int, string>(topicID, topicDescription);
                    week.Topics.Add(topic);
                }

                foreach (XElement xmlStudent in xmlWeek.Descendants(XName.Get("Student")))
                {
                    Student student = new Student();
                    student.ID = xmlStudent.Attribute(XName.Get("id")).Value.ToInt();
                    student.FirstName = xmlStudent.Element(XName.Get("FirstName")).Value;
                    student.LastName = xmlStudent.Element(XName.Get("LastName")).Value;

                    KeyValuePair<Week, Student> weekStudent = new KeyValuePair<Week, Student>(week, student);
                    Students.Add(weekStudent);
                }
            }
        }
    }
}
