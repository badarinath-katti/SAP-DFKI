using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialCamera_Server
{
    public class IndustrialCamera_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("028c0e6a-8e44-40b9-9f6c-439a71e687e8");
        private static Guid GetCoOrdinatesMethodId = new Guid("59fcfebb-27a5-418d-ab51-b2e57051e69d");
        private static Guid IdentifyMethodId = new Guid("ae526a7b-65e3-4971-a6a3-a5e618ca718c");

        public IndustrialCamera_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "OPC UA Server - Intelligent Key Finder Assembly";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();


            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets CoOrdinates from the Pneumatic Assembly Unit";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("x_CoOrdinate", new Tuple<string, Type>("X CoOrdinate of Assembly Unit", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("y_CoOrdinate", new Tuple<string, Type>("Y CoOrdinate of Assembly Unit", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("z_CoOrdinate", new Tuple<string, Type>("Z CoOrdinate of Assembly Unit", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method Identify
            MethodMetaData IdentifyMetaData = new MethodMetaData();
            IdentifyMetaData.Id = IdentifyMethodId;
            IdentifyMetaData.Name = "Identify";
            IdentifyMetaData.Description = "Identifies the Component based on RFID Read Operation";
            IdentifyMetaData.RequiresDestinationSystem = false;

            IdentifyMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            IdentifyMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(IdentifyMethodId, IdentifyMetaData);

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
            result.Add(empMetaData.Methods[GetCoOrdinatesMethodId].OutputParameters.ElementAt(1).Key, 1895423);
            result.Add(empMetaData.Methods[GetCoOrdinatesMethodId].OutputParameters.ElementAt(2).Key, 185000);

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> Identify()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

    }
}
