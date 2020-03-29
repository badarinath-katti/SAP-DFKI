using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Manufacturing_Execution_Simulation.Models;

namespace Manufacturing_Execution_Simulation.Controllers
{
    public class RoutingDetailsController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: RoutingDetails
        public ActionResult Index()
        {
            var mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Include(m => m.ME_OPERATION).Include(m => m.ME_OPERATION1).Include(m => m.ME_OPERATION_TYPE).Include(m => m.ME_ROUTING);
            return View(mE_ROUTING_DETAILS.ToList());
        }

        // GET: RoutingDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING_DETAILS mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Find(id);
            if (mE_ROUTING_DETAILS == null)
            {
                return HttpNotFound();
            }
            return View(mE_ROUTING_DETAILS);
        }

        // GET: RoutingDetails/Create
        public ActionResult Create(string selectedID)
        {
            ViewBag.SelectedID = selectedID;
            ViewBag.NC_OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME");
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME");
            ViewBag.NEXT_OPERATION = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE");
            ViewBag.SEMANTIC_ANNOTATION = new SelectList(db.ME_SEMANTIC_ANNOTATION, "ID", "IRI");
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING.Where(routing => routing.ID.Equals(selectedID)), "ID", "NAME");
            return View();
        }

        // POST: RoutingDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ROUTING,SEQUENCE,OPERATION,SEMANTIC_ANNOTATION,CONDITION,NEXT_OPERATION,NC_OPERATION")] ME_ROUTING_DETAILS mE_ROUTING_DETAILS)
        {
            if (ModelState.IsValid)
            {
                db.ME_ROUTING_DETAILS.Add(mE_ROUTING_DETAILS);
                db.SaveChanges();
                return RedirectToAction("Details", "Routing", new { id = mE_ROUTING_DETAILS.ROUTING });
            }

            ViewBag.NC_OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.NC_OPERATION);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.OPERATION);
            ViewBag.NEXT_OPERATION = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_ROUTING_DETAILS.NEXT_OPERATION);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_ROUTING_DETAILS.ROUTING);
            return View(mE_ROUTING_DETAILS);
        }

        // GET: RoutingDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING_DETAILS mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Find(id);
            if (mE_ROUTING_DETAILS == null)
            {
                return HttpNotFound();
            }
            ViewBag.NC_OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.NC_OPERATION);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.OPERATION);
            ViewBag.NEXT_OPERATION = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_ROUTING_DETAILS.NEXT_OPERATION);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_ROUTING_DETAILS.ROUTING);
            ViewBag.SEMANTIC_ANNOTATION = new SelectList(db.ME_SEMANTIC_ANNOTATION, "ID", "IRI", mE_ROUTING_DETAILS.SEMANTIC_ANNOTATION);
            return View(mE_ROUTING_DETAILS);
        }

        // POST: RoutingDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ROUTING,SEQUENCE,OPERATION,SEMANTIC_ANNOTATION,CONDITION,NEXT_OPERATION,NC_OPERATION")] ME_ROUTING_DETAILS mE_ROUTING_DETAILS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_ROUTING_DETAILS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Routing", new { id = mE_ROUTING_DETAILS.ROUTING });
            }
            ViewBag.NC_OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.NC_OPERATION);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_ROUTING_DETAILS.OPERATION);
            ViewBag.NEXT_OPERATION = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_ROUTING_DETAILS.NEXT_OPERATION);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_ROUTING_DETAILS.ROUTING);
            return View(mE_ROUTING_DETAILS);
        }

        // GET: RoutingDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING_DETAILS mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Find(id);
            if (mE_ROUTING_DETAILS == null)
            {
                return HttpNotFound();
            }
            return View(mE_ROUTING_DETAILS);
        }

        // POST: RoutingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ME_ROUTING_DETAILS mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Find(id);
            string selectedID = mE_ROUTING_DETAILS.ROUTING;
            db.ME_ROUTING_DETAILS.Remove(mE_ROUTING_DETAILS);
            db.SaveChanges();
            return RedirectToAction("Details", "Routing", new { id = selectedID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
