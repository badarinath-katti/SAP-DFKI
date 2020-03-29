using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PickAndPlaceRobot_XAxis_Server
{
    public class PickAndPlace_XAxis_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("385cc2a6-ab9b-4120-aa68-29e25e422777");
        private static Guid GetCoOrdinatesMethodId = new Guid("992a6a03-3e7a-4a5c-b1a4-1bb533c58fe3");
        private static Guid HomeMethodId = new Guid("49a06616-5109-415b-8bd1-0bd86b29ae67");
        private static Guid JogMoveMethodId = new Guid("23d3ab24-8f35-48ec-8a4f-4f525f0f2b12");
        private static Guid JogMoveStopMethodId = new Guid("82c47031-cb89-4013-a667-0bc6ed2d1e86");
        private static Guid MoveAbsoluteMethodId = new Guid("a4d9f933-de04-4b65-b6e5-99fe6c7ff08a");
        private static Guid ReadActualPositionMethodId = new Guid("e904776e-bfc1-4aea-a880-284202ed2ca7");
        private static Guid StopMethodId = new Guid("e27f1fb0-216c-4249-aff7-978761b8b4be");

        public PickAndPlace_XAxis_Server()
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
            GetCoOrdinatesMetaData.Description = "Gets Current X CoOrdinate from the Robot";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("x_CoOrdinate", new Tuple<string, Type>("X CoOrdinate of Robot", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method Home
            MethodMetaData HomeMetaData = new MethodMetaData();
            HomeMetaData.Id = HomeMethodId;
            HomeMetaData.Name = "home";
            HomeMetaData.Description = "The robot moves to home in X - direction";
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
            MoveAbsoluteMetaData.Description = "The robot moves to a instructed X Co-Ordinate";
            MoveAbsoluteMetaData.RequiresDestinationSystem = false;

            MoveAbsoluteMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            MoveAbsoluteMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            MoveAbsoluteMetaData.InputParameters.Add("position", new Tuple<string, Type>("x-Position", Type.GetType("System.Int32")));
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

        public Tuple<Dictionary<string, object>, object> moveAbsolute(int position, int velocity, int acceleration, int deceleration)
        {
            String SoapResult = Executor.Execute(Executor.xMoveToHomePath, Executor.translateElectricLinmot_WSDL, Executor.readActualPositionRequest, position.ToString());

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
    }
}
