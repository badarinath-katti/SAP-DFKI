using SAP.Manufacturing.MethodProcessingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResCom_OPC_UA_Servers
{
    public class PneumaticPlugAndPlayAssembly_Server : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("08ab2e70-3d39-4916-aef3-9720a33c15f7");
        private static Guid MoveMethodId = new Guid("7dc7694a-b4cc-4e8d-99ea-35327a2d7b0f");
        private static Guid StopMethodId = new Guid("8188dad0-f881-449d-b199-3307d8986b93");

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
            MethodMetaData MoveMetaData = new MethodMetaData();
            MoveMetaData.Id = MoveMethodId;
            MoveMetaData.Name = "move";
            MoveMetaData.Description = "Rotates the arm of robot in specified direction";
            MoveMetaData.RequiresDestinationSystem = false;

            MoveMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            MoveMetaData.InputParameters.Add("direction", new Tuple<string, Type>("Direction of rotation", Type.GetType("System.Int32")));

            MoveMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();            
            empMetaData.Methods.Add(MoveMethodId, MoveMetaData);

            //Method SumInt
            MethodMetaData StopMetaData = new MethodMetaData();
            StopMetaData.Id = StopMethodId;
            StopMetaData.Name = "stop";
            StopMetaData.Description = "Stops rotation of arm of robot";
            StopMetaData.RequiresDestinationSystem = false;

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

        public Tuple<Dictionary<string, object>, object> move(int direction)
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> stop()
        {
            Executor.Execute(Executor.PneumaticPlugAndPlayAssemblePath, Executor.PneumaticPlugAndPlay_WSDL, Executor.pneumaticPlugAndPlayAssembleRequest);
            Dictionary<string, object> result = new Dictionary<string, object>();
            return new Tuple<Dictionary<string, object>, object>(result, null);
        }
    }
}
