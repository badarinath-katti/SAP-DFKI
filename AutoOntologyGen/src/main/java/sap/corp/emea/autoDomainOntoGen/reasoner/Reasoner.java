package sap.corp.emea.autoDomainOntoGen.reasoner;

import java.io.File;
import java.io.FilenameFilter;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;

import org.semanticweb.HermiT.Configuration;
import org.semanticweb.HermiT.ReasonerFactory;
import org.semanticweb.owlapi.apibinding.OWLManager;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLDataProperty;
import org.semanticweb.owlapi.model.OWLIndividual;
import org.semanticweb.owlapi.model.OWLLiteral;
import org.semanticweb.owlapi.model.OWLNamedIndividual;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyCreationException;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.reasoner.InferenceType;
import org.semanticweb.owlapi.reasoner.NodeSet;
import org.semanticweb.owlapi.reasoner.OWLReasoner;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

import sap.corp.emea.autoDomainOntoGen.Inspector;
import sap.corp.emea.autoDomainOntoGen.Utility;
import sap.corp.emea.owls.Grounding;
import sap.corp.emea.owls.OWLSTopLevelOntology;
import sap.corp.emea.owls.ProcessModel;

public class Reasoner {
	public static final String DocumentURL = "http://emea.global.corp.sap/rescom";

