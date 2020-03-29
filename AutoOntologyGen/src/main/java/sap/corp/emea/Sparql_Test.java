package sap.corp.emea;

import java.io.InputStream;

import org.apache.jena.ontology.OntModel;
import org.apache.jena.query.Query;
import org.apache.jena.query.QueryExecution;
import org.apache.jena.query.QueryExecutionFactory;
import org.apache.jena.query.QueryFactory;
import org.apache.jena.query.QuerySolution;
import org.apache.jena.query.ResultSet;
import org.apache.jena.rdf.model.Model;
import org.apache.jena.rdf.model.ModelFactory;
import org.apache.jena.util.FileManager;

public class Sparql_Test {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
	    Model model = ModelFactory.createDefaultModel();
	    InputStream in = FileManager.get().open("sp.owl");
	    model.read( in, "RDF/XML");
	    
	    OntModel ontModel = ModelFactory.createOntologyModel();
	    in = FileManager.get().open("sp.owl");
	    in = FileManager.get().open("RescomInformationModel.owl");
	    ontModel.read( in, "RDF/XML");
	    
	    String QueryString1 = "PREFIX : <http://emea.global.corp.sap/rescom#> PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> "+
	    		"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>"+
	    		"SELECT ?PressInstance 		((abs(?Press_XCoOrdValue - ?LowerShellWarehouse_XCoOrdValue))"+
        "+(abs(?Press_XCoOrdValue - ?UpperShellWarehouse_XCoOrdValue)) +(abs(?Press_XCoOrdValue - ?CircuitBoardWarehouse_XCoOrdValue))"+
        "+(abs(?Press_XCoOrdValue - ?IndustrialCamera_XCoOrdValue))+(abs(?Press_XCoOrdValue - ?RFIDReader_XCoOrdValue))  as ?CumulativeDistance)"+
	    		"WHERE{ ?LowerShellWarehouseInstance :hasPositionX ?XCoOrdInst_LS; rdf:type :rescom_resources_LowerShellProvider.  ?XCoOrdInst_LS :Value ?LowerShellWarehouse_XCoOrdValue."+
	    		"?UpperShellWarehouseInstance :hasPositionX ?XCoOrdInst_US; rdf:type :rescom_resources_UpperShellProvider. ?XCoOrdInst_US :Value ?UpperShellWarehouse_XCoOrdValue."+
	    		"?CircuitBoardWarehouseInstance :hasPositionX ?XCoOrdInst_CB; rdf:type :rescom_resources_CircuitBoardProvider. ?XCoOrdInst_CB :Value ?CircuitBoardWarehouse_XCoOrdValue."+
	    		"?IndustrialCameraInstance :hasPositionX ?XCoOrdInst_Camera; rdf:type :rescom_resources_IndustrialCamera. ?XCoOrdInst_Camera :Value ?IndustrialCamera_XCoOrdValue."+
	    		"?RFIDReaderInstance :hasPositionX ?XCoOrdInst_RFIDReader; rdf:type :rescom_resources_RFIDReader. ?XCoOrdInst_RFIDReader :Value ?RFIDReader_XCoOrdValue."+
	    		"{ SELECT ?PressInstance ?Press_XCoOrdValue WHERE{ ?PressInstance :hasPositionX ?XCoOrdInst_Press; rdf:type :rescom_capability_abstracts_Assembler. ?XCoOrdInst_Press :Value ?Press_XCoOrdValue.} } }"+
	    		"order by asc(?CumulativeDistance) limit 1";
	    
	    String queryStr = "PREFIX : <http://www.semanticweb.org/d065085/ontologies/2018/9/untitled-ontology-81#> "+
	    		"PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> "+
	    		"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> "+
	    	"SELECT ?PressInstance 	((abs(?Press_XCoOrdValue - ?LowerShellWarehouse_XCoOrdValue)) "+
	    		"+(abs(?Press_XCoOrdValue - ?UpperShellWarehouse_XCoOrdValue)) +(abs(?Press_XCoOrdValue - ?CircuitBoardWarehouse_XCoOrdValue))"+
  					"+(abs(?Press_XCoOrdValue - ?IndustrialCamera_XCoOrdValue))+(abs(?Press_XCoOrdValue - ?RFIDReader_XCoOrdValue))  as ?CumulativeDistance)"+
	    		"WHERE{  ?LowerShellWarehouseInstance :xCoOrd ?LowerShellWarehouse_XCoOrdValue;  rdf:type :LowerShellWarehouse."+
  					"?UpperShellWarehouseInstance :xCoOrd ?UpperShellWarehouse_XCoOrdValue;  rdf:type :UpperShellWarehouse."+
	    		"?CircuitBoardWarehouseInstance :xCoOrd ?CircuitBoardWarehouse_XCoOrdValue;  rdf:type :CircuitBoardWareHouse."+
  					"?IndustrialCameraInstance :xCoOrd ?IndustrialCamera_XCoOrdValue; rdf:type :Camera."+
	    		"?RFIDReaderInstance :xCoOrd ?RFIDReader_XCoOrdValue;  rdf:type :RFIDReader."+
  					"{SELECT ?PressInstance ?Press_XCoOrdValue  WHERE{  ?PressInstance :xCoOrd ?Press_XCoOrdValue;rdf:type :PressMachine.}}}"+
	    		"ORDER BY asc(?CumulativeDistance) limit  3";
	    
	    Query query = QueryFactory.create(QueryString1);
	    QueryExecution queryExec = QueryExecutionFactory.create(QueryString1, ontModel);
	    
	    try{
	    	ResultSet results = queryExec.execSelect();
	    	while(results.hasNext()){
	    		QuerySolution solution = results.next();
	    		System.out.println(solution.toString());
	    		System.out.println(solution.getResource("?PressInstance"));
	    	}
	    }
	    finally{
	    	
	    }
	    
 
	}

}
