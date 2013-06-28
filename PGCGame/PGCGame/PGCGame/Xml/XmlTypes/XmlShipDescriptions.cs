using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Glib;

namespace PGCGame.Xml.XmlTypes
{
    public class XmlShipDescriptions : XmlBaseLoader
    {
        public struct ShipDescription
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Specs Spec { get; set; }
            public int Damage { get; set; }
             public int Health{get; set;}
        }

        public struct Specs
        {
            
        }

        public List<ShipDescription> Descriptions;

        public void Load() 
        {
            foreach (XmlElement element in _xml.GetElementsByTagName("Ship"))
            {
                ShipDescription description = new ShipDescription();

                description.ID = element.Attributes.GetNamedItem("id").Value.ToInt();
                description.Name = element.GetElementsByTagName("Name")[0].InnerText;
                description.Description = element.GetElementsByTagName("Description")[0].InnerText;
                description.Damage = element.Attributes.GetNamedItem("Damage").Value.ToInt();
                description.Health = element.Attributes.GetNamedItem("Health").Value.ToInt();

                Descriptions.Add(description);
            }
        
        }
    }

}