	public static void reasonOntology(String Semantic_Annotation) throws OWLOntologyCreationException {

		DefaultPrefixManager prefixManager = new DefaultPrefixManager(DocumentURL + "#");
		File directory = new File("C:\\Users\\D065085\\workspace\\ABoxOntReceiverService\\OWL-S");
		File[] directoryListing = directory.listFiles(new FilenameFilter() {
			public boolean accept(File dir, String name) {
				return name.toLowerCase().endsWith(".owl");
			}
		});
		if (directoryListing != null) {
			for (File child : directoryListing) {
				OWLOntologyManager oWLOntologyManager = OWLManager.createOWLOntologyManager();
				OWLDataFactory oWLDataFactory = oWLOntologyManager.getOWLDataFactory();
				File ontologyFile = new File(child.getPath());
				OWLOntology oWLOntology = oWLOntologyManager.loadOntologyFromOntologyDocument(ontologyFile);

				ReasonerFactory reasonerFactory = new ReasonerFactory();
				Configuration configuration = new Configuration();
				configuration.throwInconsistentOntologyException = false;
				OWLReasoner owlReasoner = reasonerFactory.createReasoner(oWLOntology);
				boolean isOntologyConsistent = owlReasoner.isConsistent();
				if (isOntologyConsistent) {
					owlReasoner.precomputeInferences(InferenceType.CLASS_HIERARCHY, InferenceType.CLASS_ASSERTIONS,
							InferenceType.OBJECT_PROPERTY_HIERARCHY, InferenceType.DATA_PROPERTY_HIERARCHY,
							InferenceType.OBJECT_PROPERTY_ASSERTIONS);

					OWLClass methodProcessModelClass = oWLDataFactory
							.getOWLClass(IRI.create(OWLSTopLevelOntology.DOCUMENT_IRI + "#" + "MethodProcessModel"));
					OWLNamedIndividual methodProcessModelIndv = owlReasoner.getInstances(methodProcessModelClass)
							.getFlattened().iterator().next();
					/*
					 * OWLNamedIndividual LowerShell_MaterialSuplier_Instance =
					 * oWLDataFactory.getOWLNamedIndividual(
					 * IRI.create(DocumentURL + "#" +
					 * "LowerShell_MaterialSupplier_Instance"));
					 */
					OWLObjectProperty manufacturingServiceObjectProperty = oWLDataFactory
							.getOWLObjectProperty(IRI.create(ProcessModel.DOCUMENT_IRI + "#" + "manufacturingService"));

					OWLNamedIndividual resourceService_Instance = owlReasoner
							.getObjectPropertyValues(methodProcessModelIndv, manufacturingServiceObjectProperty)
							.getFlattened().iterator().next();

					NodeSet<OWLClass> types = owlReasoner.getTypes(resourceService_Instance);

					OWLClass requirement = oWLDataFactory.getOWLClass(IRI.create(Semantic_Annotation));

					for (org.semanticweb.owlapi.reasoner.Node<OWLClass> owlClass : types) {

						if (owlClass.getEntities().iterator().hasNext()) {
							if (owlClass.getEntities().iterator().next().getIRI().toString()
									.equals(Semantic_Annotation)) {

								OWLOntologyManager oWLOntologyManager1 = OWLManager.createOWLOntologyManager();
								OWLDataFactory oWLDataFactory1 = oWLOntologyManager1.getOWLDataFactory();
								File ontologyFile1 = new File(
										"C:\\Users\\D065085\\workspace\\ABoxOntReceiverService\\Status_Services\\"
												+ ontologyFile.getName());

								OWLOntology oWLOntology1 = oWLOntologyManager1
										.loadOntologyFromOntologyDocument(ontologyFile1);

								OWLObjectProperty xCoOrdProp = oWLDataFactory1
										.getOWLObjectProperty(IRI.create(DocumentURL + "#" + "hasPositionX"));
								OWLObjectProperty yCoOrdProp = oWLDataFactory1
										.getOWLObjectProperty(IRI.create(DocumentURL + "#" + "hasPositionY"));
								OWLObjectProperty zCoOrdProp = oWLDataFactory1
										.getOWLObjectProperty(IRI.create(DocumentURL + "#" + "hasPositionZ"));

								OWLReasoner owlReasoner1 = reasonerFactory.createReasoner(oWLOntology1);

								OWLNamedIndividual XCoOrd_Instance = owlReasoner1
										.getObjectPropertyValues(resourceService_Instance, xCoOrdProp).getFlattened()
										.iterator().next();
								OWLNamedIndividual YCoOrd_Instance = owlReasoner1
										.getObjectPropertyValues(resourceService_Instance, yCoOrdProp).getFlattened()
										.iterator().next();
								OWLNamedIndividual ZCoOrd_Instance = owlReasoner1
										.getObjectPropertyValues(resourceService_Instance, zCoOrdProp).getFlattened()
										.iterator().next();

								OWLDataProperty valueProperty = oWLDataFactory1
										.getOWLDataProperty(IRI.create(DocumentURL + "#" + "Value"));

								OWLLiteral XCoOrd_Value = owlReasoner1
										.getDataPropertyValues(XCoOrd_Instance, valueProperty).iterator().next();
								OWLLiteral YCoOrd_Value = owlReasoner1
										.getDataPropertyValues(YCoOrd_Instance, valueProperty).iterator().next();
								OWLLiteral ZCoOrd_Value = owlReasoner1
										.getDataPropertyValues(ZCoOrd_Instance, valueProperty).iterator().next();

								List<OpcuaNodeLink> nodes = new ArrayList<OpcuaNodeLink>();

								OWLClass atomicProcessGroundingClass = oWLDataFactory.getOWLClass(
										IRI.create(Grounding.DOCUMENT_IRI + "#" + "AtomicProcessGrounding"));
								OWLNamedIndividual atomicProcessGroundingIndv = owlReasoner
										.getInstances(atomicProcessGroundingClass).getFlattened().iterator().next();
								OWLObjectProperty hasNodeReferenceProperty = oWLDataFactory.getOWLObjectProperty(
										IRI.create(Grounding.DOCUMENT_IRI + "#" + "hasNodeReference"));
								OWLNamedIndividual rootNodeIndv = owlReasoner
										.getObjectPropertyValues(atomicProcessGroundingIndv, hasNodeReferenceProperty)
										.getFlattened().iterator().next();

								OWLDataProperty endPointURLDataProperty = oWLDataFactory
										.getOWLDataProperty(IRI.create(Grounding.DOCUMENT_IRI + "#" + "endPointURL"));
								String serverEndPointURL = owlReasoner
										.getDataPropertyValues(atomicProcessGroundingIndv, endPointURLDataProperty)
										.iterator().next().getLiteral();

								OpcuaNodeLink rootNodeLink = new OpcuaNodeLink();
								OWLDataProperty nodeIDDataProperty = oWLDataFactory
										.getOWLDataProperty(IRI.create(Grounding.DOCUMENT_IRI + "#" + "nodeID"));
								String parentNodeID = rootNodeLink.nodeID = owlReasoner
										.getDataPropertyValues(rootNodeIndv, nodeIDDataProperty).iterator().next()
										.getLiteral();
								rootNodeLink.parentNodeId = null;
								rootNodeLink.parentNodebrowseName = null;
								rootNodeLink.browseName = rootNodeIndv.getIRI().getFragment();
								nodes.add(rootNodeLink);

								OWLNamedIndividual currentNodeIndv = rootNodeIndv;
								while (owlReasoner.getObjectPropertyValues(currentNodeIndv, hasNodeReferenceProperty)
										.getFlattened().iterator().hasNext()) {
									OWLNamedIndividual nextNodeIndv = owlReasoner
											.getObjectPropertyValues(currentNodeIndv, hasNodeReferenceProperty)
											.getFlattened().iterator().next();

									rootNodeLink = new OpcuaNodeLink();
									
									rootNodeLink.parentNodeId = parentNodeID;
									parentNodeID = rootNodeLink.nodeID = owlReasoner
											.getDataPropertyValues(nextNodeIndv, nodeIDDataProperty).iterator().next()
											.getLiteral();
									
									rootNodeLink.browseName = nextNodeIndv.getIRI().getFragment();

									OWLDataProperty browseDirectionDataProperty = oWLDataFactory.getOWLDataProperty(
											IRI.create(Grounding.DOCUMENT_IRI + "#" + "browseDirection"));
									rootNodeLink.BrowseDirection = owlReasoner
											.getDataPropertyValues(nextNodeIndv, browseDirectionDataProperty).iterator()
											.next().getLiteral();
									
									OWLDataProperty referenceTypeDataProperty = oWLDataFactory.getOWLDataProperty(
											IRI.create(Grounding.DOCUMENT_IRI + "#" + "referenceType"));
									rootNodeLink.referenceType = owlReasoner
											.getDataPropertyValues(nextNodeIndv, referenceTypeDataProperty).iterator()
											.next().getLiteral();
									
									OWLDataProperty browseResultMaskDataProperty = oWLDataFactory.getOWLDataProperty(
											IRI.create(Grounding.DOCUMENT_IRI + "#" + "browseResultMask"));
									rootNodeLink.browseResultMask = owlReasoner
											.getDataPropertyValues(nextNodeIndv, browseResultMaskDataProperty).iterator()
											.next().getLiteral();
									
									OWLDataProperty nodeClassMaskDataProperty = oWLDataFactory.getOWLDataProperty(
											IRI.create(Grounding.DOCUMENT_IRI + "#" + "nodeClassMask"));
									rootNodeLink.NodeClassMask = owlReasoner
											.getDataPropertyValues(nextNodeIndv, nodeClassMaskDataProperty).iterator()
											.next().getLiteral();
									
									nodes.add(rootNodeLink);
									currentNodeIndv = nextNodeIndv;
								}

							}
						}
					}
				}
			}
		}
	}

}
