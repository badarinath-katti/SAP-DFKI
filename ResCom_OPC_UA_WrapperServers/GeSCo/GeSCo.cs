using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeSCo
{
    public class GroundingCombination
    {

        public String attributeBundle;
        public String groundingInfo;
    }
    public class RoutingSteps
    {

        public List<GroundingCombination> groundingCombination;
    }
    public class GeSCo : ApplicationMethodsBase
    {
        private static Guid empNodeId = new Guid("028c0e6a-8e44-40b9-9f6c-439a71e687e8");
        private static Guid StartMethodId = new Guid("59fcfebb-27a5-418d-ab51-b2e57051e69d");

        public GeSCo()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empNodeId;
            empMetaData.Description = "GeSCo Server";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method start
            MethodMetaData StartMetaData = new MethodMetaData();
            StartMetaData.Id = StartMethodId;
            StartMetaData.Name = "start";
            StartMetaData.Description = "This method caches the next available SFC from CMES and " +
                "starts production execution and control to produce the end-product.";
            StartMetaData.RequiresDestinationSystem = false;

            StartMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            StartMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(StartMethodId, StartMetaData);

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

        public Tuple<Dictionary<string, object>, object> start()
        {
            ServiceReference.MESimulationServiceClient mESimulationServiceClient = new ServiceReference.MESimulationServiceClient();
            XElement sfcOrder = mESimulationServiceClient.getNextSFC_Revised(1);


            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }
    }
}
