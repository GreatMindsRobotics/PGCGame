using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PGCGame.CoreTypes;

using Glib;

namespace PGCGame.Xml.XmlTypes
{
    public abstract class XmlBaseLoader
    {
        protected XmlDocument _xml = new XmlDocument();

        public static TXmlType Create<TXmlType>(XmlDataFile xmlDataFile) where TXmlType : XmlBaseLoader
        {
            TXmlType xmlType;            

            switch (xmlDataFile)
            { 
                case XmlDataFile.SecondaryWeapons:
                    xmlType = new XmlSecondaryWeapons().Cast<TXmlType>();
                    break;

                case XmlDataFile.ShipDescriptions:
                    xmlType = new XmlShipDescriptions().Cast<TXmlType>();
                    break;

                case XmlDataFile.Credits:
                    xmlType = new XmlCredits().Cast<TXmlType>();
                    break;

                default:
                    throw new NotImplementedException("The specified XmlDataFile type is not implemented.");          
            }

            xmlType.LoadXml(xmlDataFile);
            return xmlType;
        }

        protected bool LoadXml(XmlDataFile xmlDataFile)
        {
            try
            {
                _xml.Load(String.Format("Xml\\{0}.xml", xmlDataFile.ToString()));
                return true;
            }
            catch
            {
                //TODO: Handle error
                return false;
            }
        }
    }
}
