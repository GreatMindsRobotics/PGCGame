using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
            public int ID { get; set; }
            public List<KeyValuePair<int, string>> Topics = new List<KeyValuePair<int, string>>();
        }

        /// <summary>
        /// List of Key-Value pairs; int WeekID, struct Student
        /// </summary>
        public List<KeyValuePair<Week, Student>> Students = new List<KeyValuePair<Week, Student>>();

        public void LoadData()
        {
            foreach (XmlNode week in _xml.GetElementsByTagName("Week"))
            {

            }
        }
    }
}
