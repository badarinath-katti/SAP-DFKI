using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PickAndPlaceRobot_Composed_Server
{
    public class PickAndPlaceRobot_Composed_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("6bd8e97f-61ab-4f06-ac93-8db9741b4b9e");
        private static Guid GetCoOrdinatesMethodId = new Guid("6b5298b9-c38a-4252-8c88-76609edf2587");
        private static Guid HomeMethodId = new Guid("e75bd576-9f17-4013-b741-fe2e58737717");
        private static Guid JogMoveMethodId = new Guid("b0354ead-78b6-42b4-9f97-28a1b388b10e");
        private static Guid JogMoveStopMethodId = new Guid("9a95fc76-e192-40ee-89ac-96575def9fbe");
        private static Guid MoveAbsoluteMethodId = new Guid("aed84b82-e63c-49cf-bec6-f7c383f94603");
        private static Guid ReadActualPositionMethodId = new Guid("2f21518a-d476-4a2c-ae4f-bb2ef3f4f913");
        private static Guid StopMethodId = new Guid("df0a6475-73a1-4130-af00-368449db6354");

        //WSDL Paths
        public const string translateElectricLinmot_WSDL = "http://192.168.250.34:10222/translateElectricLinmot.wsdl";
        public const string gripVacuumEject_WSDL = "http://192.168.250.35:10223/gripVacuumEject.wsdl";
        public const string identifyOptic_WSDL = "http://192.168.250.39:10221//identifyOptic.wsdl";
        public const string rotatePneumatic_WSDL = "http://192.168.250.35:10223/rotatePneumatic.wsdl";
        public const string identifyRFID_WSDL = "http://192.168.250.38:10221/identifyRFID.wsdl";
        public const string translateElectricBelt_WSDL = "http://192.168.250.36:10221/translateElectricBelt.wsdl";
        public const string verifyingGripElectric_WSDL = "http://192.168.250.32:10222/verifyingGripElectric.wsdl";

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

        //Request Configuration Paths
        public const string xMoveToHomePath = @"translateElectricLinmot_X_Axis/moveAbsolute/moveToHome.txt";
        public const string yMoveToHomePath = @"translateElectricLinmot_Y_Axis/moveAbsolute/moveToHome.txt";
        public const string zMoveToHomePath = @"translateElectricLinmot_Z_Axis/moveAbsolute/moveToHome.txt";

        public const string gripPath = @"PickAndPlace/grip.txt";
        string releasePath = @"PickAndPlace/release.txt";

        public const string identifyPath = @"Camera/identify.txt";

        public const string pneumaticRotatePath = @"PickAndPlace/pneumaticRotate.txt";

        public const string identifyRFIDPath = @"RFID/identify.txt";

        public const string moveCBToAssemblyPath = @"ConveyerBelt/moveToAssembly.txt";
        public const string moveCBToRobotPath = @"ConveyerBelt/moveToRobot.txt";
        public const string stopCBPath = @"ConveyerBelt/stop.txt";

        public const string eaGripperHoldPath = @"ElectricAssembly/Gripper/hold.txt";
        public const string eaGripperReleasePath = @"ElectricAssembly/Gripper/release.txt";

        public PickAndPlaceRobot_Composed_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "Pick and Place Robot - OPC-UA Server";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets CoOrdinates from the Robot";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("x_CoOrdinate", new Tuple<string, Type>("X CoOrdinate of Robot", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("y_CoOrdinate", new Tuple<string, Type>("Y CoOrdinate of Robot", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("z_CoOrdinate", new Tuple<string, Type>("Z CoOrdinate of Robot", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method Home
            MethodMetaData HomeMetaData = new MethodMetaData();
            HomeMetaData.Id = HomeMethodId;
            HomeMetaData.Name = "home";
            HomeMetaData.Description = "The robot moves to home";
            HomeMetaData.RequiresDestinationSystem = false;
            HomeMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            HomeMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(HomeMethodId, HomeMetaData);

            //Method JogMove
            MethodMetaData JogMoveMetaData = new MethodMetaData();
            JogMoveMetaData.Id = JogMoveMethodId;
            JogMoveMetaData.Name = "jogMove";
            JogMoveMetaData.Description = "The robot jogs in a given direction";
            JogMoveMetaData.RequiresDestinationSystem = false;

            JogMoveMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            JogMoveMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            JogMoveMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            JogMoveMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(JogMoveMethodId, JogMoveMetaData);


            /////
            //Method JogMoveStop
            MethodMetaData JogMoveStopMetaData = new MethodMetaData();
            JogMoveStopMetaData.Id = JogMoveStopMethodId;
            JogMoveStopMetaData.Name = "jogMoveStop";
            JogMoveStopMetaData.Description = "The robot stops jogging";
            JogMoveStopMetaData.RequiresDestinationSystem = false;
            JogMoveStopMetaData.httpVerb = "POST";

            JogMoveStopMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            JogMoveStopMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            JogMoveStopMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            JogMoveStopMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(JogMoveStopMethodId, JogMoveStopMetaData);

            /////
            //Method MoveAbsolute
            MethodMetaData MoveAbsoluteMetaData = new MethodMetaData();
            MoveAbsoluteMetaData.Id = MoveAbsoluteMethodId;
            MoveAbsoluteMetaData.Name = "moveAbsolute";
            MoveAbsoluteMetaData.Description = "The robot moves to an instructed Co-Ordinates";
            MoveAbsoluteMetaData.RequiresDestinationSystem = false;

            MoveAbsoluteMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            MoveAbsoluteMetaData.InputParameters.Add("positionX", new Tuple<string, Type>("x-Position", Type.GetType("System.Int32")));
            MoveAbsoluteMetaData.InputParameters.Add("positionY", new Tuple<string, Type>("y-Position", Type.GetType("System.Int32")));
            MoveAbsoluteMetaData.InputParameters.Add("positionZ", new Tuple<string, Type>("z-Position", Type.GetType("System.Int32")));
            MoveAbsoluteMetaData.InputParameters.Add("isForward", new Tuple<string, Type>("Motion_Direction", Type.GetType("System.Boolean")));
            MoveAbsoluteMetaData.InputParameters.Add("velocity", new Tuple<string, Type>("velocity of robot", Type.GetType("System.Int32")));
            MoveAbsoluteMetaData.InputParameters.Add("acceleration", new Tuple<string, Type>("acceleration of robot", Type.GetType("System.Int32")));
            MoveAbsoluteMetaData.InputParameters.Add("deceleration", new Tuple<string, Type>("deceleration of robot", Type.GetType("System.Int32")));

            MoveAbsoluteMetaData.OutputParameters.Add("moveFin", new Tuple<string, Type>("Move Finish Status", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(MoveAbsoluteMethodId, MoveAbsoluteMetaData);


            /////
            //Method ReadActualPosition
            MethodMetaData ReadActualPositionMetaData = new MethodMetaData();
            ReadActualPositionMetaData.Id = ReadActualPositionMethodId;
            ReadActualPositionMetaData.Name = "readActualPosition";
            ReadActualPositionMetaData.Description = "Reads the current position of robot";
            ReadActualPositionMetaData.RequiresDestinationSystem = false;

            ReadActualPositionMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            ReadActualPositionMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            ReadActualPositionMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            ReadActualPositionMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            ReadActualPositionMetaData.OutputParameters.Add("actualPosition", new Tuple<string, Type>("Current Position of Robot", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(ReadActualPositionMethodId, ReadActualPositionMetaData);

            /////
            //Method Stop
            MethodMetaData StopMetaData = new MethodMetaData();
            StopMetaData.Id = StopMethodId;
            StopMetaData.Name = "stop";
            StopMetaData.Description = "The robot stops moving";
            StopMetaData.RequiresDestinationSystem = false;
            StopMetaData.httpVerb = "POST";

            StopMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            StopMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            StopMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            StopMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(StopMethodId, StopMetaData);

            return empMetaData;
        }
        protected override void executeCustom(Guid methodId, DestinationCallback destinationCallback, Dictionary<string, object> inputVariables,
            out Tuple<Dictionary<string, object>, object> outputVariables)
        {

            currentDestinationCallback = destinationCallback;

            base.callEMPMethod(methodId, inputVariables, out outputVariables);
        }

        protected override EmpMetaData getMethodDefinitionsCustom()
        {
            return empMetaData;
        }

        public Tuple<Dictionary<string, object>, object> GetCoOrdinates()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add(empMetaData.Methods[GetCoOrdinatesMethodId].OutputParameters.ElementAt(0).Key, 379486);

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> home()
        {
            String SoapResult = Executor.Execute(Executor.xMoveToHomePath, Executor.translateElectricLinmot_WSDL, Executor.moveAbsoluteRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> jogMove()
        {
            Executor.Execute(Executor.xMoveToHomePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> jogMoveStop()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> moveAbsolute(int positionX, int positionY, int positionZ, bool isForward,
            int velocity, int acceleration, int deceleration)
        {
            if (isForward == true)
            {
                //Execute(xMoveToHomePath, translateElectricLinmot_WSDL, moveAbsoluteRequest, "2500000");
                //Execute(yMoveToHomePath, translateElectricLinmot_WSDL, moveAbsoluteRequest, "1500000");
                //Execute(zMoveToHomePath, translateElectricLinmot_WSDL, moveAbsoluteRequest, "480000");
            }
            String SoapResult = Executor.Execute(Executor.xMoveToHomePath, Executor.translateElectricLinmot_WSDL, Executor.readActualPositionRequest, positionX.ToString());

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(empMetaData.Methods[MoveAbsoluteMethodId].OutputParameters.ElementAt(0).Key, "");
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> readActualPosition()
        {
            String SoapResult = Executor.Execute(Executor.xReadActualPositionPath, Executor.PneumaticPlugAndPlay_WSDL, Executor.readActualPositionRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(empMetaData.Methods[MoveAbsoluteMethodId].OutputParameters.ElementAt(0).Key, "");
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> stop()
        {
            String SoapResult = Executor.Execute(Executor.PneumaticPlugAndPlayIsInUsePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayIsInUseRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        //private void Execute(string RequestFilePath, string WSDLPath, string SoapAction, string Offset = "")
        //{
        //    try
        //    {
        //        if (File.Exists(RequestFilePath))
        //        {
        //            string strRequest = File.ReadAllText(RequestFilePath);
        //            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(WSDLPath);
        //            webRequest.Headers.Add("SOAPAction", SoapAction);
        //            webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
        //            webRequest.Accept = "text/xml";
        //            webRequest.Method = "POST";

        //            XmlDocument soapEnvelopeXml = new XmlDocument();
        //            soapEnvelopeXml.LoadXml(strRequest);

        //            if (SoapAction.Equals(moveAbsoluteRequest))
        //                if (!Offset.Equals(string.Empty))
        //                    soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:moveAbsolute"]
        //                    ["position"].InnerText = Offset.ToString();

        //            if (SoapAction.Equals(rotPnMoveRequest))
        //            {
        //                if (Offset.Equals(PneumaticRotateDirection.Assembly.ToString("G")))
        //                    soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:move"]
        //                    ["direction"].InnerText = "direction2";
        //                else if (Offset.Equals(PneumaticRotateDirection.Oberschale.ToString("G")))
        //                    soapEnvelopeXml.GetElementsByTagName("soap:Body")[0]["tns:move"]
        //                    ["direction"].InnerText = "direction1";
        //            }

        //            using (Stream stream = webRequest.GetRequestStream())
        //            {
        //                soapEnvelopeXml.Save(stream);
        //            }

        //            using (WebResponse response = webRequest.GetResponse())
        //            {
        //                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
        //                {
        //                    string soapResult = rd.ReadToEnd();
        //                    //Console.WriteLine(soapResult);
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}
    }

}
