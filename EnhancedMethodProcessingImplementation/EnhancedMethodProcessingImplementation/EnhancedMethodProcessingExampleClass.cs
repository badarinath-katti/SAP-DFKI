using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using SAP.Manufacturing.MethodProcessingFramework;

namespace EnhancedMethodProcessingImplementation
{
    public class EnhancedMethodProcessingExampleClass : ApplicationMethodsBase
    {
        private static Guid empTestNodeId = new Guid("77786eba-f095-4996-9dcb-4013f3d7df04");
        private static Guid sumIntegerMethodId = new Guid("e40222b8-b9ed-4054-9d76-df355cfafcf9");
        private static Guid sumDoubleMethodId = new Guid("c8f2b171-e520-4e57-bff8-4cf0eefd93e9");
        private static Guid getWeatherDataByZipId = new Guid("94b2286d-5fd1-4126-b649-e7d32f67b23b");
        private static Guid getWeatherDataByZiptMetaDataWithResponseManipulationId = new Guid("181d82d8-c566-4dc3-a29a-d1e32ae66784");

        private DestinationCallback currentDestinationCallback;

        public EnhancedMethodProcessingExampleClass()
        {
            empMetaData = createEMPExampleMetaData();
        }

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = empTestNodeId;
            empMetaData.Description = "Test Implementation of EMP methods";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            //Method SumInt
            MethodMetaData sumIntMetaData = new MethodMetaData();
            sumIntMetaData.Id = sumIntegerMethodId;
            sumIntMetaData.Name = "SumInt";
            sumIntMetaData.Description = "Calculate sum of two integers";
            sumIntMetaData.RequiresDestinationSystem = false;

            sumIntMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            sumIntMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            sumIntMetaData.InputParameters.Add("inIntOne", new Tuple<string, Type>("First Integer", Type.GetType("System.Int32")));
            sumIntMetaData.InputParameters.Add("inIntTwo", new Tuple<string, Type>("Second Integer", Type.GetType("System.Int32")));

            //Output Parameters
            sumIntMetaData.OutputParameters.Add("outSumInteger", new Tuple<string, Type>("Sum of two integers", Type.GetType("System.Int32")));

            empMetaData.Methods.Add(sumIntegerMethodId, sumIntMetaData);


            /////
            //Method SumDouble
            MethodMetaData sumDoubleMetaData = new MethodMetaData();
            sumDoubleMetaData.Id = sumDoubleMethodId;
            sumDoubleMetaData.Name = "SumDouble";
            sumDoubleMetaData.Description = "Calculate sum of two doubles";
            sumDoubleMetaData.RequiresDestinationSystem = false;
            sumDoubleMetaData.httpVerb = "POST";
            sumDoubleMetaData.parametersInHTTPBody = true;

            sumDoubleMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            sumDoubleMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            sumDoubleMetaData.InputParameters.Add("inDoubleOne", new Tuple<string, Type>("First Double", Type.GetType("System.Double")));
            sumDoubleMetaData.InputParameters.Add("inDoubleTwo", new Tuple<string, Type>("Second Double", Type.GetType("System.Double")));

            //Output Parameters
            sumDoubleMetaData.OutputParameters.Add("outSumDouble", new Tuple<string, Type>("Sum of two doubles", Type.GetType("System.Double")));

            empMetaData.Methods.Add(sumDoubleMethodId, sumDoubleMetaData);

            
            
            /////
            //Method GetWeatherDataByZip
            MethodMetaData getWeatherDataByZiptMetaData = new MethodMetaData();
            getWeatherDataByZiptMetaData.Id = getWeatherDataByZipId;
            getWeatherDataByZiptMetaData.Name = "GetWeatherDataByZip";
            getWeatherDataByZiptMetaData.Description = "Get weather data of a city";
            getWeatherDataByZiptMetaData.RequiresDestinationSystem = true;

            getWeatherDataByZiptMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            getWeatherDataByZiptMetaData.InputParameters.Add("inZipCode", new Tuple<string, Type>("Zip Code", Type.GetType("System.String")));

            //Output Parameters
            getWeatherDataByZiptMetaData.OutputParameters.Add("outCity", new Tuple<string, Type>("City", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputParameters.Add("outDescription", new Tuple<string, Type>("Description", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputParameters.Add("outTemperature", new Tuple<string, Type>("Temperature", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputParameters.Add("outWind", new Tuple<string, Type>("Wind", Type.GetType("System.String")));

            //EMP Input Variables (= input variables mapped against the web service input parameters)
            getWeatherDataByZiptMetaData.InputEmpVariables.Add("inEMPZip", new Tuple<string, Type>("Zip Code", Type.GetType("System.String")));

            //EMP Output Variables (= output variables mapped against the web service response)
            getWeatherDataByZiptMetaData.OutputEmpVariables.Add("outEMPCity", new Tuple<string, Type>("City", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputEmpVariables.Add("outEMPDescription", new Tuple<string, Type>("Description", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputEmpVariables.Add("outEMPTemperature", new Tuple<string, Type>("Temperature", Type.GetType("System.String")));
            getWeatherDataByZiptMetaData.OutputEmpVariables.Add("outEMPWind", new Tuple<string, Type>("Wind", Type.GetType("System.String")));

            empMetaData.Methods.Add(getWeatherDataByZipId, getWeatherDataByZiptMetaData);



            /////
            //Method getWeatherDataByZiptMetaDataWithResponseManipulation
            MethodMetaData getWeatherDataByZiptMetaDataWithResponseManipulationMetaData = new MethodMetaData();
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.Id = getWeatherDataByZiptMetaDataWithResponseManipulationId;
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.Name = "GetWeatherDataByZipWithResponseManipulation";
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.Description = "Manipulates the reponse of the weather of a city";
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.RequiresDestinationSystem = true;

            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.InputParameters.Add("inZipCode", new Tuple<string, Type>("Zip Code", Type.GetType("System.String")));

            //Output Parameters
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters.Add("outCity", new Tuple<string, Type>("City", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters.Add("outDescription", new Tuple<string, Type>("Description", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters.Add("outTemperature", new Tuple<string, Type>("Temperature", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters.Add("outWind", new Tuple<string, Type>("Wind", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputParameters.Add("outResponseXML", new Tuple<string, Type>("ResponseXML", Type.GetType("System.String")));

            //EMP Input Variables (= input variables mapped against the web service input parameters)
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.InputEmpVariables.Add("inEMPZip", new Tuple<string, Type>("Zip Code", Type.GetType("System.String")));

            //EMP Output Variables (= output variables mapped against the web service response)
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables.Add("outEMPCity", new Tuple<string, Type>("City", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables.Add("outEMPDescription", new Tuple<string, Type>("Description", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables.Add("outEMPTemperature", new Tuple<string, Type>("Temperature", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables.Add("outEMPWind", new Tuple<string, Type>("Wind", Type.GetType("System.String")));
            getWeatherDataByZiptMetaDataWithResponseManipulationMetaData.OutputEmpVariables.Add("outEMPResponseXML", new Tuple<string, Type>("ResponseXML", Type.GetType("System.String")));

            empMetaData.Methods.Add(getWeatherDataByZiptMetaDataWithResponseManipulationId, getWeatherDataByZiptMetaDataWithResponseManipulationMetaData);

            return empMetaData;
        }

        protected override void executeCustom(Guid methodId, DestinationCallback destinationCallback, Dictionary<string, object> inputVariables, out Tuple<Dictionary<string, object>, object> outputVariables)
        {
            currentDestinationCallback = destinationCallback;

            base.callEMPMethod(methodId, inputVariables, out outputVariables);
        }

        protected override EmpMetaData getMethodDefinitionsCustom()
        {
            return empMetaData;
        }

        public Tuple<Dictionary<string, object>, object> SumInt(int inIntOne, int inIntTwo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add(empMetaData.Methods[sumIntegerMethodId].OutputParameters.ElementAt(0).Key, inIntOne + inIntTwo);

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> SubtractInt(int inIntOne, int inIntTwo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add(empMetaData.Methods[sumIntegerMethodId].OutputParameters.ElementAt(0).Key, inIntOne - inIntTwo);

            return new Tuple<Dictionary<string, object>, object>(result, 202);
        }

        public Tuple<Dictionary<string, object>, object> SumDouble(double inDoubleOne, double inDoubleTwo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add(empMetaData.Methods[sumDoubleMethodId].OutputParameters.ElementAt(0).Key, inDoubleOne + inDoubleTwo);

            return new Tuple<Dictionary<string, object>, object>(result, null);
        }

        public Tuple<Dictionary<string, object>, object> GetWeatherDataByZip(string inZipCode)
        {
            Dictionary<string, object> destinationInput = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();

            destinationInput.Add(this.empMetaData.Methods[getWeatherDataByZipId].InputEmpVariables.ElementAt(0).Key, inZipCode);

            Dictionary<string, object> destinationResult = currentDestinationCallback.callDestination(destinationInput, false);

            result.Add(this.empMetaData.Methods[getWeatherDataByZipId].OutputParameters.ElementAt(0).Key, destinationResult["outEMPCity"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZipId].OutputParameters.ElementAt(1).Key, destinationResult["outEMPDescription"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZipId].OutputParameters.ElementAt(2).Key, destinationResult["outEMPTemperature"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZipId].OutputParameters.ElementAt(3).Key, destinationResult["outEMPWind"]);

            return new Tuple<Dictionary<string, object>, object>(result, 202);
        }

        public Tuple<Dictionary<string, object>, object> GetWeatherDataByZipWithResponseManipulation(string inZipCode)
        {
            Dictionary<string, object> destinationInput = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();

            destinationInput.Add(this.empMetaData.Methods[getWeatherDataByZipId].InputEmpVariables.ElementAt(0).Key, inZipCode);

            Dictionary<string, object> destinationResult = currentDestinationCallback.callDestination(destinationInput, false);

            result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(0).Key, 
                destinationResult["outEMPCity"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(1).Key, 
                destinationResult["outEMPDescription"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(2).Key, 
                destinationResult["outEMPTemperature"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(3).Key, 
                destinationResult["outEMPWind"]);
            result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(4).Key,
                destinationResult["outEMPResponseXML"]);

            //result.Add(this.empMetaData.Methods[getWeatherDataByZiptMetaDataWithResponseManipulationId].OutputParameters.ElementAt(0).Key,
            //    "<note><to>Tove2</to><from>Jani2</from><heading>Reminder2</heading><body>Don't forget me this weekend!2</body></note>");
            return new Tuple<Dictionary<string, object>, object>(result, 202);
        }
    }

    public class RespManipulator : iResponseManipulator
    {
        public bool isServerResponseManipulationDone(Guid methodGuid)
        {
            switch (methodGuid.ToString())
            {
                case "e40222b8-b9ed-4054-9d76-df355cfafcf9":
                    return false;
                case "c8f2b171-e520-4e57-bff8-4cf0eefd93e9":
                    return false;
                case "94b2286d-5fd1-4126-b649-e7d32f67b23b":
                    return false;
                case "181d82d8-c566-4dc3-a29a-d1e32ae66784":
                    return true;
                default:
                    return false;

            }
        }

        public XElement responseManipulator(Dictionary<string, object> webServerResult, Guid MethodGUID)
        {
            if (MethodGUID == Guid.Parse("181d82d8-c566-4dc3-a29a-d1e32ae66784"))
                return XElement.Parse(webServerResult.ElementAt(4).Value.ToString());
            else
                return null;
        }
    }
}
