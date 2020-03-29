package sap.corp.emea.autoDomainOntoGen;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.lang.reflect.Field;
import java.util.Collections;
import java.util.EnumSet;
import java.util.Set;
import java.util.jar.JarEntry;
import java.util.jar.JarInputStream;

import org.semanticweb.owlapi.model.AddAxiom;
import org.semanticweb.owlapi.model.IRI;
import org.semanticweb.owlapi.model.OWLAnnotation;
import org.semanticweb.owlapi.model.OWLAxiom;
import org.semanticweb.owlapi.model.OWLClass;
import org.semanticweb.owlapi.model.OWLDataFactory;
import org.semanticweb.owlapi.model.OWLDataProperty;
import org.semanticweb.owlapi.model.OWLDataPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLDataPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLIndividualAxiom;
import org.semanticweb.owlapi.model.OWLNamedIndividual;
import org.semanticweb.owlapi.model.OWLObjectProperty;
import org.semanticweb.owlapi.model.OWLObjectPropertyDomainAxiom;
import org.semanticweb.owlapi.model.OWLObjectPropertyRangeAxiom;
import org.semanticweb.owlapi.model.OWLOntology;
import org.semanticweb.owlapi.model.OWLOntologyManager;
import org.semanticweb.owlapi.model.OWLOntologyStorageException;
import org.semanticweb.owlapi.model.OWLSubClassOfAxiom;
import org.semanticweb.owlapi.util.DefaultPrefixManager;

import rescom.ClassEntityAnnotation;
import rescom.DataPropertyAnnotation;
import rescom.ObjectPropertyAnnotation;
import rescom.concepts.Color;
import uk.ac.manchester.cs.owl.owlapi.OWLIndividualAxiomImpl;
import uk.ac.manchester.cs.owl.owlapi.OWLSubClassOfAxiomImpl;

public class Utility {

	public void setSubClassOfRelation(OWLOntologyManager oWLOntologyManager, OWLDataFactory oWLDataFactory,
			DefaultPrefixManager prefixManager, OWLOntology oWLOntology) throws IOException {
		JarEntry jarEntry;
		JarInputStream jarInputStream = CreateJarInputStream();
		while (true) {
			jarEntry = jarInputStream.getNextJarEntry();

			if (jarEntry == null) {
				jarInputStream.close();
				return;
			}

			if ((jarEntry.getName().endsWith(".class"))) {

				String classname = jarEntry.getName().replace('/', '.').substring(0, jarEntry.getName().length() - 6);
				Class<?> currentClass;
				try {
					currentClass = Class.forName(classname);
				} catch (ClassNotFoundException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					continue;
				}

				if (!currentClass.isAnnotationPresent(ClassEntityAnnotation.class)) {
					continue;
				}
				Class<?>[] superInterfaces = currentClass.getInterfaces();

				OWLClass ClassEntity = oWLDataFactory
						.getOWLClass(IRI.create(Inspector.DOCUMENT_IRI + "#" + currentClass.getName().replace('.', '_')));
				Set<OWLAnnotation> NO_ANNOTATIONS = Collections.emptySet();
				OWLSubClassOfAxiom OWLSubClassOfAxiom;

				if (superInterfaces != null) {
					for (Class<?> superInterface : superInterfaces) {
						OWLClass oWLSuperInterface;
						// if (!oWLOntology
						// .containsClassInSignature(IRI.create(Inspector.DOCUMENT_IRI
						// + "#" + clas.getName()))) {
						oWLSuperInterface = oWLDataFactory.getOWLClass(":" + superInterface.getName().replace('.', '_'), prefixManager);
						// oWLOntologyManager.addAxiom(oWLOntology,
						// oWLDataFactory.getOWLDeclarationAxiom(superClass));

						OWLSubClassOfAxiom = new OWLSubClassOfAxiomImpl(ClassEntity, oWLSuperInterface, NO_ANNOTATIONS);
						oWLOntologyManager.addAxiom(oWLOntology, OWLSubClassOfAxiom);
					}
				}

				Class<?> superClass = currentClass.getSuperclass();
				if ((superClass != null) && (!superClass.isInstance(Object.class)) && (!(superClass == Enum.class))) {
					OWLClass oWLSuperClass = oWLDataFactory.getOWLClass(":" + superClass.getName().replace('.', '_'), prefixManager);
					OWLSubClassOfAxiom = new OWLSubClassOfAxiomImpl(ClassEntity, oWLSuperClass, NO_ANNOTATIONS);
					oWLOntologyManager.addAxiom(oWLOntology, OWLSubClassOfAxiom);
				}

				else if (superClass == Enum.class) {

					for (Object EnumValue : currentClass.getEnumConstants()) {
						OWLNamedIndividual oWLNamedIndividual = oWLDataFactory.getOWLNamedIndividual(":" + EnumValue,
								prefixManager);
						OWLAxiom axiom = oWLDataFactory.getOWLClassAssertionAxiom(ClassEntity, oWLNamedIndividual);
						AddAxiom addAxion = new AddAxiom(oWLOntology, axiom);
						oWLOntologyManager.applyChange(addAxion);
					}
				}
			}
		}
	}

