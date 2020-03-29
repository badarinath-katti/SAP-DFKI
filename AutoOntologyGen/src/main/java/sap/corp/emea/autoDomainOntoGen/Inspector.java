package sap.corp.emea.autoDomainOntoGen;

import java.util.HashSet;
import java.util.Set;

import org.semanticweb.owlapi.vocab.OWL2Datatype;

public class Inspector {

	static String DOCUMENT_IRI = "http://emea.global.corp.sap/rescom";

	public static Boolean IsBuiltIn(Class<?> classType) {
		if (classType.isPrimitive() && classType != void.class)
			return true;
		else
			return getWrapperTypes(classType);
	}

	private static Boolean getWrapperTypes(Class<?> classType) {

		switch (classType.getTypeName()) {
		case "Boolean.class":
		case "Character.class":
		case "Byte.class":
		case "Short.class":
		case "Integer.class":
		case "Long.class":
		case "Float.class":
		case "Double.class":
		case "Number.class":
		case "java.lang.String":
			return true;
		default:
			return false;
		}

	}

	public static OWL2Datatype getOWLDatatype(Class<?> classType) {
		try {
			switch (classType.getTypeName()) {
			case "java.lang.Integer":
			case "int":
				return OWL2Datatype.XSD_INTEGER;
			case "java.lang.Boolean":
			case "Boolean":
			case "boolean":
				return OWL2Datatype.XSD_BOOLEAN;
			case "java.lang.Character":
			case "char":
				return OWL2Datatype.XSD_BYTE;
			case "java.lang.Byte":
			case "Byte":
			case "byte":
				return OWL2Datatype.XSD_BYTE;
			case "java.long.Short":
			case "Short":
			case "short":
				return OWL2Datatype.XSD_SHORT;
			case "java.lang.Long":
			case "Long":
			case "long":
				return OWL2Datatype.XSD_LONG;
			case "java.lang.Float":
			case "Float":
			case "float":
				return OWL2Datatype.XSD_FLOAT;
			case "java.lang.Double":
			case "Double":
			case "double":
				return OWL2Datatype.XSD_DOUBLE;
			case "java.lang.Number":
			case "Number":
				return OWL2Datatype.XSD_POSITIVE_INTEGER;
			case "java.lang.String":
			case "String":
			case "string":
				return OWL2Datatype.XSD_STRING;
			default:
				return OWL2Datatype.XSD_ANY_URI;
			}
		} catch (Exception ex) {
			return OWL2Datatype.XSD_ANY_URI;
		}
	}
}
