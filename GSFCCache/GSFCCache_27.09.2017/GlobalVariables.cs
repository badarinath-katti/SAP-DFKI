using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GSFCCache
{
    public static class GlobalVariables
    {
        public static Guid empTestNodeId = new Guid("77786eba-f095-4996-9dcb-4013f3d7df04");
        public static Guid getWeatherDataByZipId = new Guid("94b2286d-5fd1-4126-b649-e7d32f67b23b");

        public static Guid GetSFCsMethodID = new Guid("2e01d382-6007-463d-9022-14c5f0ddec05");
        public static Guid RegisterResourceMethodID = new Guid("5ace903c-3a11-4a77-8521-7cefd14677aa");
        public static Guid StateAvailableMethodID = new Guid("33e696df-92fd-4c11-b969-fcbd1d4f9283");
        public static Guid StateOnHoldMethodID = new Guid("30f4bac9-cb68-4a25-948d-13b7cb43aa28");
        public static Guid CallbackDelegateWorkMethodID = new Guid("17ae8cd1-fe7f-4510-a12e-f1f101002a72");
        
        public static Guid SetSFCToCompleteMethodID = new Guid("7ff4e6e7-eda2-446f-9d1a-10c044688319");

        public const int GSFCCacheNewSFCsSize = 5;
        public const int GSFCCacheNewSFCsSizeMinThreshold = 1;
        public const int GSFCCacheProcessingSFCSize = 5;
        public const int GSFCCacheProcessingSFCSizeMinThreshold = 1;

        //Machine names - hardcoded now
        public const string GetResourceNameMethod = "GetResourceName";
        public const string GetCapabilityMethod = "GetCapability";
        public const string CallGSFCMethod = "CallGSFC";
        public const string GetQueueLengthMethod = "GetQueueLength";
        public const string GetStateMethod = "GetState";
        public const string DoWorkMethod = "DoWork";

        public const string GSFCUrl = "http://localhost:8091/PCoWebServer?wsdl";
    }

    public static class Extensions
    {
        public static XElement ToXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        public static XmlElement ToXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc.DocumentElement;
            }
        }
    }
}
