using System.Collections.Generic;
using System.Xml.Linq;
using Glib;

namespace PGCGame.Xml.XmlTypes
{
    public class XmlSecondaryWeapons : XmlBaseLoader
    {
        public struct Weapon
        {
            public int ID { get; set; }
            public int Cost { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }


        public List<Weapon> Weapons;

        public void Load()
        { 
            foreach(XElement element in _xml.Element(XName.Get("SecondaryWeapons")).Descendants(XName.Get("weapon")))
            {
                Weapon weapon = new Weapon();

                weapon.ID = element.Attribute(XName.Get("id")).Value.ToInt();
                weapon.Name = element.Element(XName.Get("name")).Value;
                weapon.Description = element.Element(XName.Get("description")).Value;
                weapon.Cost = element.Element(XName.Get("Cost")).Value.ToInt();

                Weapons.Add(weapon);
            }
        }
    }
}
