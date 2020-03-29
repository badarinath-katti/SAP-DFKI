using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Manufacturing_Execution_Simulation.Application_Utility
{
    public class OPCUAUtility
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
        public static readonly int UaDefaultNamespaceIndex = 0;

        public static OpcuaNodeList opcuaNodeStructure = new OpcuaNodeList();
        private static readonly Object lockObject = new Object();


        public static void connectToOPCUAServer(String opcuaServerUrl)
        {
            #region SessionConfiguration
            EndpointConfiguration ec = new EndpointConfiguration();
            EndpointDescription endpointDescription = new EndpointDescription(opcuaServerUrl);
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

            #endregion
            opcuaNodeStructure.ChildrenNodes = new List<OpcuaNodeLink>();

            //Add root node
            OpcuaNodeLink opcuaNodeLink = new OpcuaNodeLink();
            opcuaNodeLink.nodeID = FindCurrentNode(null, clientSession, configuredEndpoint).ToString();
            opcuaNodeLink.browseName = "AddressRoot";
            opcuaNodeLink.parentNodeId = String.Empty;
            opcuaNodeLink.parentNodebrowseName = String.Empty;
            opcuaNodeLink.isMethod = opcuaNodeLink.isArgument = false;
            opcuaNodeStructure.ChildrenNodes.Add(opcuaNodeLink);

            BrowseNode(FindCurrentNode(null, clientSession, configuredEndpoint).ToString(), opcuaNodeLink.browseName);
        }

        public static void BrowseNode(String currentNode, String browseName)
        {
            BrowseResultCollection browseResultCollection = null;
            DiagnosticInfoCollection diagnosticInfos;
            BrowseDescriptionCollection browseDescriptionCollection = null;
            List<NodeClass> nodeClasses = new List<NodeClass>() { NodeClass.Unspecified };

            foreach (NodeClass nodeClass in nodeClasses)
            {
                browseDescriptionCollection = PrepareBrowseDescriptionCollection(currentNode, (uint)nodeClass);
                clientSession.Browse(
                        null,
                        null,
                        100,
                        browseDescriptionCollection,
                        out browseResultCollection,
                        out diagnosticInfos);

                if (browseResultCollection.Where(br => br.References.Count > 0).FirstOrDefault() == null)
                    continue;
                int browseResultCounter = browseResultCollection.FindIndex(br => br.References.Count > 0);
                String referenceType = String.Empty;
                
                foreach (BrowseResult browseResult in browseResultCollection)
                {
                    switch (browseResultCounter)
                    {
                        case 0:
                            referenceType = "Organizes";
                            break;
                        case 1:
                            referenceType = "HasComponent";
                            break;
                        case 2:
                            referenceType = "HasChild";
                            break;
                        case 3:
                            referenceType = "HasDescription";
                            break;
                        case 4:
                            referenceType = "HasProperty";
                            break;
                        default:
                            referenceType = "HasProperty";
                            break;
                    }

                    if (browseResult.References.Count > 0)
                    {
                        foreach (ReferenceDescription referenceDescription in browseResult.References)
                        {
                            if (referenceDescription.BrowseName.NamespaceIndex != UaDefaultNamespaceIndex)
                                BrowseNode(referenceDescription.NodeId.ToString(), referenceDescription.BrowseName.ToString());

                            //TBD - If a node id has different relations with its parent node, the additional "relation" should be added
                            //OpcuaNodeList opcuaNodeSubStructure = new OpcuaNodeList();
                            //opcuaNodeSubStructure.nodeId = referenceDescription.NodeId.ToString();
                            //opcuaNodeSubStructure.browseName = referenceDescription.BrowseName.ToString();

                            OpcuaNodeLink opcuaNodeLink = new OpcuaNodeLink();

                            opcuaNodeLink.nodeID = referenceDescription.NodeId.ToString();
                            opcuaNodeLink.browseName = referenceDescription.BrowseName.ToString();
                            opcuaNodeLink.parentNodeId = currentNode;
                            opcuaNodeLink.parentNodebrowseName = browseName;

                            opcuaNodeLink.referenceType = referenceType;
                            if (referenceDescription.IsForward)
                                opcuaNodeLink.BrowseDirection = BrowseDirection.Forward.ToString("G");
                            opcuaNodeLink.NodeClassMask = nodeClass.ToString("G") ;
                            opcuaNodeLink.browseResultMask = BrowseResultMask.All.ToString("G");

                            //if (!opcuaNodeStructure.IsChildPresent(referenceDescription.NodeId.ToString()))
                            if (opcuaNodeStructure.ChildrenNodes == null)
                                opcuaNodeStructure.ChildrenNodes = new List<OpcuaNodeLink>();

                            addNode(opcuaNodeLink);

                            if (referenceDescription.NodeClass == NodeClass.Method)
                            {

                                Node methodNode = clientSession.ReadNode((NodeId)(referenceDescription.NodeId.ToString()));

                                browseDescriptionCollection = PrepareBrowseDescriptionCollection(referenceDescription.NodeId.ToString(), (uint)nodeClass);
                                clientSession.Browse(
                                        null,
                                        null,
                                        100,
                                        browseDescriptionCollection,
                                        out browseResultCollection,
                                        out diagnosticInfos);

                                if (browseResultCollection.Where(br => br.References.Count > 0).FirstOrDefault() == null)
                                    continue;

                                String inputArgumentsNodeId = String.Empty, outputArgumentsNodeId = String.Empty;
                                foreach (BrowseResult br in browseResultCollection)
                                {
                                    foreach (ReferenceDescription rd in br.References)
                                    {
                                        OpcuaNodeLink nodeLink = new OpcuaNodeLink();

                                        nodeLink.nodeID = rd.NodeId.ToString();
                                        nodeLink.browseName = rd.BrowseName.ToString();

                                        if (nodeLink.browseName == "InputArguments")
                                            inputArgumentsNodeId = nodeLink.nodeID;
                                        else if (nodeLink.browseName == "OutputArguments")
                                            outputArgumentsNodeId = nodeLink.nodeID;

                                        nodeLink.parentNodeId = referenceDescription.NodeId.ToString();
                                        nodeLink.parentNodebrowseName = referenceDescription.BrowseName.ToString();

                                        nodeLink.referenceType = referenceType;
                                        if (rd.IsForward)
                                            opcuaNodeLink.BrowseDirection = BrowseDirection.Forward.ToString("G");
                                        nodeLink.NodeClassMask = nodeClass.ToString("G");
                                        nodeLink.browseResultMask = BrowseResultMask.All.ToString("G");

                                        //if (!opcuaNodeStructure.IsChildPresent(referenceDescription.NodeId.ToString()))
                                        if (opcuaNodeStructure.ChildrenNodes == null)
                                            opcuaNodeStructure.ChildrenNodes = new List<OpcuaNodeLink>();

                                        addNode(nodeLink);

                                    }
                                }

                                opcuaNodeLink.isMethod = true;

                                BrowsePathCollection pathsToArgs = new BrowsePathCollection();
                                BrowsePath pathToInputArgs = new BrowsePath();
                                pathToInputArgs.StartingNode = methodNode.NodeId;
                                pathToInputArgs.RelativePath = new RelativePath(ReferenceTypeIds.HasProperty, false, true, new QualifiedName("InputArguments"));
                                pathsToArgs.Add(pathToInputArgs);

                                BrowsePath pathToOutputArgs = new BrowsePath();
                                pathToOutputArgs.StartingNode = methodNode.NodeId;
                                pathToOutputArgs.RelativePath = new RelativePath(ReferenceTypeIds.HasProperty, false, true, new QualifiedName("OutputArguments"));
                                pathsToArgs.Add(pathToOutputArgs);

                                BrowsePathResultCollection results = null;
                                // Get the nodeId of the input argument
                                ResponseHeader responseHeader = clientSession.TranslateBrowsePathsToNodeIds(
                                    null,
                                    pathsToArgs,
                                    out results,
                                    out diagnosticInfos
                                    );

                                ArgumentCollection[] arguments = new ArgumentCollection[2];
                                for (int i = 0; i < 2; i++)
                                {
                                    arguments[i] = new ArgumentCollection();
                                    foreach (BrowsePathTarget bptarget in results[i].Targets)
                                    {
                                        DataValueCollection readResults = null;

                                        ReadValueId nodeToRead = new ReadValueId();
                                        nodeToRead.NodeId = (NodeId)bptarget.TargetId;
                                        nodeToRead.AttributeId = Attributes.Value;
                                        ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
                                        nodesToRead.Add(nodeToRead);

                                        DiagnosticInfoCollection readDiagnoistcInfos = null;

                                        clientSession.Read(
                                            null,
                                            0,
                                            TimestampsToReturn.Neither,
                                            nodesToRead,
                                            out readResults,
                                            out readDiagnoistcInfos
                                            );

                                        ExtensionObject[] exts = (ExtensionObject[])readResults[0].Value;
                                        for (int j = 0; j < exts.Length; ++j)
                                        {
                                            ExtensionObject ext = exts[j];
                                            arguments[i].Add((Argument)ext.Body);

                                            OpcuaNodeLink nl = new OpcuaNodeLink();

                                            nl.nodeID = ((Opc.Ua.Argument)ext.Body).BinaryEncodingId.ToString();
                                            nl.browseName = ((Opc.Ua.Argument)ext.Body).Name;

                                            if (((Opc.Ua.Argument)ext.Body).Description.Text.Equals("Input argument"))
                                                nl.parentNodeId = inputArgumentsNodeId;
                                            else if (((Opc.Ua.Argument)ext.Body).Description.Text.Equals("Output argument"))
                                                nl.parentNodeId = outputArgumentsNodeId;

                                            nl.parentNodebrowseName = referenceDescription.BrowseName.ToString();

                                            nl.referenceType = referenceType;
                                            if (referenceDescription.IsForward)
                                                opcuaNodeLink.BrowseDirection = BrowseDirection.Forward.ToString("G");
                                            nl.NodeClassMask = NodeClass.Variable.ToString("G");
                                            nl.browseResultMask = BrowseResultMask.All.ToString("G");
                                            nl.isArgument = true;

                                            //if (!opcuaNodeStructure.IsChildPresent(referenceDescription.NodeId.ToString()))
                                            if (opcuaNodeStructure.ChildrenNodes == null)
                                                opcuaNodeStructure.ChildrenNodes = new List<OpcuaNodeLink>();

                                            addNode(nl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #region OPCUA_SessionCreation

        private static void addNode(OpcuaNodeLink opcuaNodeLink)
        {
            lock (lockObject)
            {
                OpcuaNodeLink nodeLink = opcuaNodeStructure.ChildrenNodes.Where(node => (node.nodeID.Equals(opcuaNodeLink.nodeID)
                && (node.parentNodeId.Equals(opcuaNodeLink.parentNodeId) 
                && (node.browseName.Equals(opcuaNodeLink.browseName))))).FirstOrDefault();
                if (nodeLink == null)
                    opcuaNodeStructure.ChildrenNodes.Add(opcuaNodeLink);
                /*else
                    opcuaNodeStructure.ChildrenNodes[nodeId.ToString()].Add(opcuaNodeLink);*/
            }
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

            using (Stream certStream = assembly.GetManifestResourceStream("Manufacturing_Execution_Simulation.App_Data.PCo_Default_Untrusted_Certificate.pfx"))
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
        #endregion
    }
}