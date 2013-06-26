using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
            foreach (XmlElement xmlWeek in _xml.GetElementsByTagName("Week"))
            {
                Week week = new Week();
                week.ID = xmlWeek.Attributes.GetNamedItem("id").Value.ToInt();

                foreach (XmlElement xmlTopic in xmlWeek.GetElementsByTagName("Topic"))
                {
                    int topicID = xmlTopic.Attributes.GetNamedItem("id").Value.ToInt();
                    string topicDescription = xmlTopic.InnerText;

                    KeyValuePair<int, string> topic = new KeyValuePair<int, string>(topicID, topicDescription);
                    week.Topics.Add(topic);
                }

                foreach (XmlElement xmlStudent in xmlWeek.GetElementsByTagName("Student"))
                {
                    Student student = new Student();
                    student.ID = xmlStudent.Attributes.GetNamedItem("id").Value.ToInt();
                    student.FirstName = xmlStudent.GetElementsByTagName("FirstName")[0].InnerText;
                    student.LastName = xmlStudent.GetElementsByTagName("LastName")[0].InnerText;

                    KeyValuePair<Week, Student> weekStudent = new KeyValuePair<Week, Student>(week, student);
                    Students.Add(weekStudent);
                }
            }
        }
    }
}
