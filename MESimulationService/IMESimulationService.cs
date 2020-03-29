using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MESimulationService
{
    
    [ServiceContract(Namespace = "")]
    public interface IMESimulationService
    {
        [OperationContract]
        //[WebInvoke(BodyStyle =WebMessageBodyStyle.Bare, Method ="POST", RequestFormat =WebMessageFormat.Xml, ResponseFormat =WebMessageFormat.Xml, 
        //    UriTemplate = "/GetCityWeatherByZip?Zip={Zip}")]
        [WebInvoke]
        XElement getNextSFC(int SFCCount);

        [OperationContract]
        //[WebInvoke(BodyStyle =WebMessageBodyStyle.Bare, Method ="POST", RequestFormat =WebMessageFormat.Xml, ResponseFormat =WebMessageFormat.Xml, 
        //    UriTemplate = "/GetCityWeatherByZip?Zip={Zip}")]
        [WebInvoke]
        XElement getNextSFC_Revised(int SFCCount);

        [WebInvoke(UriTemplate = "/getNextSFC_AsString")]
        DataContracts.GetSFCsDC getNextSFC_AsString(int SFCCount);

        [WebInvoke(UriTemplate = "/SetSFCToComplete")]
        Boolean SetSFCToComplete(string SFCCount);

        [OperationContract]
        void testmethod();
    }
}
