package sap.corp.global.emea.ruleengine_Service;

import java.io.File;
import java.io.IOException;
import java.util.UUID;

import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.swrlapi.core.SWRLAPIRule;
import org.swrlapi.core.SWRLRuleEngine;
import org.swrlapi.exceptions.LiteralException;
import org.swrlapi.exceptions.SWRLBuiltInException;
import org.swrlapi.factory.SWRLAPIFactory;
import org.swrlapi.parser.SWRLParseException;
import org.swrlapi.sqwrl.SQWRLQueryEngine;
import org.swrlapi.sqwrl.SQWRLResult;
import org.swrlapi.sqwrl.exceptions.SQWRLException;


/**
 * Root resource (exposed at "myresource" path)
 */
@Path("myresource")
public class MyResource {

	public enum RuleEngineType {
		SWRL, SQWRL
	}
	
    /**
     * Method handling HTTP GET requests. The returned object will be sent
     * to the client as "text/plain" media type.
     *
     * @return String that will be returned as a text/plain response.
     * @throws IOException 
     */
    @POST
    @Path("runRuleEngine")
    @Produces(MediaType.TEXT_PLAIN)
    public Boolean runRuleEngine(String rule, RuleEngineType ruleEngineType) {

		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();
		File ontologyFile = new File("C:\\Users\\D065085\\workspace\\ABoxOntReceiverService\\Status_Services\\RescomInformationModel.owl");
		OWLOntology oWLOntology = null;
		try {
			oWLOntology = oWLOntologyManager.loadOntologyFromOntologyDocument(ontologyFile);
		} catch (OWLOntologyCreationException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		if (ruleEngineType == RuleEngineType.SQWRL) {
			SQWRLQueryEngine sqwrlQueryEngine = SWRLAPIFactory.createSQWRLQueryEngine(oWLOntology);
			SQWRLResult SQWRLResult = null;
			try {
				SQWRLResult = sqwrlQueryEngine.runSQWRLQuery(UUID.randomUUID().toString(), rule);
			} catch (SQWRLException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (SWRLParseException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			try {
				if (SQWRLResult.next()) {
					return SQWRLResult.getLiteral("C0").getBoolean();
				}
			} catch (LiteralException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (SQWRLException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} else if (ruleEngineType == RuleEngineType.SWRL) {
			SWRLRuleEngine swrlRuleEngine = SWRLAPIFactory.createSWRLRuleEngine(oWLOntology);
			try {
				try {
					SWRLAPIRule swrlAPIRule = swrlRuleEngine.createSWRLRule(UUID.randomUUID().toString(), rule);
				} catch (SWRLParseException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			} catch (SWRLBuiltInException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			swrlRuleEngine.infer();
			try {
				oWLOntologyManager.saveOntology(oWLOntology);
				return true;
			} catch (OWLOntologyStorageException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		return false;
	}
}
