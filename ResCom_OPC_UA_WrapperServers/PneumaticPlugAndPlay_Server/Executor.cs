using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PneumaticPlugAndPlay_Server
{
    public static class Executor
    {

        //WSDL Paths
        public const string translateElectricLinmot_WSDL = "http://192.168.250.34:10222/translateElectricLinmot.wsdl";
        public const string gripVacuumEject_WSDL = "http://192.168.250.35:10223/gripVacuumEject.wsdl";
        public const string identifyOptic_WSDL = "http://192.168.250.39:10221//identifyOptic.wsdl";
        public const string rotatePneumatic_WSDL = "http://192.168.250.35:10223/rotatePneumatic.wsdl";
        public const string identifyRFID_WSDL = "http://192.168.250.38:10221/identifyRFID.wsdl";
        public const string translateElectricBelt_WSDL = "http://192.168.250.36:10221/translateElectricBelt.wsdl";
        public const string verifyingGripElectric_WSDL = "http://192.168.250.32:10222/verifyingGripElectric.wsdl";
        public const string PneumaticPlugAndPlay_WSDL = "http://192.168.250.31:10223/assemblyPneumatic.wsdl";

        //SoapAction Constants
        public const string moveAbsoluteRequest =
            "http://www.smartfactory.de/rescom/translateElectricLinmot/translateElectricLinmot/moveAbsoluteRequest";

        public const string holdRequest =
            "http://www.smartfactory.de/gripVacuumEject/gripVacuumEject/holdRequest";
        public const string releaseRequest =
            "http://www.smartfactory.de/gripVacuumEject/gripVacuumEject/releaseRequest";

        public const string identifyRequest =
            "http://www.smartfactory.de/identifyOptic/identifyOptic/identifyRequest";

        public const string rotPnMoveRequest =
            "http://www.smartfactory.de/rotatePneumatic/rotatePneumatic/moveRequest";

        public const string identifyRFIDRequest =
            "http://www.smartfactory.de/identifyRFID/identifyRFID/identifyRequest";

        public const string moveCBRequest =
            "http://www.smartfactory.de/translateElectricBelt/translateElectricBelt/moveVelocityRequest";
        public const string stopCBRequest =
            "http://www.smartfactory.de/translateElectricBelt/translateElectricBelt/stopRequest";

        public const string eaGripperHoldRequest =
            "http://www.smartfactory.de/verifyingGripElectric/verifyingGripElectric/holdRequest";
        public const string eaGripperReleaseRequest =
            "http://www.smartfactory.de/verifyingGripElectric/verifyingGripElectric/releaseRequest";

        public const string pneumaticPlugAndPlayAssembleRequest =
            "http://www.smartfactory.de/assemblyPneumatic/assemblyPneumatic/assembleRequest";
        public const string pneumaticPlugAndPlayPrepareRequest =
            "http://www.smartfactory.de/assemblyPneumatic/assemblyPneumatic/prepareRequest";
        public const string pneumaticPlugAndPlayIsInUseRequest =
            "http://www.smartfactory.de/assemblyPneumatic/assemblyPneumatic/isInUseRequest";

        //Request Configuration Paths
        public const string xMoveToHomePath = @"translateElectricLinmot_X_Axis/moveAbsolute/moveToHome.txt";
        public const string yMoveToHomePath = @"translateElectricLinmot_Y_Axis/moveAbsolute/moveToHome.txt";
        public const string zMoveToHomePath = @"translateElectricLinmot_Z_Axis/moveAbsolute/moveToHome.txt";

        public const string gripPath = @"PickAndPlace/grip.txt";
        public const string releasePath = @"PickAndPlace/release.txt";

        public const string identifyPath = @"Camera/identify.txt";

        public const string pneumaticRotatePath = @"PickAndPlace/pneumaticRotate.txt";

        public const string identifyRFIDPath = @"RFID/identify.txt";

        public const string moveCBToAssemblyPath = @"ConveyerBelt/moveToAssembly.txt";
        public const string moveCBToRobotPath = @"ConveyerBelt/moveToRobot.txt";
        public const string stopCBPath = @"ConveyerBelt/stop.txt";

        public const string eaGripperHoldPath = @"ElectricAssembly/Gripper/hold.txt";
        public const string eaGripperReleasePath = @"ElectricAssembly/Gripper/release.txt";

        public const string PneumaticPlugAndPlayAssemblePath = @"HttpRequests/PneumaticPlugAndPlay/Assemble.txt";
        public const string PneumaticPlugAndPlayPreparePath = @"HttpRequests/PneumaticPlugAndPlay/Prepare.txt";
        public const string PneumaticPlugAndPlayIsInUsePath = @"HttpRequests/PneumaticPlugAndPlay/IsInUse.txt";

        enum PneumaticRotateDirection { Assembly, Oberschale }

        public static String Execute(string RequestFilePath, string WSDLPath, string SoapAction, string Offset = "")
        {
            string soapResult = String.Empty;
            try
            {
                if (File.Exists(RequestFilePath))
                {
                    string strRequest = File.ReadAllText(RequestFilePath);
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(WSDLPath);
                    webRequest.Headers.Add("SOAPAction", SoapAction);
                    webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
                    webRequest.Accept = "text/xml";
                    webRequest.Method = "POST";

                    XmlDocument soapEnvelopeXml = new XmlDocument();
                    soapEnvelopeXml.LoadXml(strRequest);

                    if (SoapAction.Equals(moveAbsoluteRequest))
                        if (!Offset.Equals(string.Empty))
                            soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:moveAbsolute"]
                            ["position"].InnerText = Offset.ToString();

                    if (SoapAction.Equals(rotPnMoveRequest))
                    {
                        if (Offset.Equals(PneumaticRotateDirection.Assembly.ToString("G")))
                            soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:move"]
                            ["direction"].InnerText = "direction2";
                        else if (Offset.Equals(PneumaticRotateDirection.Oberschale.ToString("G")))
                            soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:move"]
                            ["direction"].InnerText = "direction1";
                    }

                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }

                    using (WebResponse response = webRequest.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                            //Console.WriteLine(soapResult);
                        }
                    }
                }
            }
            catch { }
            return soapResult;
        }
    }
}
