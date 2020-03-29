package sap.corp.emea.autoDomainOntoGen.reasoner;

public class OpcuaNodeLink {
	
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