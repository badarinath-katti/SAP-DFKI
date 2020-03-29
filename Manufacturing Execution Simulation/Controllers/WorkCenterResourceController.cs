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
    public class WorkCenterResourceController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: WorkCenterResource
        public ActionResult Index()
        {
            IQueryable<ME_WORKCENTER_RESOURCE> mE_WORKCENTER_RESOURCE =
                 db.ME_WORKCENTER_RESOURCE.Include(m => m.ME_RESOURCE).Include(m => m.ME_WORKCENTER);            
            return View(mE_WORKCENTER_RESOURCE.ToList());
        }

        public JsonResult GetWorkCenters(string term)
        {
            var mE_WORKCENTERS = db.ME_WORKCENTER_RESOURCE.Include(m => m.ME_RESOURCE).Include(m => m.ME_WORKCENTER).
                Where(workcenter_resource => workcenter_resource.ME_WORKCENTER.NAME.StartsWith(term))
                .Select(workcenter_resource => workcenter_resource.ME_WORKCENTER.NAME).ToList(); ;
            return Json(mE_WORKCENTERS, JsonRequestBehavior.AllowGet);
        }

        // GET: WorkCenterResource/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE = db.ME_WORKCENTER_RESOURCE.Find(id);
            if (mE_WORKCENTER_RESOURCE == null)
            {
                return HttpNotFound();
            }
            return View(mE_WORKCENTER_RESOURCE);
        }

        // GET: WorkCenterResource/Create
        public ActionResult Create(string selectedID)
        {
            ViewBag.SelectedID = selectedID;
            ViewBag.RESOURCE_ID = new SelectList(db.ME_RESOURCE, "ID", "NAME");
            ViewBag.WORKCENTER_ID = new SelectList(db.ME_WORKCENTER.Where(workcenter => 
                workcenter.ID.Equals(selectedID)).ToList<ME_WORKCENTER>(), "ID", "NAME");
            return View();
        }

        // POST: WorkCenterResource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WORKCENTER_ID,RESOURCE_ID")] ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE)
        {
            if (ModelState.IsValid)
            {
                db.ME_WORKCENTER_RESOURCE.Add(mE_WORKCENTER_RESOURCE);
                db.SaveChanges();
                return RedirectToAction("Details", "WorkCenter", new { id = mE_WORKCENTER_RESOURCE.WORKCENTER_ID});
            }

            ViewBag.RESOURCE_ID = new SelectList(db.ME_RESOURCE, "ID", "NAME", mE_WORKCENTER_RESOURCE.RESOURCE_ID);
            ViewBag.WORKCENTER_ID = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_WORKCENTER_RESOURCE.WORKCENTER_ID);
            return View(mE_WORKCENTER_RESOURCE);
        }

        // GET: WorkCenterResource/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE = db.ME_WORKCENTER_RESOURCE.Find(id);
            if (mE_WORKCENTER_RESOURCE == null)
            {
                return HttpNotFound();
            }
            ViewBag.RESOURCE_ID = new SelectList(db.ME_RESOURCE, "ID", "NAME", mE_WORKCENTER_RESOURCE.RESOURCE_ID);
            ViewBag.WORKCENTER_ID = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_WORKCENTER_RESOURCE.WORKCENTER_ID);
            return View(mE_WORKCENTER_RESOURCE);
        }

        // POST: WorkCenterResource/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WORKCENTER_ID,RESOURCE_ID")] ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_WORKCENTER_RESOURCE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "WorkCenter", new { id = mE_WORKCENTER_RESOURCE.WORKCENTER_ID });
            }
            ViewBag.RESOURCE_ID = new SelectList(db.ME_RESOURCE, "ID", "NAME", mE_WORKCENTER_RESOURCE.RESOURCE_ID);
            ViewBag.WORKCENTER_ID = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_WORKCENTER_RESOURCE.WORKCENTER_ID);
            return View(mE_WORKCENTER_RESOURCE);
        }

        // GET: WorkCenterResource/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE = db.ME_WORKCENTER_RESOURCE.Find(id);
            if (mE_WORKCENTER_RESOURCE == null)
            {
                return HttpNotFound();
            }
            return View(mE_WORKCENTER_RESOURCE);
        }

        // POST: WorkCenterResource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            ME_WORKCENTER_RESOURCE mE_WORKCENTER_RESOURCE = db.ME_WORKCENTER_RESOURCE.Find(id);
            string selectedID = mE_WORKCENTER_RESOURCE.WORKCENTER_ID;
            db.ME_WORKCENTER_RESOURCE.Remove(mE_WORKCENTER_RESOURCE);
            db.SaveChanges();
            return RedirectToAction("Details", "WorkCenter", new { id = selectedID });
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