	private JarInputStream CreateJarInputStream() throws FileNotFoundException, IOException {
		JarInputStream jarInputStream = new JarInputStream(
				new FileInputStream("C:\\Users\\D065085\\.m2\\repository\\sap\\corp\\global\\"
						+ "ResComInfModel\\0.0.1-SNAPSHOT\\ResComInfModel-0.0.1-SNAPSHOT.jar"));
		return jarInputStream;
	}

	public void AssignObjectAndDataProperties(OWLOntologyManager oWLOntologyManager, OWLDataFactory oWLDataFactory,
			DefaultPrefixManager prefixManager, OWLOntology oWLOntology)
			throws ClassNotFoundException, FileNotFoundException, IOException {
		JarEntry jarEntry;
		JarInputStream jarInputStream = CreateJarInputStream();
		while (true) {
			jarEntry = jarInputStream.getNextJarEntry();

			if (jarEntry == null) {
				jarInputStream.close();
				return;
			}
			if ((jarEntry.getName().endsWith(".class"))) {

				String classname = jarEntry.getName().replace('/', '.').substring(0, jarEntry.getName().length() - 6);
				Class<?> currentClass = Class.forName(classname);

				OWLClass ClassEntity = null;
				if (currentClass.isAnnotationPresent(ClassEntityAnnotation.class)) {
					ClassEntity = oWLDataFactory.getOWLClass(":" + currentClass.getName().replace('.', '_'), prefixManager);
					// oWLOntologyManager.addAxiom(oWLOntology,
					// oWLDataFactory.getOWLDeclarationAxiom(ClassEntity));
				}

				if (currentClass.getFields().length > 0) {
					for (Field field : currentClass.getFields()) {
						Boolean IsBuiltIn = Inspector.IsBuiltIn(field.getType());
						if (field.isAnnotationPresent(DataPropertyAnnotation.class) && IsBuiltIn) {

							OWLDataProperty oWLDataProperty = oWLDataFactory.getOWLDataProperty(field.getName(),
									prefixManager);
							oWLOntologyManager.addAxiom(oWLOntology,
									oWLDataFactory.getOWLDeclarationAxiom(oWLDataProperty));

							OWLDataPropertyDomainAxiom oWLDataPropertyDomainAxiom = oWLDataFactory
									.getOWLDataPropertyDomainAxiom(oWLDataProperty, ClassEntity);

							OWLDataPropertyRangeAxiom oWLDataPropertyRangeAxiom = oWLDataFactory
									.getOWLDataPropertyRangeAxiom(oWLDataProperty,
											oWLDataFactory.getOWLDatatype(Inspector.getOWLDatatype(field.getType())));

							oWLOntologyManager.addAxiom(oWLOntology, oWLDataPropertyDomainAxiom);
							oWLOntologyManager.addAxiom(oWLOntology, oWLDataPropertyRangeAxiom);

						} else if (field.isAnnotationPresent(ObjectPropertyAnnotation.class) && !IsBuiltIn) {
							ObjectPropertyAnnotation objectPropertyAnnotation = field
									.getAnnotation(ObjectPropertyAnnotation.class);
							OWLObjectProperty oWLObjectProperty = oWLDataFactory
									.getOWLObjectProperty(objectPropertyAnnotation.objectPropertyName(), prefixManager);
							oWLOntologyManager.addAxiom(oWLOntology,
									oWLDataFactory.getOWLDeclarationAxiom(oWLObjectProperty));

							OWLObjectPropertyDomainAxiom oWLObjectPropertyDomainAxiom = oWLDataFactory
									.getOWLObjectPropertyDomainAxiom(oWLObjectProperty, ClassEntity);
							OWLObjectPropertyRangeAxiom oWLObjectPropertyRangeAxiom = oWLDataFactory
									.getOWLObjectPropertyRangeAxiom(oWLObjectProperty,
											oWLDataFactory.getOWLClass(":" + field.getType().getName().replace('.','_'), prefixManager));
							oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyDomainAxiom);
							oWLOntologyManager.addAxiom(oWLOntology, oWLObjectPropertyRangeAxiom);
						}
					}
				}
			}
		}
	}

	public void createOntologyClasses(OWLOntologyManager oWLOntologyManager, OWLDataFactory oWLDataFactory,
			DefaultPrefixManager prefixManager, OWLOntology oWLOntology) throws FileNotFoundException, IOException {
		JarEntry jarEntry;
		JarInputStream jarInputStream = CreateJarInputStream();

		while (true) {
			jarEntry = jarInputStream.getNextJarEntry();
			if (jarEntry == null) {
				jarInputStream.close();
				return;
			}

			if ((jarEntry.getName().endsWith(".class"))) {

				String classname = jarEntry.getName().replace('/', '.').substring(0, jarEntry.getName().length() - 6);
				try {
					Class<?> currentClass = Class.forName(classname);

					if (currentClass.isAnnotationPresent(ClassEntityAnnotation.class)) {
						OWLClass ClassEntity = oWLDataFactory.getOWLClass(":" + currentClass.getName().replace('.', '_'), prefixManager);
						oWLOntologyManager.addAxiom(oWLOntology, oWLDataFactory.getOWLDeclarationAxiom(ClassEntity));
					}
					// classes.add(c);
				} catch (Throwable e) {
					System.out.println("WARNING: failed to instantiate " + classname + " from " + jarEntry.getName());
				}
			}
		}
	}

}
