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
    public class WorkCenterController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: WorkCenter
        public ActionResult Index()
        {
            var mE_WORKCENTER = db.ME_WORKCENTER.Include(m => m.ME_WORKCENTER_CATEGORY).Include(m => m.ME_WORKCENTER_STATUS).Include(m => m.ME_WORKCENTER_TYPE);
            return View(mE_WORKCENTER.ToList());
        }

        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            IQueryable<ME_WORKCENTER> mE_WORKCENTER = null;
            if (!string.IsNullOrEmpty(txtSearch))
                mE_WORKCENTER = db.ME_WORKCENTER.Include(m => m.ME_WORKCENTER_CATEGORY).Include(m => m.ME_WORKCENTER_STATUS).Include(m => m.ME_WORKCENTER_TYPE)
                    .Where(workcenter => workcenter.NAME.StartsWith(txtSearch));
            else
                mE_WORKCENTER = db.ME_WORKCENTER.Include(m => m.ME_WORKCENTER_CATEGORY).Include(m => m.ME_WORKCENTER_STATUS).Include(m => m.ME_WORKCENTER_TYPE);
            return View(mE_WORKCENTER.ToList());
        }

        public JsonResult GetWorkCenters(string term)
        {
            List<string> mE_WORKCENTERS = null;
            mE_WORKCENTERS = db.ME_WORKCENTER.Include(m => m.ME_WORKCENTER_CATEGORY).Include(m => m.ME_WORKCENTER_STATUS).Include(m => m.ME_WORKCENTER_TYPE)
                .Where(workcenter_resource => workcenter_resource.NAME.StartsWith(term))
                .Select(workcenter => workcenter.NAME).ToList();
            return Json(mE_WORKCENTERS, JsonRequestBehavior.AllowGet);
        }

        // GET: WorkCenter/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER mE_WORKCENTER = db.ME_WORKCENTER.Find(id);
            if (mE_WORKCENTER == null)
            {
                return HttpNotFound();
            }
            ViewModelWorkCenter viewModelWorkCenter = new ViewModelWorkCenter();
            viewModelWorkCenter.mE_WORKCENTER = mE_WORKCENTER;
            viewModelWorkCenter.mE_WORKCENTER_RESOURCES = db.ME_WORKCENTER_RESOURCE.
                Where(workcenter_resource => workcenter_resource.WORKCENTER_ID.Equals(mE_WORKCENTER.ID));
            return View(viewModelWorkCenter);
        }

        // GET: WorkCenter/Create
        public ActionResult Create()
        {
            ViewBag.CATEGORY = new SelectList(db.ME_WORKCENTER_CATEGORY, "ID", "NAME");
            ViewBag.STATUS = new SelectList(db.ME_WORKCENTER_STATUS, "ID", "NAME");
            ViewBag.TYPE = new SelectList(db.ME_WORKCENTER_TYPE, "ID", "NAME");
            return View();
        }

        // POST: WorkCenter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,CATEGORY,TYPE")] ME_WORKCENTER mE_WORKCENTER)
        {
            if (ModelState.IsValid)
            {
                mE_WORKCENTER.ID = Guid.NewGuid().ToString();
                db.ME_WORKCENTER.Add(mE_WORKCENTER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CATEGORY = new SelectList(db.ME_WORKCENTER_CATEGORY, "ID", "NAME", mE_WORKCENTER.CATEGORY);
            ViewBag.STATUS = new SelectList(db.ME_WORKCENTER_STATUS, "ID", "NAME", mE_WORKCENTER.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_WORKCENTER_TYPE, "ID", "NAME", mE_WORKCENTER.TYPE);
            return View(mE_WORKCENTER);
        }

        // GET: WorkCenter/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER mE_WORKCENTER = db.ME_WORKCENTER.Find(id);
            if (mE_WORKCENTER == null)
            {
                return HttpNotFound();
            }
            ViewBag.CATEGORY = new SelectList(db.ME_WORKCENTER_CATEGORY, "ID", "NAME", mE_WORKCENTER.CATEGORY);
            ViewBag.STATUS = new SelectList(db.ME_WORKCENTER_STATUS, "ID", "NAME", mE_WORKCENTER.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_WORKCENTER_TYPE, "ID", "NAME", mE_WORKCENTER.TYPE);
            return View(mE_WORKCENTER);
        }

        // POST: WorkCenter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,CATEGORY,TYPE")] ME_WORKCENTER mE_WORKCENTER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_WORKCENTER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CATEGORY = new SelectList(db.ME_WORKCENTER_CATEGORY, "ID", "NAME", mE_WORKCENTER.CATEGORY);
            ViewBag.STATUS = new SelectList(db.ME_WORKCENTER_STATUS, "ID", "NAME", mE_WORKCENTER.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_WORKCENTER_TYPE, "ID", "NAME", mE_WORKCENTER.TYPE);
            return View(mE_WORKCENTER);
        }

        // GET: WorkCenter/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_WORKCENTER mE_WORKCENTER = db.ME_WORKCENTER.Find(id);
            if (mE_WORKCENTER == null)
            {
                return HttpNotFound();
            }
            return View(mE_WORKCENTER);
        }

        // POST: WorkCenter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_WORKCENTER mE_WORKCENTER = db.ME_WORKCENTER.Find(id);
            db.ME_WORKCENTER.Remove(mE_WORKCENTER);
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
