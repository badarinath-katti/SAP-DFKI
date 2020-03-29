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
    public class MaterialController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: Material
        public ActionResult Index()
        {
            var mE_MATERIAL = db.ME_MATERIAL.Include(m => m.ME_MATERIAL_STATUS);
            return View(mE_MATERIAL.ToList());
        }
        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            IQueryable<ME_MATERIAL> mE_MATERIAL = null;
            if (!string.IsNullOrEmpty(txtSearch))
                mE_MATERIAL = db.ME_MATERIAL.Include(m => m.ME_MATERIAL_STATUS).Where(material => material.NAME.StartsWith(txtSearch));
            else
                mE_MATERIAL = db.ME_MATERIAL.Include(m => m.ME_MATERIAL_STATUS);
            return View(mE_MATERIAL.ToList());
        }

        public JsonResult GetMaterials(string term)
        {
            List<string> mE_MATERIALS = null;
            mE_MATERIALS = db.ME_MATERIAL.Include(m => m.ME_MATERIAL_STATUS).Where(material => material.NAME.StartsWith(term))
                .Select(material => material.NAME).ToList();
            return Json(mE_MATERIALS, JsonRequestBehavior.AllowGet);
        }

        // GET: Material/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_MATERIAL mE_MATERIAL = db.ME_MATERIAL.Find(id);
            if (mE_MATERIAL == null)
            {
                return HttpNotFound();
            }
            return View(mE_MATERIAL);
        }

        // GET: Material/Create
        public ActionResult Create()
        {
            ViewBag.STATUS = new SelectList(db.ME_MATERIAL_STATUS, "ID", "STATUS");
            ViewBag.TYPE = new SelectList(db.ME_MATERIAL_TYPE, "ID", "TYPE");
            return View();
        }

        // POST: Material/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,LOT_SIZE,TYPE")] ME_MATERIAL mE_MATERIAL, string Command)
        {
            if (ModelState.IsValid)
            {
                mE_MATERIAL.ID = Guid.NewGuid().ToString();
                db.ME_MATERIAL.Add(mE_MATERIAL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STATUS = new SelectList(db.ME_MATERIAL_STATUS, "ID", "STATUS", mE_MATERIAL.STATUS);
            return View(mE_MATERIAL);
        }

        // GET: Material/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_MATERIAL mE_MATERIAL = db.ME_MATERIAL.Find(id);
            if (mE_MATERIAL == null)
            {
                return HttpNotFound();
            }
            ViewBag.STATUS = new SelectList(db.ME_MATERIAL_STATUS, "ID", "STATUS", mE_MATERIAL.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_MATERIAL_TYPE, "ID", "TYPE", mE_MATERIAL.TYPE);
            return View(mE_MATERIAL);
        }

        // POST: Material/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,LOT_SIZE,TYPE")] ME_MATERIAL mE_MATERIAL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_MATERIAL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STATUS = new SelectList(db.ME_MATERIAL_STATUS, "ID", "STATUS", mE_MATERIAL.STATUS);
            return View(mE_MATERIAL);
        }

        // GET: Material/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_MATERIAL mE_MATERIAL = db.ME_MATERIAL.Find(id);
            if (mE_MATERIAL == null)
            {
                return HttpNotFound();
            }
            return View(mE_MATERIAL);
        }

        // POST: Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_MATERIAL mE_MATERIAL = db.ME_MATERIAL.Find(id);
            db.ME_MATERIAL.Remove(mE_MATERIAL);
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
