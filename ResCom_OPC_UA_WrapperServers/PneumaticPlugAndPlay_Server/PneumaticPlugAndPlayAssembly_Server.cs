using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PneumaticPlugAndPlay_Server
{
    public class PneumaticPlugAndPlayAssembly_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("348259fa-527e-4d5e-bde1-f5e1ccf01d61");
        private static Guid PrepareMethodId = new Guid("9aa13936-fdcf-43c6-85fa-e6a7d941fd25");
        private static Guid AssembleMethodId = new Guid("aef01ad4-3fe3-4836-83d0-cc808735f530");
        private static Guid IsInUseMethodId = new Guid("6d4a3411-c16a-4ca1-b03f-2b07d3d58498");
        private static Guid GetCoOrdinatesMethodId = new Guid("dfda680c-2a09-40ed-90b8-6f9d40a7897e");

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
