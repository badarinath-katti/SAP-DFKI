using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using SAP.Manufacturing.MethodProcessingFramework;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.ObjectModel;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Reflection;

namespace GSFCCache
{

    
    public class GSFCCache : ApplicationMethodsBase
    {
        private Dictionary<string, ShopOrderModel> dictGSFCCache = new Dictionary<string, ShopOrderModel>();
        private Dictionary<string, ShopOrderModel> FinishedShopOrders = new Dictionary<string, ShopOrderModel>();
        private DecentralizationFacilitator decentralizationFacilitator;
        private PerceptionLayer perceptionLayer;

        private DestinationCallback currentDestinationCallback;

        public GSFCCache()
        {
            empMetaData = createEMPExampleMetaData();
            decentralizationFacilitator = new DecentralizationFacilitator();
            perceptionLayer = new PerceptionLayer();
        }

        private EmpMetaData createEMPExampleMetaData()
        {
            EmpMetaData empMetaData = new EmpMetaData();

            empMetaData.Id = GlobalVariables.empTestNodeId;
            empMetaData.Description = "Test Implementation of EMP methods";
            empMetaData.Methods = new Dictionary<Guid, MethodMetaData>();

            #region GetSFCs
            //Method GetSFCs
            MethodMetaData GetSFCsMetaData = new MethodMetaData();
            GetSFCsMetaData.Id = GlobalVariables.GetSFCsMethodID;
            GetSFCsMetaData.Name = "GetSFCs";
            GetSFCsMetaData.Description = "Gets SFCs from CMES";
            GetSFCsMetaData.RequiresDestinationSystem = true;

            GetSFCsMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetSFCsMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            GetSFCsMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            GetSFCsMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            GetSFCsMetaData.InputParameters.Add("inSFCCount", new Tuple<string, Type>("SFC(s) count", Type.GetType("System.Int32")));

            //Output Parameters
            GetSFCsMetaData.OutputParameters.Add("outSFCs", new Tuple<string, Type>("SFCs from ME", Type.GetType("System.String")));
            GetSFCsMetaData.OutputParameters.Add("outStatus", new Tuple<string, Type>("Set in EMP based on the result of ME call", Type.GetType("System.Boolean")));

            //EMP Input Variables (= input variables mapped against the web service input parameters)
            GetSFCsMetaData.InputEmpVariables.Add("inEMPinSFCCount", new Tuple<string, Type>("SFC(s) count", Type.GetType("System.Int32")));

            //EMP Output Variables (= output variables mapped against the web service response)
            GetSFCsMetaData.OutputEmpVariables.Add("outEMPSFCs", new Tuple<string, Type>("SFCs from ME", Type.GetType("System.String")));
            GetSFCsMetaData.OutputEmpVariables.Add("outEMPStatus", new Tuple<string, Type>("Set in EMP based on the result of ME call", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.GetSFCsMethodID, GetSFCsMetaData);
            #endregion

            #region RegisterResource
            //Method MachineRegister
            MethodMetaData RegisterResourceMetaData = new MethodMetaData();
            RegisterResourceMetaData.Id = GlobalVariables.RegisterResourceMethodID;
            RegisterResourceMetaData.Name = "RegisterResource";
            RegisterResourceMetaData.Description = "Register a resource";
            RegisterResourceMetaData.RequiresDestinationSystem = false;

            RegisterResourceMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            RegisterResourceMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            RegisterResourceMetaData.InputParameters.Add("ResourceUrl", new Tuple<string, Type>("Resource Url", Type.GetType("System.String")));

            //Output Parameters
            RegisterResourceMetaData.OutputParameters.Add("outResult", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.RegisterResourceMethodID, RegisterResourceMetaData);
            #endregion

            #region StateAvailable
            //Method StateAvailable
            MethodMetaData StateAvailableMetaData = new MethodMetaData();
            StateAvailableMetaData.Id = GlobalVariables.StateAvailableMethodID;
            StateAvailableMetaData.Name = "StateAvailable";
            StateAvailableMetaData.Description = "Resource informs PCo that it is free";
            StateAvailableMetaData.RequiresDestinationSystem = false;

            StateAvailableMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            StateAvailableMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            StateAvailableMetaData.InputParameters.Add("ResourceUrl", new Tuple<string, Type>("Resource Url", Type.GetType("System.String")));

            //Output Parameters
            StateAvailableMetaData.OutputParameters.Add("outResult", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.StateAvailableMethodID, StateAvailableMetaData);
            #endregion

            #region CallbackDelegateWork
            //Method CallbackDelegateWork
            MethodMetaData CallbackDelegateWorkMetaData = new MethodMetaData();
            CallbackDelegateWorkMetaData.Id = GlobalVariables.CallbackDelegateWorkMethodID;
            CallbackDelegateWorkMetaData.Name = "CallbackDelegateWork";
            CallbackDelegateWorkMetaData.Description = "Resource informs finished task to GSFC";
            CallbackDelegateWorkMetaData.RequiresDestinationSystem = false;

            CallbackDelegateWorkMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            CallbackDelegateWorkMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            CallbackDelegateWorkMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            CallbackDelegateWorkMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            CallbackDelegateWorkMetaData.InputParameters.Add("ResourceUrl", new Tuple<string, Type>("Resource Url", Type.GetType("System.String")));
            CallbackDelegateWorkMetaData.InputParameters.Add("WorkElement", new Tuple<string, Type>("WorkElement element", Type.GetType("System.String")));
            CallbackDelegateWorkMetaData.InputParameters.Add("Product", new Tuple<string, Type>("Product string", Type.GetType("System.String")));

            //Output Parameters
            CallbackDelegateWorkMetaData.OutputParameters.Add("outStatus", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.CallbackDelegateWorkMethodID, CallbackDelegateWorkMetaData);
            #endregion

            #region StateOnHold
            //Method StateOnHold
            MethodMetaData StateOnHoldMetaData = new MethodMetaData();
            StateOnHoldMetaData.Id = GlobalVariables.StateOnHoldMethodID;
            StateOnHoldMetaData.Name = "StateOnHold";
            StateOnHoldMetaData.Description = "Resource informs PCo that it is OnHold";
            StateOnHoldMetaData.RequiresDestinationSystem = false;

            StateOnHoldMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            StateOnHoldMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            StateOnHoldMetaData.InputParameters.Add("ResourceUrl", new Tuple<string, Type>("Resource Url", Type.GetType("System.String")));

            //Output Parameters
            StateOnHoldMetaData.OutputParameters.Add("outResult", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.StateOnHoldMethodID, StateOnHoldMetaData);
            #endregion

            #region SetSFCToComplete
            //Method SetSFCToComplete
            MethodMetaData SetSFCToCompleteMetaData = new MethodMetaData();
            SetSFCToCompleteMetaData.Id = GlobalVariables.SetSFCToCompleteMethodID;
            SetSFCToCompleteMetaData.Name = "SetSFCToComplete";
            SetSFCToCompleteMetaData.Description = "Sets an SFC to complete";
            SetSFCToCompleteMetaData.RequiresDestinationSystem = true;

            SetSFCToCompleteMetaData.InputParameters = new Dictionary<string, Tuple<string, Type>>();
            SetSFCToCompleteMetaData.OutputParameters = new Dictionary<string, Tuple<string, Type>>();
            SetSFCToCompleteMetaData.InputEmpVariables = new Dictionary<string, Tuple<string, Type>>();
            SetSFCToCompleteMetaData.OutputEmpVariables = new Dictionary<string, Tuple<string, Type>>();

            //Input Parameters
            SetSFCToCompleteMetaData.InputParameters.Add("inSFC", new Tuple<string, Type>("SFC number", Type.GetType("System.String")));
            SetSFCToCompleteMetaData.InputEmpVariables.Add("inEMPSFC", new Tuple<string, Type>("SFC number", Type.GetType("System.String")));

            //Output Parameters
            SetSFCToCompleteMetaData.OutputParameters.Add("outResult", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));
            SetSFCToCompleteMetaData.OutputEmpVariables.Add("outEMPResult", new Tuple<string, Type>("Return Value", Type.GetType("System.Boolean")));

            empMetaData.Methods.Add(GlobalVariables.SetSFCToCompleteMethodID, SetSFCToCompleteMetaData);
#endregion


            return empMetaData;
        }

