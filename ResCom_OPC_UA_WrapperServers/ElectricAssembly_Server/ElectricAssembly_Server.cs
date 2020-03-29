using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricAssembly_Server
{
    public class PneumaticPlugAndPlayAssembly_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("1bd7239f-3060-43c4-95d9-18f72cc78712");
        private static Guid PrepareMethodId = new Guid("8b081ad7-9f2a-47ac-b282-7c5450a95165");
        private static Guid AssembleMethodId = new Guid("7144825d-6166-4cd8-86bc-eda6a8cc1415");
        private static Guid IsInUseMethodId = new Guid("213e3dbe-9ea4-4dc1-b730-59694a68e9e5");
        private static Guid GetCoOrdinatesMethodId = new Guid("baa1892a-ca51-4679-a214-cdbb3fbc9dec");

        public PneumaticPlugAndPlayAssembly_Server()
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

            //Method SumInt
            MethodMetaData PrepareMetaData = new MethodMetaData();
            PrepareMetaData.Id = PrepareMethodId;
            PrepareMetaData.Name = "Prepare";
            PrepareMetaData.Description = "Prepares for assembly of individual components of Key Finder";
            PrepareMetaData.RequiresDestinationSystem = false;

            PrepareMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            PrepareMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            PrepareMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            PrepareMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(PrepareMethodId, PrepareMetaData);


            /////
            //Method Assemble
            MethodMetaData AssembleMetaData = new MethodMetaData();
            AssembleMetaData.Id = AssembleMethodId;
            AssembleMetaData.Name = "Assemble";
            AssembleMetaData.Description = "Assembles the Key Finder Components";
            AssembleMetaData.RequiresDestinationSystem = false;
            AssembleMetaData.httpVerb = "POST";
            AssembleMetaData.parametersInHTTPBody = true;

            AssembleMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            AssembleMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            AssembleMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            AssembleMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(AssembleMethodId, AssembleMetaData);

            /////
            //Method IsInUse
            MethodMetaData IsInUseMetaData = new MethodMetaData();
            IsInUseMetaData.Id = IsInUseMethodId;
            IsInUseMetaData.Name = "IsInUse";
            IsInUseMetaData.Description = "Provides the Status of the Assembly Unit";
            IsInUseMetaData.RequiresDestinationSystem = false;

            IsInUseMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            IsInUseMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            IsInUseMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            IsInUseMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            empMetaData.Methods.Add(IsInUseMethodId, IsInUseMetaData);

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

        public Tuple<Dictionary<string, object>, object> Assemble()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> Prepare()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayPreparePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayPrepareRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> IsInUse()
        {
            String SoapResult = Executor.Execute(Executor.PneumaticPlugAndPlayIsInUsePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayIsInUseRequest);

            Dictionary<string, object> result = new Dictionary<string, object>();

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }
    }
}
