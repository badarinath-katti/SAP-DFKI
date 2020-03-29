using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Manufacturing_Execution_Simulation.Application_Utility
{
    public class RestClient
    {
        public static string SendRequest(string uri, string method, string contentType, string body)
        {
            string responseBody = null;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.Method = method;
            if (!String.IsNullOrEmpty(contentType))
            {
                req.ContentType = contentType;
            }
            if (!String.IsNullOrEmpty(body))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                req.GetRequestStream().Write(bodyBytes, 0, bodyBytes.Length);
                req.GetRequestStream().Close();
            }
            //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("PCo" + ":" + "Lazmilazmi15"));
            //req.Headers.Add("Authorization", "Basic " + svcCredentials);

            /*X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            X509Certificate certificate = store.Certificates
                 .Find(X509FindType.FindBySubjectName, "Cert2", false)
                 .OfType<X509Certificate>()
                 .First();
            //req.PreAuthenticate = true;
            req.ClientCertificates.Add(certificate);*/

            //ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertficate;



            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException e)
            {
                resp = (HttpWebResponse)e.Response;
            }
            Console.WriteLine("HTTP/{0} {1} {2}", resp.ProtocolVersion, (int)resp.StatusCode, resp.StatusDescription);
            foreach (string headerName in resp.Headers.AllKeys)
            {
                Console.WriteLine("{0}: {1}", headerName, resp.Headers[headerName]);
            }
            Console.WriteLine();
            Stream respStream = resp.GetResponseStream();
            if (respStream != null)
            {
                responseBody = new StreamReader(respStream).ReadToEnd();
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine("HttpWebResponse.GetResponseStream returned null");
            }
            Console.WriteLine();
            Console.WriteLine(" *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* ");
            Console.WriteLine();

            return responseBody;
        }
    }
}