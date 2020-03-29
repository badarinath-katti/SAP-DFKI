using Manufacturing_Execution_Simulation.Application_Utility;
using Manufacturing_Execution_Simulation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Manufacturing_Execution_Simulation.Controllers
{
    public class ABoxOntologyGenerationController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();
        private Dictionary<long, string> dictAxioms = new Dictionary<long, string>()
                {
                  {1, "EquivalentTo"},
                  {2, "SubClassOf"},
                  {3, "Individual"},
                };

        public class DragDropTest_Product
        {
            [Key]
            public int ProductId { get; set; }
            public String ProductName { get; set; }

            public int ProductQuantity { get; set; }
        }

        // GET: ABoxOntologyGeneration
        public ActionResult Index()
        {
            OpcuaNodeList opcuaNodeList = new OpcuaNodeList();
            opcuaNodeList.ontologyConcepts = new List<OntologyConcept>();
            opcuaNodeList.ChildrenNodes = new List<OpcuaNodeLink>();
            ViewBag.OntologyConcepts = new SelectList( opcuaNodeList.ontologyConcepts, "IRI", "IRI");
            ViewBag.axiomList = new SelectList(dictAxioms.Select(kv => new { Value = kv.Key, Text = kv.Value }), "Text", "Text");
            return View(opcuaNodeList);
        }

        [HttpPost]
        public ActionResult browseOPCUAServer(OpcuaNodeList opcuaNodeList)
        {
            OPCUAUtility.connectToOPCUAServer(opcuaNodeList.opcuaEndpointURL);

            var mE_SEMANTIC_ANNOTATION = db.ME_SEMANTIC_ANNOTATION.ToList();
            opcuaNodeList.ontologyConcepts = mE_SEMANTIC_ANNOTATION
              .Select(x => new OntologyConcept() { Id = x.ID, IRI = x.IRI, Short_Name = x.SHORT_NAME })
              .ToList();
            ViewBag.OntologyConcepts = new SelectList(opcuaNodeList.ontologyConcepts, "IRI", "IRI");
            ViewBag.axiomList = new SelectList(dictAxioms.Select(kv => new { Value = kv.Value, Text = kv.Value }), "Text", "Text");

            return View("Index", OPCUAUtility.opcuaNodeStructure);
        }

        public IEnumerable<DragDropTest_Product> GetProds()
        {
            List<DragDropTest_Product> dragDropTest_Product = new List<DragDropTest_Product> {
                new DragDropTest_Product { ProductId = 1, ProductName = "Product 1", ProductQuantity = 10 },
                new DragDropTest_Product { ProductId = 2, ProductName = "Product 2", ProductQuantity = 20 },
                new DragDropTest_Product { ProductId = 3, ProductName = "Product 3", ProductQuantity = 30 }};
            return dragDropTest_Product;
        }

        public ActionResult TestDragDrop()
        {
            return View();
        }

        [HttpPost]
        public ActionResult saveMethodOntology(OpcuaNodeList opcuaNodeList)
        {
            String SelectedMethodNodeId = opcuaNodeList.SelectedMethodNodeId;
            opcuaNodeList.ChildrenNodes = OPCUAUtility.opcuaNodeStructure.ChildrenNodes;
            opcuaNodeList.MethodBrowseLinks = new List<OpcuaNodeLink>();

            int insertCount = 0;
            while (true)
            {
                OpcuaNodeLink link = opcuaNodeList.ChildrenNodes.Where(node => node.nodeID.Equals(SelectedMethodNodeId)).FirstOrDefault();
                //OpcuaNodeLink parentNode = opcuaNodeList.ChildrenNodes.Where(n => n.nodeID.Equals(sourceNode.parentNodeId)).FirstOrDefault();
                opcuaNodeList.MethodBrowseLinks.Insert(insertCount, link);

                if (String.IsNullOrWhiteSpace( link.parentNodeId))
                    break;
                
                SelectedMethodNodeId = link.parentNodeId;
                insertCount++;
            }
            opcuaNodeList.MethodBrowseLinks.Reverse();
            opcuaNodeList.ChildrenNodes = OPCUAUtility.opcuaNodeStructure.ChildrenNodes;
            String strOpcuaNodeList = new JavaScriptSerializer().Serialize(opcuaNodeList);
            RestClient.SendRequest("http://localhost:8081/ABoxOntReceiverService/webapi/myresource/getOntology", "POST", "application/json", strOpcuaNodeList);

            var mE_SEMANTIC_ANNOTATION = db.ME_SEMANTIC_ANNOTATION.ToList();
            opcuaNodeList.ontologyConcepts = mE_SEMANTIC_ANNOTATION
              .Select(x => new OntologyConcept() { Id = x.ID, IRI = x.IRI, Short_Name = x.SHORT_NAME })
              .ToList();
            OPCUAUtility.connectToOPCUAServer(opcuaNodeList.opcuaEndpointURL);
            ViewBag.OntologyConcepts = new SelectList(opcuaNodeList.ontologyConcepts, "IRI", "IRI");
            ViewBag.axiomList = new SelectList(dictAxioms.Select(kv => new { Value = kv.Key, Text = kv.Value }), "Text", "Text");

            return View("Index", OPCUAUtility.opcuaNodeStructure);
        }



    }
}