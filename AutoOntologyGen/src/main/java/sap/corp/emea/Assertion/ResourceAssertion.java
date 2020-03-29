package sap.corp.emea.Assertion;

import java.io.File;
import java.util.Set;

import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.AxiomType;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLAxiom;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLIndividual;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

public class ResourceAssertion {

	private static final String ResCom_InfoModel_Document_IRI = "http://emea.global.corp.sap/rescom";

	/**
	 * @param Resource
	 * @throws OWLOntologyCreationException
	 * @throws OWLOntologyStorageException
	 */
	public static void createResourceAssertion(String Resource)
			throws OWLOntologyCreationException, OWLOntologyStorageException {

		OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
		OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();
		File RescomInformationModelFile = new File(
				"C:\\Users\\D065085\\workspace\\AutoOntologyGen\\RescomInformationModel.owl");
		OWLOntology oWLRescomInformationModelOntology = oWLOntologyManager
				.loadOntologyFromOntologyDocument(RescomInformationModelFile);

		DefaultPrefixManager rescomInfoModelPrefixManager = new DefaultPrefixManager(
				ResCom_InfoModel_Document_IRI + "#");

		Resource = ResourceConstants.AutonomousTransporter;
		/* if (Resource.equals(ResourceConstants.AutonomousTransporter)) */ {

			OWLClass resourceClass = oWLDataFactory.getOWLClass(IRI.create(Resource));

			String resourceShortName = resourceClass.getIRI().getShortForm()
					.substring(resourceClass.getIRI().getShortForm().lastIndexOf('_') + 1);
			OWLIndividual resourceIndv = oWLDataFactory.getOWLNamedIndividual(
					IRI.create(ResCom_InfoModel_Document_IRI + "#" + resourceShortName + "Instance"));
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);

			for (OWLObjectPropertyDomainAxiom op : oWLRescomInformationModelOntology
					.getAxioms(AxiomType.OBJECT_PROPERTY_DOMAIN)) {
				if (op.getDomain().equals(resourceClass)) {
					for (OWLObjectProperty owlObjectProperty : op.getObjectPropertiesInSignature()) {
						// System.out.println("\t\t +: " +
						// owlObjectProperty.getIRI().getShortForm());

						Set<OWLObjectPropertyRangeAxiom> setRange = oWLRescomInformationModelOntology
								.getObjectPropertyRangeAxioms(owlObjectProperty);

						for (OWLObjectPropertyRangeAxiom oWLObjectPropertyRangeAxiom : setRange) {

							/*
							 * String rangeShortName =
							 * oWLObjectPropertyRangeAxiom.getRange().asOWLClass
							 * ().getIRI() .getRemainder().get().substring(
							 * oWLObjectPropertyRangeAxiom.getRange().asOWLClass
							 * ()
							 * .getIRI().getRemainder().get().lastIndexOf('_') +
							 * 1);
							 */

							String rangeShortName = oWLObjectPropertyRangeAxiom.getRange().asOWLClass().getIRI()
									.getShortForm().substring(oWLObjectPropertyRangeAxiom.getRange().asOWLClass()
											.getIRI().getShortForm().lastIndexOf('_') + 1);

							OWLIndividual resourceRangeIndv = oWLDataFactory.getOWLNamedIndividual(
									IRI.create(rangeShortName + "_" + resourceShortName + "_Instance"));

							OWLAxiom axmResourceRangeIndv = oWLDataFactory.getOWLClassAssertionAxiom(
									oWLObjectPropertyRangeAxiom.getRange().asOWLClass(), resourceRangeIndv);
							oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceRangeIndv);

							OWLAxiom axmObjectPropertyAssertion = oWLDataFactory.getOWLObjectPropertyAssertionAxiom(
									owlObjectProperty, resourceIndv, resourceRangeIndv);
							oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyAssertion);
						}
					}
				}
			}
		}
		Resource = ResourceConstants.LowerShellProvider;
		/* else if (Resource.equals(ResourceConstants.LowerShellProvider)) */ {

			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_capability_concrete_MaterialSupplier",
					rescomInfoModelPrefixManager);
			OWLClass objectPropertyRangeClass = oWLDataFactory.getOWLClass(":rescom_product_LowerShell",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory
					.getOWLNamedIndividual(":" + "LowerShell_MaterialSupplier_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);

			OWLIndividual objectPropertyRangeIndv = oWLDataFactory
					.getOWLNamedIndividual(":rescom_product_LowerShell_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmObjectPropertyRangeIndv = oWLDataFactory.getOWLClassAssertionAxiom(objectPropertyRangeClass,
					objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyRangeIndv);

			OWLObjectProperty owlObjectProperty = oWLDataFactory.getOWLObjectProperty(":supplies",
					rescomInfoModelPrefixManager);

			OWLAxiom axmObjectPropertyAssertion = oWLDataFactory.getOWLObjectPropertyAssertionAxiom(owlObjectProperty,
					resourceIndv, objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyAssertion);

		}

		Resource = ResourceConstants.CircuitBoardProvider;
		/* else if (Resource.equals(ResourceConstants.CircuitBoardProvider)) */ {

			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_capability_concrete_MaterialSupplier",
					rescomInfoModelPrefixManager);
			OWLClass objectPropertyRangeClass = oWLDataFactory.getOWLClass(":rescom_product_CircuitBoard",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory.getOWLNamedIndividual(
					":" + "CircuitBoard_MaterialSupplier_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);

			OWLIndividual objectPropertyRangeIndv = oWLDataFactory
					.getOWLNamedIndividual(":rescom_product_CircuitBoard_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmObjectPropertyRangeIndv = oWLDataFactory.getOWLClassAssertionAxiom(objectPropertyRangeClass,
					objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyRangeIndv);

			OWLObjectProperty owlObjectProperty = oWLDataFactory.getOWLObjectProperty(":supplies",
					rescomInfoModelPrefixManager);

			OWLAxiom axmObjectPropertyAssertion = oWLDataFactory.getOWLObjectPropertyAssertionAxiom(owlObjectProperty,
					resourceIndv, objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyAssertion);

		}
		Resource = ResourceConstants.UpperShellProvider;
		/* else if (Resource.equals(ResourceConstants.UpperShellProvider)) */ {

			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_capability_concrete_MaterialSupplier",
					rescomInfoModelPrefixManager);
			OWLClass objectPropertyRangeClass = oWLDataFactory.getOWLClass(":rescom_product_UpperShell",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory
					.getOWLNamedIndividual(":" + "UpperShell_MaterialSupplier_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);

			OWLIndividual objectPropertyRangeIndv = oWLDataFactory
					.getOWLNamedIndividual(":rescom_product_UpperShell_Instance", rescomInfoModelPrefixManager);
			OWLAxiom axmObjectPropertyRangeIndv = oWLDataFactory.getOWLClassAssertionAxiom(objectPropertyRangeClass,
					objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyRangeIndv);

			OWLObjectProperty owlObjectProperty = oWLDataFactory.getOWLObjectProperty(":supplies",
					rescomInfoModelPrefixManager);

			OWLAxiom axmObjectPropertyAssertion = oWLDataFactory.getOWLObjectPropertyAssertionAxiom(owlObjectProperty,
					resourceIndv, objectPropertyRangeIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmObjectPropertyAssertion);

		}
		Resource = ResourceConstants.Presser;
		/* else if (Resource.equals(ResourceConstants.Presser)) */ {

			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_capability_abstracts_Assembler",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory.getOWLNamedIndividual(":" + "PneumaticPress_Instance",
					rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);

			/*
			 * OWLIndividual objectPropertyRangeIndv = oWLDataFactory
			 * .getOWLNamedIndividual(":rescom_product_UpperShell_Instance",
			 * rescomInfoModelPrefixManager); OWLAxiom
			 * axmObjectPropertyRangeIndv =
			 * oWLDataFactory.getOWLClassAssertionAxiom(
			 * objectPropertyRangeClass, objectPropertyRangeIndv);
			 * oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology,
			 * axmObjectPropertyRangeIndv);
			 * 
			 * OWLObjectProperty owlObjectProperty =
			 * oWLDataFactory.getOWLObjectProperty(":supplies",
			 * rescomInfoModelPrefixManager);
			 * 
			 * OWLAxiom axmObjectPropertyAssertion =
			 * oWLDataFactory.getOWLObjectPropertyAssertionAxiom(
			 * owlObjectProperty, resourceIndv, objectPropertyRangeIndv);
			 * oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology,
			 * axmObjectPropertyAssertion);
			 */
			OWLIndividual resourceIndv2 = oWLDataFactory.getOWLNamedIndividual(":" + "ElectricAssembler_Instance",
					rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv2 = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv2);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv2);

		}

		Resource = ResourceConstants.IndustrialCamera;
		{
			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_resources_IndustrialCamera",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory.getOWLNamedIndividual(":" + "IndustrialCamera_Instance",
					rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);
		}

		Resource = ResourceConstants.RFIDReader;
		{
			OWLClass resourceClass = oWLDataFactory.getOWLClass(":" + "rescom_resources_RFIDReader",
					rescomInfoModelPrefixManager);

			OWLIndividual resourceIndv = oWLDataFactory.getOWLNamedIndividual(":" + "RFIDReader_Instance",
					rescomInfoModelPrefixManager);
			OWLAxiom axmResourceIndv = oWLDataFactory.getOWLClassAssertionAxiom(resourceClass, resourceIndv);
			oWLOntologyManager.addAxiom(oWLRescomInformationModelOntology, axmResourceIndv);
		}

		// oWLOntologyManager.applyChange(addAxion);
		oWLOntologyManager.saveOntology(oWLRescomInformationModelOntology,
				IRI.create(RescomInformationModelFile.toURI()));
	}

}