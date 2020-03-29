package sap.corp.emea.owls.Conf;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import sap.corp.emea.owls.OWLSTopLevelOntology;
import sap.corp.emea.utility.TupleWrapper;

public class OWLSGroundingConf {

	private static final String AtomicProcessGrounding = "AtomicProcessGrounding";
	private static final String OPCUANode = "OPCUANode";

	private static final String hasAtomicProcessGrounding = "hasAtomicProcessGrounding";
	private static final String hasOWLSProcess = "hasOWLSProcess";
	private static final String hasInput = "hasInput";
	private static final String hasOutput = "hasOutput";
	private static final String methodBrowseInfo = "methodBrowseInfo";
	private static final String hasNodeReference = "hasNodeReference";

	private static final String browseDirection = "browseDirection";
	private static final String endPointURL = "endPointURL";
	private static final String nodeClassMask = "nodeClassMask";
	private static final String nodeID = "nodeID";
	private static final String referenceType = "referenceType";
	private static final String browseResultMask = "browseResultMask";

	private static List<String> groundingClasses;
	private static List<String> groundingObjectProperties;
	private static List<String> groundingDataProperties;

	static HashMap<String, TupleWrapper.Tuple<String, String>> objectPropertyClassMapping;
	static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping;

	public static List<String> getGroundingClasses() {
		groundingClasses = Arrays.asList(AtomicProcessGrounding, OPCUANode);
		return groundingClasses;
	}

	public static List<String> getGroundingObjectProperties() {
		groundingObjectProperties = Arrays.asList(hasAtomicProcessGrounding, hasOWLSProcess, hasInput, hasOutput,
				/*methodBrowseInfo,*/ hasNodeReference);
		return groundingObjectProperties;
	}

	public static List<String> getGroundingDataProperties() {
		groundingDataProperties = Arrays.asList(browseDirection, endPointURL, nodeClassMask, nodeID, referenceType,
				browseResultMask);
		return groundingDataProperties;
	}

	public static HashMap<String, TupleWrapper.Tuple<String, String>> getObjectPropertyClassMapping() {
		objectPropertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, String>>();
		objectPropertyClassMapping.put(hasAtomicProcessGrounding,
				new TupleWrapper.Tuple<String, String>(OWLSOntologyConf.MethodGrounding, AtomicProcessGrounding));
		objectPropertyClassMapping.put(hasOWLSProcess,
				new TupleWrapper.Tuple<String, String>(AtomicProcessGrounding, OWLSOntologyConf.MethodProcessModel));
		objectPropertyClassMapping.put(hasInput,
				new TupleWrapper.Tuple<String, String>(AtomicProcessGrounding, OWLSProcessModelConf.Parameter));
		objectPropertyClassMapping.put(hasOutput,
				new TupleWrapper.Tuple<String, String>(AtomicProcessGrounding, OWLSProcessModelConf.Parameter));
		/*objectPropertyClassMapping.put(hasNodeReference,
				new TupleWrapper.Tuple<String, String>(AtomicProcessGrounding, OPCUANode));*/
		objectPropertyClassMapping.put(hasNodeReference,
				new TupleWrapper.Tuple<String, String>(OPCUANode, OPCUANode));
		return objectPropertyClassMapping;
	}

	public static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> getDataPropertyClassMapping() {
		dataPropertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, Class<?>>>();
		dataPropertyClassMapping.put(browseDirection,
				new TupleWrapper.Tuple<String, Class<?>>(OPCUANode, String.class));
		dataPropertyClassMapping.put(endPointURL,
				new TupleWrapper.Tuple<String, Class<?>>(AtomicProcessGrounding, null));
		dataPropertyClassMapping.put(nodeClassMask,
				new TupleWrapper.Tuple<String, Class<?>>(OPCUANode, String.class));
		dataPropertyClassMapping.put(nodeID,
				new TupleWrapper.Tuple<String, Class<?>>(OPCUANode, String.class));
		dataPropertyClassMapping.put(referenceType,
				new TupleWrapper.Tuple<String, Class<?>>(OPCUANode, String.class));
		dataPropertyClassMapping.put(browseResultMask,
				new TupleWrapper.Tuple<String, Class<?>>(OPCUANode, String.class));
		return dataPropertyClassMapping;
	}

	public void setGroundingClasses(List<String> groundingClasses) {
		this.groundingClasses = groundingClasses;
	}

	public void setGroundingObjectProperties(List<String> groundingObjectProperties) {
		this.groundingObjectProperties = groundingObjectProperties;
	}

	public void setGroundingDataProperties(List<String> groundingDataProperties) {
		this.groundingDataProperties = groundingDataProperties;
	}

	public static void setObjectPropertyClassMapping(
			HashMap<String, TupleWrapper.Tuple<String, String>> objectPropertyClassMapping) {
		OWLSGroundingConf.objectPropertyClassMapping = objectPropertyClassMapping;
	}

	public static void setDataPropertyClassMapping(
			HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping) {
		OWLSGroundingConf.dataPropertyClassMapping = dataPropertyClassMapping;
	}
}
