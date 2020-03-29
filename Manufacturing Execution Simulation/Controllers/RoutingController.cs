using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Manufacturing_Execution_Simulation.Models;
using Manufacturing_Execution_Simulation.ViewModels;
using PagedList;

namespace Manufacturing_Execution_Simulation.Controllers
{
    public class RoutingController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: Routing
        public ActionResult Index()
        {
            var mE_ROUTING = db.ME_ROUTING.Include(m => m.ME_ROUTING_STATUS).Include(m => m.ME_ROUTING_TYPE);
            return View(mE_ROUTING.ToList());
        }

        // GET: Routing/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING mE_ROUTING = db.ME_ROUTING.Find(id);
            if (mE_ROUTING == null)
            {
                return HttpNotFound();
            }
            ViewModelRouting viewModelRouting = new ViewModelRouting();
            viewModelRouting.mE_ROUTING = mE_ROUTING;
            viewModelRouting.mE_ROUTING_DETAILS = db.ME_ROUTING_DETAILS.Where(routing_details
                => routing_details.ROUTING.Equals(mE_ROUTING.ID)).OrderBy(detail => detail.SEQUENCE).ToPagedList(page ?? 1, 5);
            return View(viewModelRouting);
        }

        // GET: Routing/Create
        public ActionResult Create()
        {
            ViewBag.STATUS = new SelectList(db.ME_ROUTING_STATUS, "ID", "NAME");
            ViewBag.TYPE = new SelectList(db.ME_ROUTING_TYPE, "ID", "NAME");
            return View();
        }

        // POST: Routing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,TYPE,STATUS")] ME_ROUTING mE_ROUTING)
        {
            if (ModelState.IsValid)
            {
                mE_ROUTING.ID = Guid.NewGuid().ToString();
                db.ME_ROUTING.Add(mE_ROUTING);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STATUS = new SelectList(db.ME_ROUTING_STATUS, "ID", "NAME", mE_ROUTING.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_ROUTING_TYPE, "ID", "NAME", mE_ROUTING.TYPE);
            return View(mE_ROUTING);
        }

        // GET: Routing/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING mE_ROUTING = db.ME_ROUTING.Find(id);
            if (mE_ROUTING == null)
            {
                return HttpNotFound();
            }
            ViewBag.STATUS = new SelectList(db.ME_ROUTING_STATUS, "ID", "NAME", mE_ROUTING.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_ROUTING_TYPE, "ID", "NAME", mE_ROUTING.TYPE);
            return View(mE_ROUTING);
        }

        // POST: Routing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,TYPE,STATUS")] ME_ROUTING mE_ROUTING)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_ROUTING).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STATUS = new SelectList(db.ME_ROUTING_STATUS, "ID", "NAME", mE_ROUTING.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_ROUTING_TYPE, "ID", "NAME", mE_ROUTING.TYPE);
            return View(mE_ROUTING);
        }

        // GET: Routing/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_ROUTING mE_ROUTING = db.ME_ROUTING.Find(id);
            if (mE_ROUTING == null)
            {
                return HttpNotFound();
            }
            return View(mE_ROUTING);
        }

        // POST: Routing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_ROUTING mE_ROUTING = db.ME_ROUTING.Find(id);
            db.ME_ROUTING.Remove(mE_ROUTING);
            db.SaveChanges();
            return RedirectToAction("Index");
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
