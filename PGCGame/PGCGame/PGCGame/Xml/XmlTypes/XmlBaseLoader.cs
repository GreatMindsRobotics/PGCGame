using System;
using System.Xml.Linq;
using Glib;
using PGCGame.CoreTypes;

namespace PGCGame.Xml.XmlTypes
{
    public abstract class XmlBaseLoader
    {
        protected XDocument _xml = new XDocument();

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

            if (!xmlType.LoadXml(xmlDataFile))
            {
                throw new Exception("The data file failed to load.");
            }
            return xmlType;
        }

        protected bool LoadXml(XmlDataFile xmlDataFile)
        {
            try
            {
                _xml = XDocument.Load(String.Format("Xml\\{0}.xml", xmlDataFile.ToString()));
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
