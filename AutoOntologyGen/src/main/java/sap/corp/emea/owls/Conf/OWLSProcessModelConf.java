package sap.corp.emea.owls.Conf;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import sap.corp.emea.owls.Conf.OWLSOntologyConf;
import sap.corp.emea.utility.TupleWrapper;
import sap.corp.emea.utility.TupleWrapper.Tuple;

public class OWLSProcessModelConf {

	private static final String AtomicProcess = "AtomicProcess";
	private static final String SimpleProcess = "SimpleProcess";
	private static final String CompositeProcess = "CompositeProcess";
	
	private static final String Variable = "Variable";
	public static final String Parameter = "Parameter";
	private static final String Input = "Input";
	private static final String Output = "Output";
	private static final String PreCondition = "PreCondition";
	private static final String PostCondition = "PostCondition";
	
	private static final String Status = "Status";
	private static final String Err = "Err";
	private static final String NoErr = "NoErr";
	
	private static final List<String> Parameters = Arrays.asList(Input,Output,PreCondition,PostCondition);
	private static final List<String> StatusList = Arrays.asList(Err,NoErr);
	
	private static final String manufacturingService = "manufacturingService";
	private static final String hasInput = "hasInput";
	private static final String hasOutput = "hasOutput";
	private static final String hasprecondition = "hasPreCondition";
	private static final String haspostcondition = "hasPostCondition";
	private static final String realizedBy = "realizedBy";
	private static final String realizes = "realizes";	
	
	private static final String executionTime = "executionTime";
	private static final String manufacturingCost = "ManufacturingCost";
	private static final String argumentType = "argumentType";
	private static final String condition = "condition";
	
	private static HashMap<String, List<String>> processSubClassMapping;
	private static HashMap<String, HashMap<String, List<String>>> variableSubClassMapping;	
	private static HashMap<String, Tuple<String, String>> objectPropertyClassMapping;
	static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping;
	
	

	public static HashMap<String, TupleWrapper.Tuple<String, Class<?>>> getDataPropertyClassMapping() {
		dataPropertyClassMapping = new HashMap<String, TupleWrapper.Tuple<String, Class<?>>>();
		
		dataPropertyClassMapping.put(executionTime,
				new TupleWrapper.Tuple<String, Class<?>>(OWLSOntologyConf.MethodProcessModel, Integer.class));
		dataPropertyClassMapping.put(manufacturingCost,
				new TupleWrapper.Tuple<String, Class<?>>(OWLSOntologyConf.MethodProcessModel, Double.class));
		dataPropertyClassMapping.put(argumentType,
				new TupleWrapper.Tuple<String, Class<?>>(Parameter, null));
		dataPropertyClassMapping.put(condition,
				new TupleWrapper.Tuple<String, Class<?>>(Parameter, String.class));
		return dataPropertyClassMapping;
	}

	public static void setDataPropertyClassMapping(
			HashMap<String, TupleWrapper.Tuple<String, Class<?>>> dataPropertyClassMapping) {
		OWLSProcessModelConf.dataPropertyClassMapping = dataPropertyClassMapping;
	}

	public static HashMap<String, Tuple<String, String>> getObjectPropertyClassMapping() {
		objectPropertyClassMapping = new HashMap<String, Tuple<String, String>>();
		objectPropertyClassMapping.put(manufacturingService, new Tuple<String, String>(AtomicProcess, null));
		objectPropertyClassMapping.put(hasInput, new Tuple<String, String>(AtomicProcess, Input));
		objectPropertyClassMapping.put(hasOutput, new Tuple<String, String>(AtomicProcess, Output));
		objectPropertyClassMapping.put(hasprecondition, new Tuple<String, String>(AtomicProcess, PreCondition));
		objectPropertyClassMapping.put(haspostcondition, new Tuple<String, String>(AtomicProcess, PostCondition));
		
		objectPropertyClassMapping.put(realizedBy, new Tuple<String, String>(SimpleProcess, AtomicProcess));
		
		return objectPropertyClassMapping;
	}

	public static void setObjectPropertyClassMapping(HashMap<String, Tuple<String, String>> objectPropertyClassMapping) {
		OWLSProcessModelConf.objectPropertyClassMapping = objectPropertyClassMapping;
	}

	public static HashMap<String, HashMap<String, List<String>>> getVariableSubClassMapping() {
		variableSubClassMapping = new HashMap<String, HashMap<String, List<String>>>();
		HashMap<String, List<String>> var = new HashMap<String, List<String>>();
		var.put(Parameter, Parameters);
		var.put(Status, StatusList);		
		variableSubClassMapping.put(Variable,var);
		return variableSubClassMapping;
	}

	public static void setVariableSubClassMapping(HashMap<String, HashMap<String, List<String>>> variableSubClassMapping) {
		OWLSProcessModelConf.variableSubClassMapping = variableSubClassMapping;
	}

	public static HashMap<String, List<String>> getprocessSubClassMapping() {
		processSubClassMapping = new HashMap<String, List<String>>();
		processSubClassMapping.put(OWLSOntologyConf.MethodProcessModel,
				Arrays.asList(AtomicProcess, SimpleProcess, CompositeProcess));
		return processSubClassMapping;
	}

	public void setClassSubClassMapping(HashMap<String, List<String>> processSubClassMapping) {
		OWLSProcessModelConf.processSubClassMapping = processSubClassMapping;
	}

}
