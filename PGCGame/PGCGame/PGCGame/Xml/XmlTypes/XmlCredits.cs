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

        /// <summary>
        /// List of Key-Value pairs; int WeekID, struct Student
        /// </summary>
        public List<KeyValuePair<Week, Student>> Students = new List<KeyValuePair<Week, Student>>();

        public void LoadData()
        {
            foreach (XElement xmlWeek in _xml.Element(XName.Get("Credits")).Descendants(XName.Get("Week")))
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