        protected override void executeCustom(Guid methodId, DestinationCallback destinationCallback, 
            Dictionary<string, object> inputVariables, out Tuple<Dictionary<string, object>, object> outputVariables)
        {
            currentDestinationCallback = destinationCallback;

            base.callEMPMethod(methodId, inputVariables, out outputVariables);
        }

        protected override EmpMetaData getMethodDefinitionsCustom()
        {
            return empMetaData;
        }

        #region Private_Methods
        private void CallbackDecentralizationFacilitator(ResourceDetails resourceDetails)
        {
            decentralizationFacilitator.AddResource(resourceDetails);
            return;
        }

        public object GetSFCsCaller(Tuple<int, object> SFCCount_ProxyClient)
        {
            object objct = null;
            try
            {
                objct = SFCCount_ProxyClient.Item2.GetType().
                    GetMethod("GSFCCache_GetSFCs").
                    Invoke(SFCCount_ProxyClient.Item2, new object[] { SFCCount_ProxyClient.Item1 });
            }
            catch (Exception ex)
            {

            }
            return objct;
        }

        private Boolean ManageGSFCCache()
        {
            int InProcessSFCs = this.dictGSFCCache.Values.Where(order => order.sFCState == SFCState.InProcess).Count();

            while (InProcessSFCs <= GlobalVariables.GSFCCacheProcessingSFCSize)
            {
                if (this.dictGSFCCache.Values.Where(order => order.sFCState == SFCState.New).Count()
                    < GlobalVariables.GSFCCacheNewSFCsSizeMinThreshold)
                {
                    //TBD : Make this call generic by passsing parameter types
                    //ThreadedExecuter<Tuple<int, object>, object> GetSFCsExecutor = 
                    //    new ThreadedExecuter<Tuple<int, object>, object>
                    //    (GetSFCsCaller, null, true);
                    //GetSFCsExecutor.Start( new Tuple<int, object>(
                    //    ( GlobalVariables.GSFCCacheNewSFCsSize - dictGSFCCache.Count), 
                    //    this.perceptionLayer.CreateDynamicClientProxy(GlobalVariables.GSFCUrl)));

                    object GSFCClientProxy = this.perceptionLayer.CreateDynamicClientProxy(GlobalVariables.GSFCUrl);
                    object result = GSFCClientProxy.GetType().
                    GetMethod("GSFCCache_GetSFCs").
                    Invoke(GSFCClientProxy, new object[] { GlobalVariables.GSFCCacheNewSFCsSize - dictGSFCCache.Count });

                    PropertyInfo propertyInfo = result.GetType().GetProperty("outStatus");
                    if (Convert.ToBoolean(propertyInfo.GetValue(result, null))
                        && this.dictGSFCCache.Values.Where(order => order.sFCState == SFCState.New).Count() == 0)
                        return false;

                    //Tuple<Dictionary<string, object>, object> result = GetSFCs(GlobalVariables.GSFCCacheNewSFCsSize - dictGSFCCache.Count);

                    //if ((Convert.ToBoolean(result.Item1["outStatus"]) == false)
                    //    && this.dictGSFCCache.Values.Where(order => order.sFCState == SFCState.New).Count() == 0)
                    //    return false;
                }
                if (StartSFC())
                    InProcessSFCs++;
                else
                    break;
            }

            if (this.dictGSFCCache.Values.Where(order => order.sFCState == SFCState.New).Count()
                < GlobalVariables.GSFCCacheNewSFCsSizeMinThreshold)
            {
                //TBD : Make this call generic by passsing parameter types
                ThreadedExecuter<Tuple<int, object>, object> GetSFCsExecutor =
                    new ThreadedExecuter<Tuple<int, object>, object>
                    (GetSFCsCaller, null, true);
                GetSFCsExecutor.Start(new Tuple<int, object>(
                    (GlobalVariables.GSFCCacheNewSFCsSize - dictGSFCCache.Count),
                    this.perceptionLayer.CreateDynamicClientProxy(GlobalVariables.GSFCUrl)));
            }
            return true;
        }

