using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GSFCCache
{
    class PerceptionLayer
    {
        private Dictionary<string, string> Resources = new Dictionary<string, string>();

        public ResourceDetails BrowseMachine(string ResourceUrl)
        {
            ResourceDetails resourceDetails = new ResourceDetails();
            resourceDetails.ResourceUrl = ResourceUrl;
            object DynamicClientProxy = null;
            try
            {
                DynamicClientProxy = CreateDynamicClientProxy(ResourceUrl);
            }
            catch (Exception exception)
            {
                resourceDetails.IsOnline = false;
                return resourceDetails;
            }           

            resourceDetails.Name = DynamicClientProxy.GetType().GetMethod(GlobalVariables.GetResourceNameMethod).
                Invoke(DynamicClientProxy, new object[] { }).ToString();

            resourceDetails.Capability = DynamicClientProxy.GetType().GetMethod(GlobalVariables.GetCapabilityMethod).
                Invoke(DynamicClientProxy, new object[] { }).ToString();

            resourceDetails.QueueLength = Int32.Parse(DynamicClientProxy.GetType().GetMethod(GlobalVariables.GetQueueLengthMethod).
                Invoke(DynamicClientProxy, new object[] { }).ToString());

            resourceDetails.State = (ResourceState)Enum.Parse(typeof(ResourceState), 
                DynamicClientProxy.GetType().GetMethod(GlobalVariables.GetStateMethod).
                Invoke(DynamicClientProxy, new object[] { }).ToString());

            resourceDetails.ClientProxy = DynamicClientProxy;
            resourceDetails.IsOnline = true;

            return resourceDetails;
        }

        public object CreateDynamicClientProxy(string ResourceUrl)
        {
            if (!ResourceUrl.EndsWith("?wsdl", true, CultureInfo.CurrentCulture))
                ResourceUrl = string.Concat(ResourceUrl, "?wsdl");
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(ResourceUrl);
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.IO.Stream stream = httpWebResponse.GetResponseStream();

            //read the downloaded WSDL file
            System.Web.Services.Description.ServiceDescription serviceDescription = System.Web.Services.Description.ServiceDescription.Read(stream);

            Uri mexAddress = new Uri(ResourceUrl);
            MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;

            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress, mexMode);
            mexClient.ResolveMetadataReferences = true;
            MetadataSet metaSet = mexClient.GetMetadata();

            WsdlImporter importer = new WsdlImporter(metaSet);
            Collection<ContractDescription> contracts = importer.ImportAllContracts();
            ServiceEndpointCollection allEndpoints = importer.ImportAllEndpoints();

            ServiceContractGenerator generator = new ServiceContractGenerator();
            var endpointsForContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

            foreach (ContractDescription contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
                // Keep a list of each contract's endpoints
                endpointsForContracts[contract.Name] = allEndpoints.Where(se => se.Contract.Name == contract.Name).ToList();
            }

            if (generator.Errors.Count != 0)
                throw new Exception("There were errors during code compilation.");

            // Generate a code file for the contracts
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

            // Compile the code file to an in-memory assembly
            // Don't forget to add all WCF-related assemblies as references
            CompilerParameters compilerParameters = new CompilerParameters(new string[] {
                "System.dll", "System.ServiceModel.dll", "System.Runtime.Serialization.dll" });
            compilerParameters.GenerateInMemory = true;

            CompilerResults results = codeDomProvider.CompileAssemblyFromDom(
                compilerParameters, generator.TargetCompileUnit);

            if (results.Errors.Count > 0)
            {
                throw new Exception("There were errors during generated code compilation");
            }
            else
            {
                // Find the proxy type that was generated for the specified contract
                // (identified by a class that implements
                // the contract and ICommunicationbject)
                Type clientProxyType = results.CompiledAssembly.GetTypes().First(
                    t => t.IsClass &&
                        t.GetInterface(serviceDescription.PortTypes[0].Name) != null &&
                        t.GetInterface(typeof(ICommunicationObject).Name) != null);

                // Get the first service endpoint for the contract
                ServiceEndpoint se = endpointsForContracts[serviceDescription.PortTypes[0].Name].First();

                // Create an instance of the proxy
                // Pass the endpoint's binding and address as parameters
                // to the ctor
                object ClientProxyInstance = results.CompiledAssembly.CreateInstance(
                    clientProxyType.Name,
                    false,
                    System.Reflection.BindingFlags.CreateInstance,
                    null,
                    new object[] { se.Binding, se.Address },
                    CultureInfo.CurrentCulture, null);

                

                //this.Resources.Add(retVal.ToString(), )
                return ClientProxyInstance;
            }
        }

        public bool AssignWorkToResource(ResourceDetails resourceDetails, XElement RoutingStep, string Product)
        {
            return Convert.ToBoolean(resourceDetails.ClientProxy.GetType().GetMethod(GlobalVariables.DoWorkMethod).
            Invoke(resourceDetails.ClientProxy, new object[] { RoutingStep.ToXmlNode(), Product }));
        }
    }
}
