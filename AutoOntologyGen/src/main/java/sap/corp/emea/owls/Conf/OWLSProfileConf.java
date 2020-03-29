package sap.corp.emea.owls.Conf;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import sap.corp.emea.utility.TupleWrapper;

public class OWLSProfileConf {

	// Classes
	private static final String MethodCategory = "MethodCategory";

	// Object Properties
	// private static final String contactInformation = "contactInformation";
	private static final String has_ProcessModel = "has_ProcessModel";
	private static final String methodCategory = "methodCategory";

	// Data Properties
	private static final String category = "category";
	private static final String methodClassification = "methodClassification";
	private static final String methodName = "methodName";
	private static final String taxonomy = "taxonomy";
	private static final String textDescription = "textDescription";

	static List<String> profileClassNames;
	static List<String> objectPropertyNames;
	static List<String> dataPropertyNames;
	static HashMap<String, TupleWrapper.Tuple<String, String>> objectPropertyClassMapping;
	static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping;

	public static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> getDataPropertyClassMapping() {
		
		dataPropertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, Class<?>>>();
		dataPropertyClassMapping.put(category,
				new TupleWrapper.Tuple<String, Class<?>>(MethodCategory, null));
		dataPropertyClassMapping.put(methodClassification,
				new TupleWrapper.Tuple<String, Class<?>>(MethodCategory, String.class));
		dataPropertyClassMapping.put(methodName,
				new TupleWrapper.Tuple<String, Class<?>>(MethodCategory, String.class));
		dataPropertyClassMapping.put(taxonomy,
				new TupleWrapper.Tuple<String, Class<?>>(MethodCategory, String.class));
		dataPropertyClassMapping.put(textDescription,
				new TupleWrapper.Tuple<String, Class<?>>(MethodCategory, String.class));
		return dataPropertyClassMapping;
	}

	public static void setDataPropertyClassMapping(
			HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping) {
		OWLSProfileConf.dataPropertyClassMapping = dataPropertyClassMapping;
	}

	public static List<String> getProfileClassNames() {
		profileClassNames = Arrays.asList(MethodCategory);
		return profileClassNames;
	}

	public static List<String> getObjectPropertyNames() {
		objectPropertyNames = Arrays.asList(methodCategory);
		return objectPropertyNames;
	}

	public static List<String> getDataPropertyNames() {
		dataPropertyNames = Arrays.asList(category, methodClassification, methodName, taxonomy, textDescription);
		return dataPropertyNames;
	}

	public static HashMap<String, TupleWrapper.Tuple<String, String>> getObjectPropertyClassMapping() {
		objectPropertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, String>>();
		objectPropertyClassMapping.put(methodCategory,
				new TupleWrapper.Tuple<String, String>(OWLSOntologyConf.MethodProfile, MethodCategory));
		return objectPropertyClassMapping;
	}
	

	public static void setProfileClassNames(List<String> profileClassNames) {
		OWLSProfileConf.profileClassNames = profileClassNames;
	}

	public static void setObjectPropertyNames(List<String> objectPropertyNames) {
		OWLSProfileConf.objectPropertyNames = objectPropertyNames;
	}

	public static void setDataPropertyNames(List<String> dataPropertyNames) {
		OWLSProfileConf.dataPropertyNames = dataPropertyNames;
	}

	public static void setObjectPropertyClassMapping(
			HashMap<String, TupleWrapper.Tuple<String, String>> objectPropertyClassMapping) {
		OWLSProfileConf.objectPropertyClassMapping = objectPropertyClassMapping;
	}
}
