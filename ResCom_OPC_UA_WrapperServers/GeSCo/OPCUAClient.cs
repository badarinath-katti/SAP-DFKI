using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GeSCo
{
    class OPCUAClient
    {
        private static Session clientSession = null;
        private static ApplicationConfiguration applicationConfiguration = null;
        private static ConfiguredEndpoint configuredEndpoint = null;
        private static string sessionName = "SAPClientSession" + ": " + Guid.NewGuid();
        private static uint sessionTimeout = 60000;
        private static IUserIdentity identity = null;

        public static readonly string UAurlIdentifier = "nsurl";
        public static readonly string UAnsIdentifier = "ns";
        public static readonly char[] UANodeIDseperators = { '=', ';' };
        public static readonly char[] UANodeIDFirstSeparatorSingle = { '=' };
        public static readonly char[] UANodeIDSecondSeparatorSingle = { ';' };
        public static readonly string[] UANodeIDIDidentifier = { "i=", "s=", "g=", "b=" };
        private static readonly string UaSourceEncodingDelimiter = @"|";
        public static readonly string UaNodeIdHintEqual = "=";
        public static readonly string UaNodeIdHintPeriod = ".";
        public static void callMethod(String methodNodeID, String methodParentNodeID, object[] methodParameters)
        {
            EndpointConfiguration ec = new EndpointConfiguration();
            EndpointDescription endpointDescription = new EndpointDescription("opc.tcp://localhost:58710/PCoUaServer");
            configuredEndpoint = new ConfiguredEndpoint(new ConfiguredEndpointCollection(), endpointDescription);
            configuredEndpoint.Configuration.UseBinaryEncoding = true;
            configuredEndpoint.Description.UserIdentityTokens.Add(new UserTokenPolicy(UserTokenType.Anonymous));

            applicationConfiguration = new ApplicationConfiguration();
            applicationConfiguration.ApplicationType = ApplicationType.Client;
            applicationConfiguration.ApplicationName = "SAPClientSession";
            applicationConfiguration.ApplicationUri = "SAPClientSession";
            SecurityConfiguration secConfig = new SecurityConfiguration();
            secConfig.ApplicationCertificate = GetPCoDefaultCertificate();
            applicationConfiguration.SecurityConfiguration = secConfig;
            TransportQuotas transportQuotas = new TransportQuotas();
            applicationConfiguration.TransportQuotas = transportQuotas;
            ClientConfiguration clientConfiguration = new ClientConfiguration();
            applicationConfiguration.ClientConfiguration = clientConfiguration;
            applicationConfiguration.Validate(ApplicationType.Client);

            CertificateValidationOptions certOptions = applicationConfiguration.SecurityConfiguration.TrustedPeerCertificates.ValidationOptions;
            certOptions |= CertificateValidationOptions.SuppressCertificateExpired;
            certOptions |= CertificateValidationOptions.SuppressHostNameInvalid;

            securelyUpdateEndpointConfiguration(configuredEndpoint, applicationConfiguration);
            applicationConfiguration.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;

            clientSession = Session.Create(
                    applicationConfiguration,
                    configuredEndpoint,
                    false, false,
                    sessionName,
                    sessionTimeout,
                    identity,
                    null, null);

            clientSession.ReturnDiagnostics = DiagnosticsMasks.All;

            clientSession.KeepAliveInterval = 2 * 1000;

            try
            {
                int maxNodesPerReadRuntimeInformation = Convert.ToInt32(clientSession.ReadValue(Opc.Ua.VariableIds.Server_ServerCapabilities_OperationLimits_MaxNodesPerRead).Value);
                //if (tracer.Switch.ShouldTrace(TraceEventType.Verbose))
                {
                    String message = String.Format("Retrieved operation limits for reading ({0}) from server", maxNodesPerReadRuntimeInformation);
                    //TraceUtility.LogData(tracer, TraceUtility.EventId.E2718, TraceEventType.Verbose, message);
                }

            }
            catch (Exception)
            {
                // the value is not supplied or an error occured.
                {
                    String message = String.Format("Could not retrieve operation limits for reading from server");
                }
            }

            //NodeId nodeiD = clientSession.ReadNode();
            var response = clientSession.Call((NodeId)(methodNodeID), (NodeId)(methodParentNodeID), 
                methodParameters);

            //BrowseResultCollection browseResultCollection;
            //DiagnosticInfoCollection diagnosticInfos;
            //NodeId currentNode = FindCurrentNode(null, clientSession, configuredEndpoint);
            //uint nodeClassMask = (uint)(NodeClass.Object | NodeClass.Method);
            //BrowseDescriptionCollection nodesToBrowse = PrepareBrowseDescriptionCollection(currentNode, nodeClassMask);
            //clientSession.Browse(
            //        null,
            //        null,
            //        100,
            //        nodesToBrowse,
            //        out browseResultCollection,
            //        out diagnosticInfos);

            //var app_Base_To_method = clientSession.ReadNode((NodeId)(browseResultCollection[0].References[1].NodeId.ToString()));

            //ClientBase.ValidateResponse(browseResultCollection, nodesToBrowse);
            //ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);
            //ReferenceDescriptionCollection references;

            ////int resultLength = 0;

            //foreach (BrowseResult br in browseResultCollection)
            //{
            //    /// TODO: Store the continuation point as member of UaBrowser (new class) and
            //    /// use it when we are called with BrowseNext mode.
            //    byte[] continuationPoint = br.ContinuationPoint;
            //    while (continuationPoint != null)
            //    {
            //        byte[] revisedContinuationPoint;
            //        ResponseHeader rc = clientSession.BrowseNext(
            //            null,
            //            false,
            //            continuationPoint,
            //            out revisedContinuationPoint,
            //            out references);
            //        br.References.AddRange(references);
            //        //resultLength += br.References.Count;
            //        if (br.References.Count >= 100)
            //        {
            //            //int removeCount = br.References.Count - maxRows;
            //            br.References.RemoveRange((int)100, (int)br.References.Count - (int)100);
            //            continuationPoint = revisedContinuationPoint;
            //            if (continuationPoint != null)
            //            {
            //                // release continuation point on server
            //                rc = clientSession.BrowseNext(
            //                        null,
            //                        true, // <- release cp
            //                        continuationPoint,
            //                        out revisedContinuationPoint,
            //                        out references);
            //            }
            //            break;

            //        }
            //        continuationPoint = revisedContinuationPoint;
            //    }
            //}

            ////ns=2;s=a72e725d-6be7-4a17-bcd4-0be67b6cbfbe
            ////browseResultCollection[0].References[1].NodeId;


            //var vrere = clientSession.ReadNode((NodeId)("ns=2;s=50bcabac-623b-43ea-8f69-17b12d533166"));

            //nodesToBrowse = PrepareBrowseDescriptionCollection
            //    ((NodeId)(browseResultCollection[0].References[1].NodeId.ToString()), nodeClassMask);
            //clientSession.Browse(
            //        null,
            //        null,
            //        100,
            //        nodesToBrowse,
            //        out browseResultCollection,
            //        out diagnosticInfos);

            //var application_method = clientSession.ReadNode((NodeId)(browseResultCollection[1].References[0].NodeId.ToString()));
            //var v1 = clientSession.FetchReferences((NodeId)(browseResultCollection[1].References[0].NodeId.ToString()));
            ////var v2 = clientSession.ReadValue((NodeId)(browseResultCollection[1].References[0].NodeId.ToString()));

            //nodesToBrowse = PrepareBrowseDescriptionCollection
            //    ((NodeId)(browseResultCollection[1].References[0].NodeId.ToString()), nodeClassMask);
            //clientSession.Browse(null, null, 100, nodesToBrowse, out browseResultCollection, out diagnosticInfos);

            //var v = clientSession.ReadNode((NodeId)(browseResultCollection[5].References[0].NodeId.ToString()));
            //v1 = clientSession.FetchReferences((NodeId)(browseResultCollection[5].References[0].NodeId.ToString()));
            ////v2 = clientSession.ReadValue((NodeId)(browseResultCollection[5].References[0].NodeId.ToString()));


            ////TypeInfo.GetSystemType(dataType, clientSession.Factory);
            //nodeClassMask = (uint)(NodeClass.Object | NodeClass.Method | NodeClass.View |
            //    NodeClass.VariableType | NodeClass.Variable | NodeClass.ObjectType);
            //nodesToBrowse = PrepareBrowseDescriptionCollection
            //    ((NodeId)(browseResultCollection[5].References[0].NodeId.ToString()), nodeClassMask);
            //var v5 = clientSession.ReadNode((NodeId)(browseResultCollection[5].References[0].NodeId.ToString()));
            //clientSession.Browse(null, null, 100, nodesToBrowse, out browseResultCollection, out diagnosticInfos);


            //BrowsePathCollection pathsToArgs = new BrowsePathCollection();

            //BrowsePath pathToInputArgs = new BrowsePath();
            //pathToInputArgs.StartingNode = application_method.NodeId;
            //pathToInputArgs.RelativePath = new RelativePath(ReferenceTypeIds.HasProperty, false, true, new QualifiedName("InputArguments"));

            //pathsToArgs.Add(pathToInputArgs);

            //BrowsePath pathToOutputArgs = new BrowsePath();
            //pathToOutputArgs.StartingNode = application_method.NodeId;
            //pathToOutputArgs.RelativePath = new RelativePath(ReferenceTypeIds.HasProperty, false, true, new QualifiedName("OutputArguments"));

            //pathsToArgs.Add(pathToOutputArgs);

            //BrowsePathResultCollection results = null;
            //// Get the nodeId of the input argument
            //ResponseHeader responseHeader = clientSession.TranslateBrowsePathsToNodeIds(
            //    null,
            //    pathsToArgs,
            //    out results,
            //    out diagnosticInfos
            //    );

            //ArgumentCollection[] arguments = new ArgumentCollection[2];
            //for (int i = 0; i < 2; i++)
            //{
            //    arguments[i] = new ArgumentCollection();
            //    foreach (BrowsePathTarget bptarget in results[i].Targets)
            //    {
            //        DataValueCollection readResults = null;

            //        ReadValueId nodeToRead = new ReadValueId();
            //        nodeToRead.NodeId = (NodeId)bptarget.TargetId;
            //        nodeToRead.AttributeId = Attributes.Value;
            //        ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            //        nodesToRead.Add(nodeToRead);

            //        DiagnosticInfoCollection readDiagnoistcInfos = null;

            //        clientSession.Read(
            //            null,
            //            0,
            //            TimestampsToReturn.Neither,
            //            nodesToRead,
            //            out readResults,
            //            out readDiagnoistcInfos
            //            );

            //        ExtensionObject[] exts = (ExtensionObject[])readResults[0].Value;
            //        for (int j = 0; j < exts.Length; ++j)
            //        {
            //            ExtensionObject ext = exts[j];
            //            arguments[i].Add((Argument)ext.Body);
            //        }

            //    }
            //}
            // establish keep-alive
            //clientSession.KeepAlive += new KeepAliveEventHandler(clientSession_KeepAlive);
        }

        private static void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateTimeInvalid)
            {
                //if (isInvalidPeriodAccepted)
                {
                    e.Accept = true;
                    return;
                }
                return;
            }
            else if (e.Error.StatusCode == StatusCodes.BadCertificateHostNameInvalid)
            {
                //if (isInvalidHostNameAllowed)
                {
                    e.Accept = true;
                    return;
                }
            }
            else if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                if (configuredEndpoint.Description.SecurityPolicyUri == SecurityPolicies.None
                    && configuredEndpoint.Description.SecurityMode == MessageSecurityMode.None)
                {
                    e.Accept = true;
                    return;
                }
            }

            e.Accept = false;
            return;
        }

        private static void securelyUpdateEndpointConfiguration(ConfiguredEndpoint ep, ApplicationConfiguration applicationConfiguration)
        {
            // ep.UpdateFromServer(...) looks for matching endpoints on the server.
            // If there is no match with the same securityMode and securityPolicy, it will simply 
            // _replace_ ep by one of the server endpoints, possibly lowering the security settings. Therefore:
            // save old settings - update from server - verify security settings -- and throw update from server away if security settings have changed.
            // note that ConfiguredEndpoints need a collection to live in. Note also that some settings of the configured endpoint may be changed nonetheless behind the scenes.
            ConfiguredEndpointCollection cepc = new ConfiguredEndpointCollection();
            EndpointDescription epd = (EndpointDescription)ep.Description.Clone();
            ConfiguredEndpoint cep = new ConfiguredEndpoint(cepc, epd);

            Opc.Ua.BindingFactory bindingFactory = BindingFactory.Create(applicationConfiguration, applicationConfiguration.CreateMessageContext());
            ep.UpdateFromServer(bindingFactory);

            if (ep.Description.SecurityMode != epd.SecurityMode || ep.Description.SecurityPolicyUri != epd.SecurityPolicyUri)
            {
                ep.Update(cep);
                throw ServiceResultException.Create(StatusCodes.BadConfigurationError, "No endpoint with matching security configuration could be found during updated on connect.");
            }
        }

        internal static CertificateIdentifier GetPCoDefaultCertificate()
        {
            // Reads the untrusted test certificate "PCo_Default_Untrusted_Certificate.pfx" from the
            // embedded project resources. If you need to replace the certificate with another one, perform the
            // following steps:
            // 1. Create a new certificate using makecert.exe:
            //    makecert -r -pe -n "CN=PCo Untrusted Test Certificate" -b 01/01/2000 -e 01/01/2099 -ss my
            // 2. Export the certificate from your private certificate store to a file named
            //    "PCo_Default_Untrusted_Certificate.pfx." Export the certificate together with the private key.
            //    The password has to be "pco".
            // 3. Replace the existing project resource "PCo_Default_Untrusted_Certificate.pfx" with the new file. 
            //    Make sure that the "build action" property of the resource is set to "Embedded Resource".
            // 4. Set a breakpoint in this method at statement
            //    if (!cert.Thumbprint.Equals(OpcUaAgent.DefaultPCoApplCertificateThumbprint))
            // 5. Run the PCo Management Console and create an OPC UA source system. The system should stop at the
            //    breakpoint. Now look up the property "Thumbprint" of the certificate object "cert". Write the
            //    thumbprint value into the constant OpcUaAgent.DefaultPCoApplCertificateThumbprint

            byte[] certByteArray;
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream certStream = assembly.GetManifestResourceStream("ConsoleApp1.Resources.PCo_Default_Untrusted_Certificate.pfx"))
            {
                certByteArray = new byte[certStream.Length];
                certStream.Read(certByteArray, 0, (int)certStream.Length);
            }

            X509Certificate2 cert = new X509Certificate2(certByteArray, "pco", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

            return new CertificateIdentifier(cert);
        }

        private static BrowseDescriptionCollection PrepareBrowseDescriptionCollection(
                NodeId nodeId, uint nodeClassMask)
        {
            // We do not inspect all references, but only organize and HasComponent.
            BrowseDescriptionCollection browseDescriptionCollection = new BrowseDescriptionCollection();
            BrowseDescription nodeToBrowse = new BrowseDescription()
            {
                NodeId = nodeId,
                ReferenceTypeId = ReferenceTypeIds.Organizes,
                IncludeSubtypes = true,
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = nodeClassMask,
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            nodeToBrowse = new BrowseDescription()
            {
                NodeId = nodeId,
                ReferenceTypeId = ReferenceTypeIds.HasComponent,
                IncludeSubtypes = true,
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = nodeClassMask,
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            nodeToBrowse = new BrowseDescription()
            {
                NodeId = nodeId,
                ReferenceTypeId = ReferenceTypeIds.HasChild,
                IncludeSubtypes = true,
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = nodeClassMask,
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            nodeToBrowse = new BrowseDescription()
            {
                NodeId = nodeId,
                ReferenceTypeId = ReferenceTypeIds.HasDescription,
                IncludeSubtypes = true,
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = nodeClassMask,
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            nodeToBrowse = new BrowseDescription()
            {
                NodeId = nodeId,
                ReferenceTypeId = ReferenceTypeIds.HasProperty,
                IncludeSubtypes = true,
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = nodeClassMask,
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            nodeToBrowse = new BrowseDescription
            {
                NodeId = nodeId,
                BrowseDirection = BrowseDirection.Forward,
                ReferenceTypeId = ReferenceTypeIds.HasProperty,
                IncludeSubtypes = true,
                NodeClassMask = (uint)(NodeClass.Variable | NodeClass.Object),
                ResultMask = (uint)BrowseResultMask.All
            };
            browseDescriptionCollection.Add(nodeToBrowse);

            return browseDescriptionCollection;
        }

        internal static NodeId FindCurrentNode(
                string level,
                Opc.Ua.Client.Session session,
                ConfiguredEndpoint endpoint
            )
        {
            NodeId curNode;
            if (string.IsNullOrEmpty(level))
            {
                // Set the default root OPC node id to look at the objects
                curNode = Objects.ObjectsFolder;
            }
            else
            {
                // need to decode the level to get the NodeId
                string source;
                if (level.StartsWith(UAurlIdentifier + "="))
                {
                    source = getNSIStringfromNSURLString(level, session);
                }
                else
                {
                    source = level;
                }
                //curNode = (NodeId)source;
                curNode = GetNodeId(source, false, session, endpoint);
            }
            return curNode;
        }

        internal static string getNSIStringfromNSURLString(string nsurlString, Opc.Ua.Client.Session clientSession,
            Dictionary<int, string> nameSpaceIndexDict = null)
        {
            if (nsurlString == null || nsurlString.Equals(string.Empty))
            {
                return nsurlString;
            }

            string[] firstDecomposition = nsurlString.Split(UANodeIDFirstSeparatorSingle, 2);
            if (firstDecomposition.Length != 2 || !UAurlIdentifier.Equals(firstDecomposition[0]))
            {
                throw new ApplicationException("Invalid input when converting namespace URL to namespace index)");
            }
            string[] secondDecomposition = firstDecomposition[1].Split(UANodeIDSecondSeparatorSingle, 2);
            if (secondDecomposition.Length != 2 || secondDecomposition[1].Length < 2)
            {
                throw new ApplicationException("Invalid input when converting namespace URL to namespace index)");
            }

            // verify the last part begins with "i=", "s=", "g=" or "b="
            string idTypeID = secondDecomposition[1].Trim().Substring(0, 2);
            if (!(UANodeIDIDidentifier.Any(s => idTypeID.Equals(s))))
            {
                throw new ApplicationException("Invalid input when converting namespace URL to namespace index: no valid IdType detected)");
            }
            if (nameSpaceIndexDict == null)
            {
                int nsindex = clientSession.NamespaceUris.GetIndex(secondDecomposition[0]);
                if (nsindex < 0) throw new ApplicationException("Invalid input when converting namespace URL to namespace index: namespace index is negative)");
                return UAnsIdentifier + "=" + nsindex.ToString() + ";" + secondDecomposition[1];
            }
            else
            {
                // the index is unique or there is an error.
                int index = nameSpaceIndexDict.FirstOrDefault(x => x.Value.Equals(secondDecomposition[0])).Key;
                return UAnsIdentifier + "=" + index.ToString() + ";" + secondDecomposition[1];
            }
        }

        public static NodeId GetNodeId(string source,
                        bool firstOnly,
                        Opc.Ua.Client.Session session,
                        ConfiguredEndpoint endpoint)
        {
            NodeId nodeId = null;

            try
            {
                // Determine Node Id

                // if the tag comes from a persisted cache element, it may start with "nsurl="
                if (source.StartsWith(UAurlIdentifier + "=")) source = getNSIStringfromNSURLString(source, session);

                // in some cases we can shortcut: ns=ABC or i=2253 etc. or Channel1.Device1.Tag_1 are already Node Ids
                if (source.Contains(UaNodeIdHintEqual) || (source.Contains(UaNodeIdHintPeriod)))
                {
                    nodeId = (NodeId)source;
                }
            }
            catch (ServiceResultException srex)
            {
                if ((srex.StatusCode == StatusCodes.BadCommunicationError) || (srex.StatusCode == StatusCodes.BadConnectionClosed))
                {
                    //ConfiguredEndpoint endpoint = (ConfiguredEndpoint)this.agentInstance.SourceChannel.Aspects[OpcUaAgent.SessionConfigurationAspectName]
                    //    .Properties[OpcUaAgent.SessionEndpointProperty].BinaryValue.Value;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to determine Node Id from source path ", source, ex);
            }

            return nodeId;
        }

        internal static string getNSUrlStringfromNodeId(NodeId nodeId, Opc.Ua.Client.Session session)
        {
            if (nodeId == null || nodeId.IsNullNodeId) return string.Empty;

            string[] idSeparatorArray = { getNSIshortCutfromIDType(nodeId.IdType) };
            string nodeIDString = nodeId.ToString();

            if (nodeId.NamespaceIndex != 0)
            {
                // Hack. Remvove 'ns' from the beginning of the string so there is no collision with the later search for "s="
                // only needed for nodeId.idType == String
                nodeIDString = nodeIDString.Substring(2);
            }

            string[] idParts = nodeIDString.Split(idSeparatorArray, StringSplitOptions.RemoveEmptyEntries);

            string[] idParts2 = nodeIDString.Split(idSeparatorArray, StringSplitOptions.None);
            string result = String.Empty;
            // we expect one string in case ns=0, two otherwise
            // in case ns=0, empty identifier is not allowed, otherwise it is
            // learnt this at Opc IOP Nürnberg 201411
            if (!((idParts.Length == 1 && nodeId.NamespaceIndex == 0) || idParts.Length == 2))
            {
                if (idParts2.Length == idParts.Length || nodeId.NamespaceIndex == 0)
                {
                    // OPC IOW 2014
                    // some parts of the nodeid string are empty. This is allowed (according to 
                    // personal communication with Randy Newman) if !ns==0
                    throw (new ApplicationException("invalid String Representation of UA node"));
                }
                result = UAurlIdentifier + "=" + session.NodeCache.NamespaceUris.GetString(nodeId.NamespaceIndex) + ";" +
                    idSeparatorArray[0];
                return result;
            }
            result = UAurlIdentifier + "=" + session.NodeCache.NamespaceUris.GetString(nodeId.NamespaceIndex) + ";" +
                idSeparatorArray[0] +
                idParts[idParts.Length - 1];

            return result;
        }

        private static string getNSIshortCutfromIDType(IdType t)
        {
            // string constants are taken from the opc UA core library source (version 1.2.334.5)
            switch (t)
            {
                case IdType.Guid:
                    {
                        return "g=";
                    }
                case IdType.Numeric:
                    {
                        return "i=";
                    }
                case IdType.Opaque:
                    {
                        return "b=";
                    }
                case IdType.String:
                    {
                        return "s=";
                    }
                default: //should not happen, but t might be null?
                    return string.Empty;
            }
        }
    }
}
