using System.Collections.Generic;
using System.Xml.Linq;
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
            foreach (XElement element in _xml.Element(XName.Get("Ships")).Descendants(XName.Get("Ship")))
            {
                ShipDescription description = new ShipDescription();

                description.ID = element.Attribute(XName.Get("id")).Value.ToInt();
                description.Name = element.Element(XName.Get("Name")).Value;
                description.Description = element.Element(XName.Get("Description")).Value;
                description.Damage = element.Element(XName.Get("Specs")).Element(XName.Get("Damage")).Value.ToInt();
                description.Health = element.Element(XName.Get("Specs")).Element(XName.Get("Health")).Value.ToInt();

                Descriptions.Add(description);
            }
        
        }
    }

}
