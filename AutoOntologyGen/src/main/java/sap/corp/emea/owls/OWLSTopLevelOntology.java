package sap.corp.emea.owls;

import java.io.File;
import java.io.IOException;
import java.util.HashMap;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

import sap.corp.emea.owls.Conf.OWLSOntologyConf;
import sap.corp.emea.utility.TupleWrapper;

public class OWLSTopLevelOntology {

	public static final String DOCUMENT_IRI = "http://www.rescom.org/owls";

	public static void CreateOWLSTopLevelOntology()
			throws OWLOntologyCreationException, IOException, OWLOntologyStorageException {
		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();

		DefaultPrefixManager prefixManager = new DefaultPrefixManager(DOCUMENT_IRI + "#");
		OWLOntology oWLOntology = oWLOntologyManager.createOntology(IRI.create(DOCUMENT_IRI));

		File ontologyFile = new File("OWLSTopLevelOntology.owl");
		if (ontologyFile.exists()) {
			ontologyFile.delete();
			ontologyFile.createNewFile();
		} else {
			ontologyFile.createNewFile();
		}

		for (String TopOntologyClass : OWLSOntologyConf.getTopOntologyClassNames()) {
			OWLClass ClassEntity = oWLDataFactory.getOWLClass(":" + TopOntologyClass, prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(ClassEntity));
		}

		HashMap<String, TupleWrapper.Tuple<String, String>> propertyClassMapping = OWLSOntologyConf.getPropertyClassMapping();

		for (String TopOntologyProperty : OWLSOntologyConf.getTopOntologyPropertyNames()) {
			OWLObjectProperty oWLObjectProperty = oWLDataFactory.getOWLObjectProperty(TopOntologyProperty,
					prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(oWLObjectProperty));

			OWLClass DomainEntity = oWLDataFactory
					.getOWLClass(":" + propertyClassMapping.get(TopOntologyProperty).getItem1(), prefixManager);
			OWLClass RangeEntity = oWLDataFactory
					.getOWLClass(":" + propertyClassMapping.get(TopOntologyProperty).getItem2(), prefixManager);

			OWLObjectPropertyDomainAxiom oWLObjectPropertyDomainAxiom = oWLDataFactory
					.getOWLObjectPropertyDomainAxiom(oWLObjectProperty, DomainEntity);
			OWLObjectPropertyRangeAxiom oWLObjectPropertyRangeAxiom = oWLDataFactory
					.getOWLObjectPropertyRangeAxiom(oWLObjectProperty, RangeEntity);
			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyDomainAxiom);
			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyRangeAxiom);
		}
		for (TupleWrapper.Tuple<String, String> InversePropertyMapping : OWLSOntologyConf.getinversePropertyMapping()) {

			OWLObjectProperty TopOntologyProperty = oWLDataFactory
					.getOWLObjectProperty(":" + InversePropertyMapping.getItem1(), prefixManager);
			OWLObjectProperty InverseProperty = oWLDataFactory
					.getOWLObjectProperty(":" + InversePropertyMapping.getItem2(), prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(InverseProperty));
			oWLOntologyManager.addAxiom(oWLOntology,
					oWLDataFactory.getOWLInverseObjectPropertiesAxiom(TopOntologyProperty, InverseProperty));

		}
		
		//Set<OWLObjectProperty> prop = oWLOntology.getObjectPropertiesInSignature();		
		//Set<OWLAxiom> axioms = oWLOntology.getAxioms(true);
		oWLOntologyManager.saveOntology(oWLOntology, IRI.create(ontologyFile.toURI()));
	}

}
