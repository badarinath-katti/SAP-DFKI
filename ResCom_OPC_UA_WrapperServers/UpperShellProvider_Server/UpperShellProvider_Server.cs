using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpperShellProvider_Server
{
    public class UpperShellProvider_Server : ApplicationMethodsBase
    {
        private static Guid empNodeId = new Guid("348247fc-b5e4-4412-bdba-de59a6a01691");
        private static Guid GetCoOrdinatesMethodId = new Guid("1e261754-1391-4cc0-8459-8a0be1f108aa");

        public UpperShellProvider_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empNodeId;
            empMetaData.Description = "Upper Shell Provider - OPC-UA Server";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets Current X,Y and Z CoOrdinates of the Upper Shell Provider";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("xCoOrdinate", new Tuple<string, Type>("X CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("yCoOrdinate", new Tuple<string, Type>("Y CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("zCoOrdinate", new Tuple<string, Type>("Z CoOrdinate of Circuit Board Provider", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);
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
    }
}
