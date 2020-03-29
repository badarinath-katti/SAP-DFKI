package sap.corp.emea.owls;

import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.AddImport;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLDataProperty;
import org.semanticweb.owlapi.model.OWLDataPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLDataPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLImportsDeclaration;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.util.DefaultPrefixManager;
import org.semanticweb.owlapitools.decomposition.TopEquivalenceEvaluator;

import sap.corp.emea.autoDomainOntoGen.Inspector;
import sap.corp.emea.owls.Conf.OWLSGroundingConf;
import sap.corp.emea.owls.Conf.OWLSProcessModelConf;
import sap.corp.emea.owls.Conf.OWLSProfileConf;
import sap.corp.emea.utility.TupleWrapper;

public class Grounding {

	public static final String DOCUMENT_IRI = "http://www.rescom.org/owls/methodGrounding";

	public static void createOWLSGroundingOntology()
			throws OWLOntologyCreationException, OWLOntologyStorageException, IOException {

		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();

		DefaultPrefixManager topOntPrefixManager = new DefaultPrefixManager(OWLSTopLevelOntology.DOCUMENT_IRI + "#");
		DefaultPrefixManager processModelPrefixManager = new DefaultPrefixManager(ProcessModel.DOCUMENT_IRI + "#");
		DefaultPrefixManager prefixManager = new DefaultPrefixManager(DOCUMENT_IRI + "#");

		OWLOntology oWLOntology = oWLOntologyManager.createOntology(IRI.create(DOCUMENT_IRI));

		File ontologyFile = new File("MethodGrounding.owl");
		if (ontologyFile.exists()) {
			ontologyFile.delete();
			ontologyFile.createNewFile();
		} else {
			ontologyFile.createNewFile();
		}

		File oWLSTopLevelOntologyFile = new File("OWLSTopLevelOntology.owl");
		File processFile = new File("ProcessModel.owl");
		// oWLOntologyManager.loadOntologyFromOntologyDocument(IRI.create("http://www.rescom.org/owls"));
		oWLOntologyManager.loadOntologyFromOntologyDocument(oWLSTopLevelOntologyFile);
		oWLOntologyManager.loadOntologyFromOntologyDocument(processFile);

		OWLImportsDeclaration processModelImportDeclaration = oWLOntologyManager.getOWLDataFactory()
				.getOWLImportsDeclaration(IRI.create(processFile));
		oWLOntologyManager.applyChange(new AddImport(oWLOntology, processModelImportDeclaration));

		for (String TopOntologyClass : OWLSGroundingConf.getGroundingClasses()) {
			OWLClass ClassEntity = oWLDataFactory.getOWLClass(":" + TopOntologyClass, prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(ClassEntity));
		}

		HashMap<String, TupleWrapper.Tuple<String, String>> objectPropertyClassMapping = OWLSGroundingConf
				.getObjectPropertyClassMapping();
		for (String objectProperty : OWLSGroundingConf.getGroundingObjectProperties()) {
			OWLObjectProperty oWLObjectProperty = oWLDataFactory.getOWLObjectProperty(objectProperty, prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(oWLObjectProperty));

			OWLClass DomainEntity, RangeEntity;
			if (oWLOntology.containsClassInSignature(IRI.create(OWLSTopLevelOntology.DOCUMENT_IRI + "#"
					+ objectPropertyClassMapping.get(objectProperty).getItem1()), true))
				DomainEntity = oWLDataFactory.getOWLClass(
						":" + objectPropertyClassMapping.get(objectProperty).getItem1(), topOntPrefixManager);
			else if (oWLOntology.containsClassInSignature(IRI.create(
					ProcessModel.DOCUMENT_IRI + "#" + objectPropertyClassMapping.get(objectProperty).getItem1()), true))
				DomainEntity = oWLDataFactory.getOWLClass(
						":" + objectPropertyClassMapping.get(objectProperty).getItem1(), processModelPrefixManager);
			else
				DomainEntity = oWLDataFactory
						.getOWLClass(":" + objectPropertyClassMapping.get(objectProperty).getItem1(), prefixManager);

			if (oWLOntology.containsClassInSignature(IRI.create(OWLSTopLevelOntology.DOCUMENT_IRI + "#"
					+ objectPropertyClassMapping.get(objectProperty).getItem2()), true))
				RangeEntity = oWLDataFactory.getOWLClass(
						":" + objectPropertyClassMapping.get(objectProperty).getItem2(), topOntPrefixManager);
			else if (oWLOntology.containsClassInSignature(IRI.create(
					ProcessModel.DOCUMENT_IRI + "#" + objectPropertyClassMapping.get(objectProperty).getItem2()), true))
				RangeEntity = oWLDataFactory.getOWLClass(
						":" + objectPropertyClassMapping.get(objectProperty).getItem2(), processModelPrefixManager);
			else
				RangeEntity = oWLDataFactory
						.getOWLClass(":" + objectPropertyClassMapping.get(objectProperty).getItem2(), prefixManager);

			OWLObjectPropertyDomainAxiom oWLObjectPropertyDomainAxiom = oWLDataFactory
					.getOWLObjectPropertyDomainAxiom(oWLObjectProperty, DomainEntity);
			OWLObjectPropertyRangeAxiom oWLObjectPropertyRangeAxiom = oWLDataFactory
					.getOWLObjectPropertyRangeAxiom(oWLObjectProperty, RangeEntity);

			// Set<OWLAxiom> axioms = oWLOntology.getAxioms(true);
			// for (OWLAxiom owlAxiom : axioms) {
			// System.out.println(owlAxiom);
			// if (!owlAxiom.getSignature().isEmpty()) {
			// System.out.println(owlAxiom.getSignature().iterator().next().getIRI().getShortForm());
			// //
			// if(oWLOntology.containsClassInSignature(IRI.create(DOCUMENT_IRI
			// // + "#" +
			// //
			// owlAxiom.getSignature().iterator().next().getIRI().getShortForm()))
			// }
			// }

			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyDomainAxiom);
			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyRangeAxiom);
		}

		for (Map.Entry<String, TupleWrapper.Tuple<String, Class<?>>> DataPropertyClassMapping : OWLSGroundingConf
				.getDataPropertyClassMapping().entrySet()) {
			OWLDataProperty oWLDataProperty = oWLDataFactory.getOWLDataProperty(DataPropertyClassMapping.getKey(),
					prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(oWLDataProperty));

			OWLClass DomainEntity;
			if (oWLOntology.containsClassInSignature(
					IRI.create(
							OWLSTopLevelOntology.DOCUMENT_IRI + "#" + DataPropertyClassMapping.getValue().getItem1()),
					true))
				DomainEntity = oWLDataFactory.getOWLClass(":" + DataPropertyClassMapping.getValue().getItem1(),
						topOntPrefixManager);
			else
				DomainEntity = oWLDataFactory.getOWLClass(":" + DataPropertyClassMapping.getValue().getItem1(),
						prefixManager);

			// OWLDatatype stringOWLDatatype = oWLDataFactory.getOWLDatatype(
			// Inspector.getOWLDatatype(DataPropertyClassMapping.getValue().getItem2()),
			// prefixManager);

			OWLDataPropertyDomainAxiom oWLObjectPropertyDomainAxiom = oWLDataFactory
					.getOWLDataPropertyDomainAxiom(oWLDataProperty, DomainEntity);

			OWLDataPropertyRangeAxiom oWLObjectPropertyRangeAxiom = oWLDataFactory.getOWLDataPropertyRangeAxiom(
					oWLDataProperty, Inspector.getOWLDatatype(DataPropertyClassMapping.getValue().getItem2()));

			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyDomainAxiom);
			oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyRangeAxiom);
		}

		oWLOntologyManager.saveOntology(oWLOntology, IRI.create(ontologyFile.toURI()));
	}

}
