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

namespace Manufacturing_Execution_Simulation.Controllers
{
    public class BOMController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: BOM
        public ActionResult Index()
        {
            return View(db.ME_BOM.ToList());
        }

        public JsonResult GetBOMs(string term)
        {
            List<string> mE_BOMS = null;
            mE_BOMS = db.ME_BOM.Where(bom => bom.NAME.StartsWith(term)).Select(bom => bom.NAME).ToList();
            return Json(mE_BOMS, JsonRequestBehavior.AllowGet);
        }

        // GET: BOM/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ME_BOM mE_BOM = db.ME_BOM.Find(id);
            if (mE_BOM == null)
            {
                return HttpNotFound();
            }
            ViewModelBOM viewModelBOM = new ViewModelBOM();
            viewModelBOM.mE_BOM = mE_BOM;
            viewModelBOM.mE_BOM_DETAILS = db.ME_BOM_DETAILS.Where(bom_detail => bom_detail.BOM.Equals(mE_BOM.ID)).OrderBy(me_detail => me_detail.SEQUENCE);
            return View(viewModelBOM);
        }

        //Partial view action
        public ActionResult CreateDetailTemplate()
        {
            ViewBag.BOM_ID = new SelectList(db.ME_BOM, "ID", "NAME");
            ViewBag.COMPONENT = new SelectList(db.ME_MATERIAL, "ID", "NAME");
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME");
            return View();
        }

        // GET: BOM/Create
        public ActionResult Create()
        {
            ViewBag.STATUS = new SelectList(db.ME_BOM_STATUS, "ID", "NAME");
            return View();
        }

        // POST: BOM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,STATUS")] ME_BOM mE_BOM)
        {
            if (ModelState.IsValid)
            {
                mE_BOM.ID = Guid.NewGuid().ToString();
                db.ME_BOM.Add(mE_BOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mE_BOM);
        }

        // GET: BOM/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_BOM mE_BOM = db.ME_BOM.Find(id);
            if (mE_BOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.STATUS = new SelectList(db.ME_BOM_STATUS, "ID", "NAME", mE_BOM.STATUS);
            return View(mE_BOM);
        }

        // POST: BOM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,STATUS")] ME_BOM mE_BOM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_BOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mE_BOM);
        }

        // GET: BOM/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_BOM mE_BOM = db.ME_BOM.Find(id);
            if (mE_BOM == null)
            {
                return HttpNotFound();
            }
            return View(mE_BOM);
        }

        // POST: BOM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_BOM mE_BOM = db.ME_BOM.Find(id);
            db.ME_BOM.Remove(mE_BOM);
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
