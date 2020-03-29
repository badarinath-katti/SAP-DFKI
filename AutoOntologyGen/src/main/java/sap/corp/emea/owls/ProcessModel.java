package sap.corp.emea.owls;

import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.AddAxiom;
import org.semanticweb.owlapi.model.AddImport;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLAxiom;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLDataProperty;
import org.semanticweb.owlapi.model.OWLDataPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLDataPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLEntity;
import org.semanticweb.owlapi.model.OWLImportsDeclaration;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

import sap.corp.emea.autoDomainOntoGen.Inspector;
import sap.corp.emea.owls.Conf.OWLSProcessModelConf;
import sap.corp.emea.owls.Conf.OWLSProfileConf;
import sap.corp.emea.utility.TupleWrapper;

public class ProcessModel {
	public static final String DOCUMENT_IRI = "http://www.rescom.org/owls/processModel";
	private static final String SWRL_IRI = "http://www.w3.org/2003/11/swrl";

	public static void createMethodProcessModelOntology()
			throws OWLOntologyCreationException, IOException, OWLOntologyStorageException {

		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();

		DefaultPrefixManager prefixManager = new DefaultPrefixManager(DOCUMENT_IRI + "#");
		DefaultPrefixManager topOntprefixManager = new DefaultPrefixManager(OWLSTopLevelOntology.DOCUMENT_IRI + "#");

		OWLOntology oWLOntology = oWLOntologyManager.createOntology(IRI.create(DOCUMENT_IRI));

		File ontologyFile = new File("ProcessModel.owl");
		if (ontologyFile.exists()) {
			ontologyFile.delete();
			ontologyFile.createNewFile();
		} else {
			ontologyFile.createNewFile();
		}

		File importFile = new File("OWLSTopLevelOntology.owl");
		oWLOntologyManager.loadOntologyFromOntologyDocument(importFile);
		OWLImportsDeclaration importDeclaration = oWLOntologyManager.getOWLDataFactory()
				.getOWLImportsDeclaration(IRI.create(importFile));
		oWLOntologyManager.applyChange(new AddImport(oWLOntology, importDeclaration));

		for (Map.Entry<String, List<String>> mapKeyValue : OWLSProcessModelConf.getprocessSubClassMapping()
				.entrySet()) {

			OWLClass processModelEntity = oWLDataFactory
					.getOWLClass(IRI.create(OWLSTopLevelOntology.DOCUMENT_IRI + "#" + mapKeyValue.getKey()));

			for (String subClass : mapKeyValue.getValue()) {
				OWLClass subClassEntity = oWLDataFactory.getOWLClass(IRI.create(DOCUMENT_IRI + "#" + subClass));

				OWLAxiom oWLAxiom = oWLDataFactory.getOWLSubClassOfAxiom(subClassEntity, processModelEntity);
				AddAxiom addAxiom = new AddAxiom(oWLOntology, oWLAxiom);
				oWLOntologyManager.applyChange(addAxiom);

			}
		}

		Map.Entry<String, HashMap<String, List<String>>> mapKeyValue = OWLSProcessModelConf.getVariableSubClassMapping()
				.entrySet().iterator().next();

		OWLClass variableEntity = oWLDataFactory.getOWLClass(IRI.create(SWRL_IRI + "#" + mapKeyValue.getKey()));

		for (Map.Entry<String, List<String>> mapEntry : mapKeyValue.getValue().entrySet()) {

			OWLClass subVariableEntity = oWLDataFactory.getOWLClass(IRI.create(DOCUMENT_IRI + "#" + mapEntry.getKey()));

			OWLAxiom oWLAxiom = oWLDataFactory.getOWLSubClassOfAxiom(subVariableEntity, variableEntity);
			AddAxiom addAxiom = new AddAxiom(oWLOntology, oWLAxiom);
			oWLOntologyManager.applyChange(addAxiom);

			for (String twoLevelSubVariable : mapEntry.getValue()) {
				OWLClass twoLevelSubVariableEntity = oWLDataFactory
						.getOWLClass(IRI.create(DOCUMENT_IRI + "#" + twoLevelSubVariable));
				oWLAxiom = oWLDataFactory.getOWLSubClassOfAxiom(twoLevelSubVariableEntity, subVariableEntity);
				addAxiom = new AddAxiom(oWLOntology, oWLAxiom);
				oWLOntologyManager.applyChange(addAxiom);
			}

		}

		for (Map.Entry<String, TupleWrapper.Tuple<String, String>> entry : OWLSProcessModelConf
				.getObjectPropertyClassMapping().entrySet()) {
			OWLObjectProperty oWLObjectProperty = oWLDataFactory.getOWLObjectProperty(entry.getKey(), prefixManager);
			oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(oWLObjectProperty));

			if (entry.getValue().getItem1() != null && !entry.getValue().getItem1().isEmpty()) {
				OWLClass DomainEntity = oWLDataFactory.getOWLClass(":" + entry.getValue().getItem1(), prefixManager);
				OWLObjectPropertyDomainAxiom oWLObjectPropertyDomainAxiom = oWLDataFactory
						.getOWLObjectPropertyDomainAxiom(oWLObjectProperty, DomainEntity);
				oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyDomainAxiom);
			}
			if (entry.getValue().getItem2() != null && !entry.getValue().getItem2().isEmpty()) {
				OWLClass RangeEntity = oWLDataFactory.getOWLClass(":" + entry.getValue().getItem2(), prefixManager);
				OWLObjectPropertyRangeAxiom oWLObjectPropertyRangeAxiom = oWLDataFactory
						.getOWLObjectPropertyRangeAxiom(oWLObjectProperty, RangeEntity);
				oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyRangeAxiom);
			}
		}

		for (Map.Entry<String, TupleWrapper.Tuple<String, Class<?>>> DataPropertyClassMapping : OWLSProcessModelConf
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
						topOntprefixManager);
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
