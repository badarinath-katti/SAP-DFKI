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
    public class BOMDetailsController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: BOMDetails
        public ActionResult Index()
        {
            var mE_BOM_DETAILS = db.ME_BOM_DETAILS.Include(m => m.ME_BOM).Include(m => m.ME_MATERIAL).Include(m => m.ME_OPERATION);
            return View(mE_BOM_DETAILS.ToList());
        }

        // GET: BOMDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_BOM_DETAILS mE_BOM_DETAILS = db.ME_BOM_DETAILS.Find(id);
            if (mE_BOM_DETAILS == null)
            {
                return HttpNotFound();
            }
            return View(mE_BOM_DETAILS);
        }

        // GET: BOMDetails/Create
        public ActionResult Create(string selectedID)
        {
            ViewBag.SelectedID = selectedID;
            ViewBag.BOM = new SelectList(db.ME_BOM.Where(bom => bom.ID.Equals(selectedID)).ToList<ME_BOM>(), "ID", "NAME");
            ViewBag.COMPONENT = new SelectList(db.ME_MATERIAL, "ID", "NAME");
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME");
            return View();
        }

        // POST: BOMDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BOM,SEQUENCE,COMPONENT,OPERATION,ASSEMBLY_QUANTITY")] ME_BOM_DETAILS mE_BOM_DETAILS)
        {
            if (ModelState.IsValid)
            {
                db.ME_BOM_DETAILS.Add(mE_BOM_DETAILS);
                db.SaveChanges();
                return RedirectToAction("Details", "BOM", new { id= mE_BOM_DETAILS.BOM});
            }

            ViewBag.BOM_ID = new SelectList(db.ME_BOM, "ID", "NAME", mE_BOM_DETAILS.BOM);
            ViewBag.COMPONENT = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_BOM_DETAILS.COMPONENT);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_BOM_DETAILS.OPERATION);
            return View(mE_BOM_DETAILS);
        }

        // GET: BOMDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_BOM_DETAILS mE_BOM_DETAILS = db.ME_BOM_DETAILS.Find(id);
            if (mE_BOM_DETAILS == null)
            {
                return HttpNotFound();
            }
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_BOM_DETAILS.BOM);
            ViewBag.COMPONENT = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_BOM_DETAILS.COMPONENT);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_BOM_DETAILS.OPERATION);
            return View(mE_BOM_DETAILS);
        }

        // POST: BOMDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BOM,SEQUENCE,COMPONENT,OPERATION,ASSEMBLY_QUANTITY")] ME_BOM_DETAILS mE_BOM_DETAILS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_BOM_DETAILS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "BOM", new { id = mE_BOM_DETAILS.BOM});
            }
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_BOM_DETAILS.BOM);
            ViewBag.COMPONENT = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_BOM_DETAILS.COMPONENT);
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME", mE_BOM_DETAILS.OPERATION);
            return View(mE_BOM_DETAILS);
        }

        // GET: BOMDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_BOM_DETAILS mE_BOM_DETAILS = db.ME_BOM_DETAILS.Find(id);
            if (mE_BOM_DETAILS == null)
            {
                return HttpNotFound();
            }
            return View(mE_BOM_DETAILS);
        }

        // POST: BOMDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ME_BOM_DETAILS mE_BOM_DETAILS = db.ME_BOM_DETAILS.Find(id);
            string selectedID = mE_BOM_DETAILS.BOM;
            db.ME_BOM_DETAILS.Remove(mE_BOM_DETAILS);
            db.SaveChanges();
            return RedirectToAction("Details", "BOM", new { id = selectedID });
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
