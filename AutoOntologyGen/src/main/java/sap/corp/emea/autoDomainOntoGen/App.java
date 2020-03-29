package sap.corp.emea.autoDomainOntoGen;

import java.io.File;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.ListIterator;
import java.util.Set;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.AddAxiom;
import org.semanticweb.owlapi.model.AxiomType;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLAxiom;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLDeclarationAxiom;
import org.semanticweb.owlapi.model.OWLEntity;
import org.semanticweb.owlapi.model.OWLIndividual;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.reasoner.OWLReasoner;
import org.semanticweb.owlapi.reasoner.OWLReasonerFactory;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

import sap.corp.emea.Assertion.ResourceAssertion;
import sap.corp.emea.autoDomainOntoGen.reasoner.Reasoner;
import sap.corp.emea.owls.Grounding;
import sap.corp.emea.owls.MethodProfile;
import sap.corp.emea.owls.OWLSTopLevelOntology;
import sap.corp.emea.owls.ProcessModel;

public class App {

	public static void main(String[] args)
			throws IOException, OWLOntologyCreationException, OWLOntologyStorageException, ClassNotFoundException {
		
		//Reasoner.reasonOntology(Inspector.DOCUMENT_IRI + "#" + "rescom_resources_LowerShellProvider"); int i = 5; if(i == 5)return;

		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();

		DefaultPrefixManager prefixManager = new DefaultPrefixManager(Inspector.DOCUMENT_IRI + "#");

		OWLOntology oWLOntology = oWLOntologyManager.createOntology(IRI.create(Inspector.DOCUMENT_IRI));

		File ontologyFile = new File("RescomInformationModel.owl");
		if (ontologyFile.exists()) {
			ontologyFile.delete();
			ontologyFile.createNewFile();
		} else {
			ontologyFile.createNewFile();
		}

		Utility utility = new Utility();
		utility.createOntologyClasses(oWLOntologyManager, oWLDataFactory, prefixManager, oWLOntology);
		utility.setSubClassOfRelation(oWLOntologyManager, oWLDataFactory, prefixManager, oWLOntology);
		utility.AssignObjectAndDataProperties(oWLOntologyManager, oWLDataFactory, prefixManager, oWLOntology);

		/*Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
		String connectionUrl = "jdbc:sqlserver://localhost:1433;databaseName=ManufacturingExecution;integratedSecurity=true;";
		// System.setProperty("java.security.krb5.conf","c:\\flats\\java\\krb5.conf");

		try (Connection con = DriverManager.getConnection(connectionUrl); Statement stmt = con.createStatement();) {

			Set<OWLDeclarationAxiom> oWLAxioms = oWLOntology.getAxioms(AxiomType.DECLARATION, true);

			String SQL = "DELETE FROM ME_SEMANTIC_ANNOTATION; DBCC CHECKIDENT ('ME_SEMANTIC_ANNOTATION', RESEED, 0);";
			for (OWLAxiom oWLAxiom : oWLAxioms) {

				OWLEntity owlEntity = oWLAxiom.getSignature().iterator().next();

				SQL += "INSERT INTO ME_SEMANTIC_ANNOTATION VALUES ('" + owlEntity.getIRI() + "','"
						+ owlEntity.getIRI().getShortForm() + "');";
			}
			// ResultSet rs = stmt.executeQuery(SQL);
			stmt.execute(SQL);

			// Iterate through the data in the result set and display it.
			
			 * while (rs.next()) { System.out.println(rs.getString("ID") + " " +
			 * rs.getString("NAME")); }
			 
		}
		// Handle any errors that may have occurred.
		catch (SQLException e) {
			e.printStackTrace();
		}*/

		/*
		 * PelletReasonerFactory reasonerFactory =
		 * PelletReasonerFactory.getInstance();
		 * com.clarkparsia.pellet.owlapiv3.PelletReasoner pelletReasoner =
		 * reasonerFactory.createReasoner(oWLOntology);
		 * pelletReasoner.prepareReasoner();
		 * pelletReasoner.precomputeInferences();
		 */

		oWLOntologyManager.saveOntology(oWLOntology, IRI.create(ontologyFile.toURI()));

		OWLSTopLevelOntology.CreateOWLSTopLevelOntology();
		ProcessModel.createMethodProcessModelOntology();
		MethodProfile.createMethodProfileOntology();
		Grounding.createOWLSGroundingOntology();
		ResourceAssertion.createResourceAssertion("");
		
		System.out.println("Automatic Tbox and Abox Ontology generation is completed.");
	}
}
