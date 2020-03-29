using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PickAndPlaceRobot_ZAxis_Server
{
    public class PickAndPlace_ZAxis_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("d1b9683a-8baa-4c01-945b-053f600d4375");
        private static Guid GetCoOrdinatesMethodId = new Guid("93ca9034-0073-4563-80b2-b9633a55d58e");
        private static Guid HomeMethodId = new Guid("1b5710f3-5621-40c0-94c2-a4ecffc7cd89");
        private static Guid JogMoveMethodId = new Guid("d2a68eef-77e0-4f1f-8bcd-b1d59cae0756");
        private static Guid JogMoveStopMethodId = new Guid("a9a5e2b5-cd2a-4732-bf57-33302e8f3c7d");
        private static Guid MoveAbsoluteMethodId = new Guid("81a0f415-e970-497c-b228-9d017254eed1");
        private static Guid ReadActualPositionMethodId = new Guid("ee3f3317-e383-4d01-ac7f-386b45c994d4");
        private static Guid StopMethodId = new Guid("2adf57cd-6d62-49f4-bfde-8f795bc50928");

        public PickAndPlace_ZAxis_Server()
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
            GetCoOrdinatesMetaData.Description = "Gets current Z CoOrdinate from the Robot";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("z_CoOrdinate", new Tuple<string, Type>("Z CoOrdinate of Robot", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method Home
            MethodMetaData HomeMetaData = new MethodMetaData();
            HomeMetaData.Id = HomeMethodId;
            HomeMetaData.Name = "home";
            HomeMetaData.Description = "The robot moves to home in Z - direction";
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

            MoveAbsoluteMetaData.InputParameters.Add("position", new Tuple<string, Type>("z-Position", Type.GetType("System.Int32")));
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
            StopMetaData.Description = "The robot stops jogging";
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
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> jogMove()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

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
            String SoapResult = Executor.Execute(Executor.zMoveToHomePath, Executor.translateElectricLinmot_WSDL, Executor.moveAbsoluteRequest, position.ToString());

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(empMetaData.Methods[MoveAbsoluteMethodId].OutputParameters.ElementAt(0).Key, "");
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> readActualPosition()
        {
            String SoapResult = Executor.Execute(Executor.yReadActualPositionPath, Executor.PneumaticPlugAndPlay_WSDL, Executor.readActualPositionRequest);
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
