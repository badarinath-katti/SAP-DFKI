using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GSFCCache
{
    public class ShopOrderModel
    {
        public Guid SFC;
        public SFCState sFCState;
        public string SFCName;
        public string PlannedComponent;
        public string CustomerOrder;
        public string WorkCenter;
        public Dictionary<int, RoutingStep> RoutingSteps;
        public XmlDocument Routing;
        public string Product;

        public ShopOrderModel()
        {
            RoutingSteps = new Dictionary<int, RoutingStep>();
            Product = string.Empty;
        }
    }

    public class RoutingStep
    {
        public int StepID;
        public string Resource;
        public string Operation;
        public OperationState StepState;
        public List<Component> Components;
        public XElement StepElement;

        public RoutingStep()
        {
            Components = new List<Component>();
        }
    }

    public class Component
    {
        public int AssemblyQuantity;
        public string AssemblyComponent;
    }

    public enum SFCState
    {
        New = 0, InProcess = 1, Done = 2
    }

    public enum OperationState
    {
        New = 0,
        InProcess = 1,
        Done = 2
    }
}
