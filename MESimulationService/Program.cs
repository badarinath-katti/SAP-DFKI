using MESimulationService.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MESimulationService
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8090/MESimulationService");
            //ServiceHost webServiceHost = new WebServiceHost(typeof(MESimulationService), new Uri[] { baseAddress });
            ServiceHost serviceHost = new ServiceHost(typeof(MESimulationService), new Uri[] { baseAddress });
            //WebHttpBinding webHttpBinding = new WebHttpBinding(WebHttpSecurityMode.None);
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);

            ContractDescription contractDescription = (ContractDescription.GetContract(typeof(IMESimulationService)));

            ServiceEndpoint endPoint = new ServiceEndpoint(
                contractDescription,
                basicHttpBinding,
                new EndpointAddress(baseAddress));

            serviceHost.Description.Endpoints.Add(endPoint);

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.ExportContract(contractDescription);
            serviceHost.Description.Behaviors.Add(smb);

            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;


            serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            serviceHost.Open();

            Console.WriteLine("The service is ready at {0}", baseAddress);
            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();

            //    // Close the ServiceHost.
            serviceHost.Close();

        }
    }


    

    
}
