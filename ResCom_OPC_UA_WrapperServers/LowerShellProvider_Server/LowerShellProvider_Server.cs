using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowerShellProvider_Server
{
    public class LowerShellProvider_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("25ac884a-e0bf-402f-a202-8b805f3c7311");
        private static Guid GetCoOrdinatesMethodId = new Guid("2d8a1abb-9772-4ff4-803b-9d54d12f683d");
        private static Guid SupplyMethodId = new Guid("2a6e7258-1ef6-455b-8749-d2945810bdcf");
        private static Guid IsFilledMethodId = new Guid("123aaa8d-a073-4b4c-906a-62d7693281d0");

        public LowerShellProvider_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "LowerShell Provider - OPC-UA Server";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets Current X,Y and Z CoOrdinates of the lower shell provider";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("xCoOrdinate", new Tuple<string, Type>("X CoOrdinate of lower shell provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("yCoOrdinate", new Tuple<string, Type>("Y CoOrdinate of lower shell provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("zCoOrdinate", new Tuple<string, Type>("Z CoOrdinate of lower shell provider", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method IsFilled
            MethodMetaData IsFiledMetaData = new MethodMetaData();
            IsFiledMetaData.Id = IsFilledMethodId;
            IsFiledMetaData.Name = "isFilled";
            IsFiledMetaData.Description = "This method provides information whether warehouse has lower shell(s)";
            IsFiledMetaData.RequiresDestinationSystem = false;
            IsFiledMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            IsFiledMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(IsFilledMethodId, IsFiledMetaData);

            //Method Supply
            MethodMetaData SupplyMetaData = new MethodMetaData();
            SupplyMetaData.Id = SupplyMethodId;
            SupplyMetaData.Name = "supply";
            SupplyMetaData.Description = "This method provides lower shell(s)";
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
