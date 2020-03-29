using Opc.Ua;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Application_Utility
{
    public class OpcuaNodeList
    {
        [Key]
        public String opcuaEndpointURL { get; set; }
        public String SelectedMethodNodeId { get; set; }
        public String SelectedMethod { get; set; }

        public String Resource { get; set; }
        public List<OpcuaNodeLink> ChildrenNodes { get; set; }

        public List<OpcuaNodeLink> MethodBrowseLinks { get; set; }

        public List<OntologyConcept> ontologyConcepts { get; set; }

        
        //===========================================================

        public String ManServInstance { get; set; }
        public String ManServAxiom { get; set; }
        public String ManServConcept { get; set; }

        //===========================================================
        public String ManServClassificationIndv { get; set; }
        public String ManServClassificationAxiom { get; set; }
        public String ManServClassificationConcept { get; set; }

        //===========================================================
        public String ManServCategoryIndv { get; set; }
        public String ManServCategoryAxiom { get; set; }
        public String ManServCategoryConcept { get; set; }
        //===========================================================
        public String ManServPreCondition { get; set; }
        public String ManServPostCondition { get; set; }
        //===========================================================

        public List<String> ManServInputs { get; set; }
        public List<String> ManServOutputs { get; set; }
        //===========================================================



        //public Boolean IsChildPresent(String searchNodeId)
        //{
        //    foreach (KeyValuePair<OpcuaNodeStructure, OpcuaNodeLink> keyValuePair in ChildrenNodes)
        //    {
        //        if (searchNodeId.Equals(keyValuePair.Key.nodeId))
        //            return true;
        //        else if (keyValuePair.Key.ChildrenNodes.Count > 0)
        //            IsChildPresent(searchNodeId);
        //        else
        //            return false;
        //    }
        //    return false;
        //}
    }

    public class OpcuaNodeLink
    {
        public String nodeID;
        public String browseName;

        public String parentNodebrowseName;
        public String parentNodeId;

        public String referenceType;
        public String BrowseDirection;
        public String NodeClassMask;
        public String browseResultMask;

        public Boolean isMethod;
        public Boolean isArgument;
    }

    public class OntologyConcept
    {
        public int Id { get; set; }
        public String IRI { get; set; }
        public String Short_Name { get; set; }
    }
}