        private Tuple<XElement, String> DelegateWork(Tuple<XElement, String> RoutingStepElement_Product)
        {
            string Capability = RoutingStepElement_Product.Item1.Attribute("Operation").Value;
            int RoutingStepID = Convert.ToInt32(RoutingStepElement_Product.Item1.Attribute("ID").Value);
            string ShopOrder = RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value;

            foreach (string ResourceUrl in this.decentralizationFacilitator.GetRegisteredResourceUrls(Capability))
            {
                this.decentralizationFacilitator.AddResource(this.perceptionLayer.BrowseMachine(ResourceUrl));
            }
            ResourceDetails resourceDetails = this.decentralizationFacilitator.ResourceDetermination(Capability);
            this.perceptionLayer.AssignWorkToResource(resourceDetails, RoutingStepElement_Product.Item1, this.dictGSFCCache[ShopOrder].Product);

            dictGSFCCache[RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value].RoutingSteps[RoutingStepID].StepState = OperationState.InProcess;
            dictGSFCCache[RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value].RoutingSteps[RoutingStepID].Resource = resourceDetails.Name;

            this.dictGSFCCache[RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value].Routing.
                SelectSingleNode("SFC/RoutingStep[@ID=" + RoutingStepID + "]").Attributes["Resource"].Value = resourceDetails.ResourceUrl;
            this.dictGSFCCache[RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value].Routing.
                SelectSingleNode("SFC/RoutingStep[@ID=" + RoutingStepID + "]").Attributes["Status"].Value = OperationState.InProcess.ToString();            

            return null;
        }

