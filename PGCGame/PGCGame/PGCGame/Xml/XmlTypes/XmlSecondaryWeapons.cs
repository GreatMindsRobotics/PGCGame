using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Glib;

namespace PGCGame.Xml.XmlTypes
{
    public class XmlSecondaryWeapons : XmlBaseLoader
    {
        public struct Weapon
        {
            public int ID { get; set; }
            public string Cost { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }


        public List<Weapon> Weapons;

        public void Load()
        { 
            foreach(XmlElement element in _xml.GetElementsByTagName("weapon"))
            {
                Weapon weapon = new Weapon();

                weapon.ID = element.Attributes.GetNamedItem("id").Value.ToInt();
                weapon.Name = element.GetElementsByTagName("name")[0].InnerText;
                weapon.Description = element.GetElementsByTagName("description")[0].InnerText;
                weapon.Cost = element.GetElementsByTagName("Cost")[0].InnerText; 
            }
        }
    }
}
