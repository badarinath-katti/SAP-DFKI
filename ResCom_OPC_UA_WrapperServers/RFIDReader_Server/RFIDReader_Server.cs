using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RFIDReader_Server
{
    public class RFIDReader_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("10e9862c-b06e-4e84-ad95-4373eb72690a");
        private static Guid GetCoOrdinatesMethodId = new Guid("8f3d0508-1943-41a7-9bb1-020e80a54e5d");
        private static Guid RFIDIdentifyMethodId = new Guid("0377fa55-4010-4c75-9139-497099100539");
        private static Guid ReadMethodId = new Guid("b5420720-d4af-4c2d-9598-c647031ba7b2");
        private static Guid WriteMethodId = new Guid("b74fac34-da83-43c5-999e-6560b61e4e30");

        public RFIDReader_Server()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private DestinationCallback currentDestinationCallback;

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "OPC UA Server - RFID Reader";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method GetCoOrdinates
            MethodMetaData GetCoOrdinatesMetaData = new MethodMetaData();
            GetCoOrdinatesMetaData.Id = GetCoOrdinatesMethodId;
            GetCoOrdinatesMetaData.Name = "GetCoOrdinates";
            GetCoOrdinatesMetaData.Description = "Gets CoOrdinates of the RFID Reader";
            GetCoOrdinatesMetaData.RequiresDestinationSystem = false;

            GetCoOrdinatesMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();

            GetCoOrdinatesMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetCoOrdinatesMetaData.OutputParameters.Add("x_CoOrdinate", new Tuple<string, Type>("X CoOrdinate of RFID Reader", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("y_CoOrdinate", new Tuple<string, Type>("Y CoOrdinate of RFID Reader", Type.GetType("System.Int32")));
            GetCoOrdinatesMetaData.OutputParameters.Add("z_CoOrdinate", new Tuple<string, Type>("Z CoOrdinate of RFID Reader", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(GetCoOrdinatesMethodId, GetCoOrdinatesMetaData);

            //Method Identify
            MethodMetaData IdentifyMetaData = new MethodMetaData();
            IdentifyMetaData.Id = RFIDIdentifyMethodId;
            IdentifyMetaData.Name = "rfidIdentify";
            IdentifyMetaData.Description = "Identifies the Component based on RFID Read Operation";
            IdentifyMetaData.RequiresDestinationSystem = false;

            IdentifyMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            IdentifyMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            empMetaData.Methods.Add(RFIDIdentifyMethodId, IdentifyMetaData);

            //Method Read
            MethodMetaData ReadMetaData = new MethodMetaData();
            ReadMetaData.Id = ReadMethodId;
            ReadMetaData.Name = "read";
            ReadMetaData.Description = "RFID Read Operation";
            ReadMetaData.RequiresDestinationSystem = false;

            ReadMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            ReadMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            ReadMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            ReadMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            ReadMetaData.InputParameters.Add("position", new Tuple<string, Type>( "Position of Read Operation", Type.GetType("System.Int32")));
            ReadMetaData.InputParameters.Add("length", new Tuple<string, Type>("Length of Read Operation", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(ReadMethodId, ReadMetaData);

            //Method Write
            MethodMetaData WriteMetaData = new MethodMetaData();
            WriteMetaData.Id = WriteMethodId;
            WriteMetaData.Name = "write";
            WriteMetaData.Description = "RFID Write Operation";
            WriteMetaData.RequiresDestinationSystem = false;

            WriteMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            WriteMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            WriteMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            WriteMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            WriteMetaData.InputParameters.Add("position", new Tuple<string, Type>("Position of Read Operation", Type.GetType("System.Int32")));
            WriteMetaData.InputParameters.Add("data", new Tuple<string, Type>("Length of Read Operation", Type.GetType("System.Int32[]")));

            empMetaData.Methods.Add(WriteMethodId, WriteMetaData);


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

        public Tuple<Dictionary<string, object>, object> identify()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> read(int position, int length)
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> write(int position, int[] data)
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }
    }
}