        public Tuple<Dictionary<string, object>, object> CallbackDelegateWork(string ResourceUrl, string RoutingStepElement, String Product)
        {
            AssignNextOperation(ResourceUrl, new Tuple<XElement, string>(XElement.Parse(RoutingStepElement), Product));

            Dictionary<string, object> Result = new Dictionary<string, object>();
            Result.Add(this.empMetaData.Methods[GlobalVariables.RegisterResourceMethodID].OutputParameters.ElementAt(0).Key, true);
            return new Tuple<Dictionary<string, object>, object>(Result, 202);
        }

        private void AssignNextOperation(string ResourceUrl, Tuple<XElement, String> RoutingStepElement_Product)
        {
            string ShopOrder = RoutingStepElement_Product.Item1.Attribute("ShopOrder").Value;
            int RoutingStepID = Convert.ToInt32(RoutingStepElement_Product.Item1.Attribute("ID").Value);
            try
            {
                if (RoutingStepElement_Product.Item1.Attribute("Status").Value.Equals(OperationState.Done.ToString("G")))
                {
                    this.dictGSFCCache[ShopOrder].Routing.SelectSingleNode("SFC/RoutingStep[@ID=" + RoutingStepID + "]").Attributes["Status"].Value
                        = OperationState.Done.ToString();
                    this.dictGSFCCache[ShopOrder].RoutingSteps[RoutingStepID].StepState = OperationState.Done;
                    this.dictGSFCCache[ShopOrder].Product = RoutingStepElement_Product.Item2;
                    try
                    {
                        XElement NextRoutingStep = XElement.Load(this.dictGSFCCache[ShopOrder].
                        Routing.SelectSingleNode("SFC/RoutingStep[@ID=" + ++RoutingStepID + "]")
                        .CreateNavigator().ReadSubtree());
                        DelegateWork(new Tuple<XElement, string>(NextRoutingStep, RoutingStepElement_Product.Item2));
                    }
                    //All routing steps are executed
                    catch
                    {
                        this.dictGSFCCache[ShopOrder].Routing["Status"].Value = SFCState.Done.ToString();
                        this.dictGSFCCache[ShopOrder].sFCState = SFCState.Done;
                        this.FinishedShopOrders.Add(ShopOrder, this.dictGSFCCache[ShopOrder]);
                        this.dictGSFCCache.Remove(ShopOrder);
                        SetSFCToComplete(ShopOrder);
                        ManageGSFCCache();
                    }
                }
                //Retry with other resources
                else
                {
                    DelegateWork(RoutingStepElement_Product);
                }
            }
            catch (Exception exception)
            { }
        }

