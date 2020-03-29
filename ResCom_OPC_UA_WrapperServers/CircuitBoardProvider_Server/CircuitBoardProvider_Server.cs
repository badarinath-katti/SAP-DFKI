using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitBoardProvider_Server
{
    public class CircuitBoardProvider_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("fc73ad48-e50e-414b-896f-df55fc1948c6");
        private static Guid GetCoOrdinatesMethodId = new Guid("15d2b708-1dfb-4737-ba54-1415bc083029");
        private static Guid SupplyMethodId = new Guid("1d2de0f6-222b-4873-a196-5718d8f606b8");
        private static Guid IsFilledMethodId = new Guid("c1b1f8ae-1f53-4d4c-b3f8-9a0303885707");

        public CircuitBoardProvider_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "Circuit Board Provider - OPC-UA Server";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets Current X,Y and Z CoOrdinates of the Circuit Board Provider";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("xCoOrdinate", new Tuple<string, Type>("X CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("yCoOrdinate", new Tuple<string, Type>("Y CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("zCoOrdinate", new Tuple<string, Type>("Z CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method IsFilled
            MethodMetaData IsFiledMetaData = new MethodMetaData();
            IsFiledMetaData.Id = IsFilledMethodId;
            IsFiledMetaData.Name = "isFilled";
            IsFiledMetaData.Description = "This method provides information whether warehouse has circuit board(s)";
            IsFiledMetaData.RequiresDestinationSystem = false;
            IsFiledMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            IsFiledMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(IsFilledMethodId, IsFiledMetaData);

            //Method Supply
            MethodMetaData SupplyMetaData = new MethodMetaData();
            SupplyMetaData.Id = SupplyMethodId;
            SupplyMetaData.Name = "supply";
            SupplyMetaData.Description = "This method provides circuit boards(s)";
            SupplyMetaData.RequiresDestinationSystem = false;

            SupplyMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            SupplyMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            SupplyMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            SupplyMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(SupplyMethodId, SupplyMetaData);

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
            result.Add(empMetaData.Methods[GetCoOrdinatesMethodId].OutputParameters.ElementAt(1).Key, 379486);
            result.Add(empMetaData.Methods[GetCoOrdinatesMethodId].OutputParameters.ElementAt(2).Key, 379486);

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> isFilled()
        {
            String SoapResult = Executor.Execute(Executor.xMoveToHomePath, Executor.translateElectricLinmot_WSDL, Executor.moveAbsoluteRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> suppply()
        {
            Executor.Execute(Executor.xMoveToHomePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }
    }
}
