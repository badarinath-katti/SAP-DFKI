package sap.corp.emea.owls.Conf;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import sap.corp.emea.utility.TupleWrapper;

public final class OWLSOntologyConf {

	public static final String OPCUAMethod = "OPCUAMethod";
	public static final String MethodProfile = "MethodProfile";
	public static final String MethodProcessModel = "MethodProcessModel";
	public static final String MethodGrounding = "MethodGrounding";

	public static final String presents = "presents";
	public static final String describedBy = "describedBy";
	public static final String supports = "supports";

	public static final String presentedBy = "presentedBy";
	public static final String describes = "describes";
	public static final String supportedBy = "supportedBy";

	static HashMap<String, TupleWrapper.Tuple<String, String>> propertyClassMapping;
	static List<TupleWrapper.Tuple<String, String>> inversePropertyMapping;
	static List<String> TopOntologyClassNames;
	static List<String> TopOntologyPropertyNames;
	static List<String> TopinversePropertyNames;
	
	public OWLSOntologyConf(){
		
		
		
	}
	
	public static List<String> getTopOntologyClassNames() {
		TopOntologyClassNames = Arrays.asList(OPCUAMethod, MethodProfile, MethodProcessModel, MethodGrounding);
		return TopOntologyClassNames;
	}

	public static void setTopOntologyClassNames(List<String> topOntologyClassNames) {
		TopOntologyClassNames = topOntologyClassNames;
	}

	public static List<String> getTopOntologyPropertyNames() {
		TopOntologyPropertyNames = Arrays.asList(presents, describedBy, supports);
		return TopOntologyPropertyNames;
	}

	public static void setTopOntologyPropertyNames(List<String> topOntologyPropertyNames) {
		TopOntologyPropertyNames = topOntologyPropertyNames;
	}

	public static List<String> getTopinversePropertyNames() {
		TopinversePropertyNames = Arrays.asList(presentedBy, describes, supportedBy);
		return TopinversePropertyNames;
	}

	public static void setTopinversePropertyNames(List<String> topinversePropertyNames) {
		TopinversePropertyNames = topinversePropertyNames;
	}

	public static HashMap<String, TupleWrapper.Tuple<String, String>> getPropertyClassMapping(){
		propertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, String>>();
		propertyClassMapping.put(presents, new TupleWrapper.Tuple<String, String>(OPCUAMethod, MethodProfile));
		propertyClassMapping.put(describedBy, new TupleWrapper.Tuple<String, String>(OPCUAMethod, MethodProcessModel));
		propertyClassMapping.put(supports, new TupleWrapper.Tuple<String, String>(OPCUAMethod, MethodGrounding));
		return propertyClassMapping;
	}
	
	public static List<TupleWrapper.Tuple<String, String>> getinversePropertyMapping(){
		inversePropertyMapping = new ArrayList<TupleWrapper.Tuple<String, String>>();
		inversePropertyMapping.add(new TupleWrapper.Tuple<String, String>(presents, presentedBy));
		inversePropertyMapping.add(new TupleWrapper.Tuple<String, String>(describedBy, describes));
		inversePropertyMapping.add(new TupleWrapper.Tuple<String, String>(supports, supportedBy));
		return inversePropertyMapping;
	}
	
}