        private Boolean StartSFC()
        {
            KeyValuePair<string, ShopOrderModel> shopOrder = dictGSFCCache.Where(order => order.Value.sFCState == SFCState.New).FirstOrDefault();

            if (shopOrder.Equals(default(KeyValuePair<string, ShopOrderModel>)))
                return false;
            //Assumption is there will be no routing steps with status other than "New"
            int RoutingStepID = Int32.MaxValue;
            XElement RoutingStep = null;
            XmlNodeList RoutingSteps = shopOrder.Value.Routing.SelectNodes("/SFC/RoutingStep");
            foreach (XmlNode RoutingStepNode in RoutingSteps)
            {
                if (RoutingStepID > Convert.ToInt32(RoutingStepNode.Attributes["ID"].Value))
                {
                    RoutingStepID = Convert.ToInt32(RoutingStepNode.Attributes["ID"].Value);
                    RoutingStep = XElement.Parse(RoutingStepNode.OuterXml);
                }
            }
            ThreadedExecuter<Tuple<XElement, String>, Tuple<XElement, string>> threadedExecuter =
                new ThreadedExecuter<Tuple<XElement, string>, Tuple<XElement, string>>(DelegateWork, null);
            threadedExecuter.Start(new Tuple<XElement, string>(RoutingStep, shopOrder.Value.Product));
            shopOrder.Value.sFCState = SFCState.InProcess;
            shopOrder.Value.Routing.DocumentElement.Attributes["Status"].Value = OperationState.InProcess.ToString();
            //ManageGSFCCache();
            return true;
        }
        #endregion

        public Tuple<Dictionary<string, object>, object> GetSFCs(int SFCCount)
        {
            Dictionary<string, object> destinationInput = new Dictionary<string, object>();
            destinationInput.Add(this.empMetaData.Methods[GlobalVariables.GetSFCsMethodID].InputEmpVariables.ElementAt(0).Key, SFCCount);

            Dictionary<string, object> result = new Dictionary<string, object>();

            Dictionary<string, object> destinationResult = currentDestinationCallback.callDestination(destinationInput, false);
            result.Add(this.empMetaData.Methods[GlobalVariables.GetSFCsMethodID].OutputParameters.ElementAt(0).Key, destinationResult["outEMPSFCs"]);

            if (destinationResult["outEMPSFCs"] != null)
            {
                result.Add(this.empMetaData.Methods[GlobalVariables.GetSFCsMethodID].OutputParameters.ElementAt(1).Key, true);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(destinationResult["outEMPSFCs"].ToString());
                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("SFCList");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    foreach (XmlNode sfcNode in xmlNode.ChildNodes)
                    {
                        string sfcNumber = sfcNode.Attributes["ID"].Value;

                        ShopOrderModel shopOrderModel = new ShopOrderModel();

                        shopOrderModel.Routing = new XmlDocument();
                        shopOrderModel.Routing.LoadXml(sfcNode.OuterXml);
                        List<RoutingStep> routingSteps = new List<RoutingStep>();
                        shopOrderModel.SFC = Guid.Parse(sfcNode.Attributes["ID"].Value);
                        shopOrderModel.WorkCenter = sfcNode.Attributes["WorkCenter"].Value;
                        shopOrderModel.sFCState = (SFCState)Enum.Parse(typeof(SFCState), sfcNode.Attributes["Status"].Value);

                        shopOrderModel.SFCName = sfcNode["SFCName"].Value;
                        shopOrderModel.PlannedComponent = sfcNode["Material"].Value;
                        shopOrderModel.CustomerOrder = sfcNode["OrderNumber"].Value;

                        for (int RoutingStepCounter = 0; RoutingStepCounter < sfcNode.SelectNodes("RoutingStep").Count; RoutingStepCounter++)
                        {
                            RoutingStep routingStep = new RoutingStep();
                            routingStep.StepID = Convert.ToInt32(sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].Attributes["ID"].Value);
                            routingStep.StepState = (OperationState)Enum.Parse(typeof(OperationState), sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].Attributes["Status"].Value);
                            routingStep.Resource = sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].Attributes["Resource"].Value;
                            routingStep.Operation = sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].Attributes["Operation"].Value;

                            routingStep.Components = new List<Component>();
                            XmlNodeList ComponentNodeList = sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].SelectNodes("Components/Component");

                            foreach (XmlNode ComponentNode in ComponentNodeList)
                            {
                                Component component = new Component();
                                component.AssemblyComponent = ComponentNode.Attributes["Name"].Value;
                                component.AssemblyQuantity = Convert.ToInt32(ComponentNode.Attributes["Assembly_Quantity"].Value);
                                routingStep.Components.Add(component);
                            }

                            routingStep.StepElement = XElement.Parse(sfcNode.SelectNodes("RoutingStep")[RoutingStepCounter].OuterXml);
                            shopOrderModel.RoutingSteps.Add(routingStep.StepID, routingStep);
                        }

                        dictGSFCCache.Add(sfcNumber, shopOrderModel);
                    }
                }
            }
            else
            {
                result.Add(this.empMetaData.Methods[GlobalVariables.GetSFCsMethodID].OutputParameters.ElementAt(1).Key, false);
            }
            return new Tuple<Dictionary<string, object>, object>(result, 202);
        }

        public Tuple<Dictionary<string, object>, object> RegisterResource(string ResourecUrl)
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();
            Result.Add(this.empMetaData.Methods[GlobalVariables.RegisterResourceMethodID].OutputParameters.ElementAt(0).Key, true);

            //ThreadedExecuter<string, ResourceDetails> BrowseMachineExecutor = new ThreadedExecuter<string, ResourceDetails>
            //    (perceptionLayer.BrowseMachine, CallbackDecentralizationFacilitator, true);
            //BrowseMachineExecutor.Start(ResourecUrl);

            this.decentralizationFacilitator.AddResource(this.perceptionLayer.BrowseMachine(ResourecUrl));

            ManageGSFCCache();

            return new Tuple<Dictionary<string, object>, object>(Result, 202);
        }

        public Tuple<Dictionary<string, object>, object> StateAvailable(string ResourceUrl)
        {
            this.decentralizationFacilitator.SetResourceDetails(ResourceUrl, ResourceState.Idle);
            ManageGSFCCache();
            
                 
            Dictionary<string, object> Result = new Dictionary<string, object>();
            Result.Add(this.empMetaData.Methods[GlobalVariables.StateAvailableMethodID].OutputParameters.ElementAt(0).Key, true);
            return new Tuple<Dictionary<string, object>, object>(Result, 202);
        }

        public Tuple<Dictionary<string, object>, object> StateOnHold(string ResourceUrl)
        {
            this.decentralizationFacilitator.SetResourceDetails(ResourceUrl, ResourceState.OnHold);

            Dictionary<string, object> Result = new Dictionary<string, object>();
            Result.Add(this.empMetaData.Methods[GlobalVariables.StateOnHoldMethodID].OutputParameters.ElementAt(0).Key, true);
            return new Tuple<Dictionary<string, object>, object>(Result, 202);
        }

        public Tuple<Dictionary<string, object>, object> SetSFCToComplete(string SFC)
        {
            Dictionary<string, object> destinationInput = new Dictionary<string, object>();
            destinationInput.Add(this.empMetaData.Methods[GlobalVariables.SetSFCToCompleteMethodID].InputEmpVariables.ElementAt(0).Key, SFC);

            Dictionary<string, object> result = new Dictionary<string, object>();

            Dictionary<string, object> destinationResult = currentDestinationCallback.callDestination(destinationInput, false);
            result.Add(this.empMetaData.Methods[GlobalVariables.GetSFCsMethodID].OutputParameters.ElementAt(0).Key, destinationResult["outEMPSFCs"]);
            
            return new Tuple<Dictionary<string, object>, object>(result, 202);
        }
    }            
}
