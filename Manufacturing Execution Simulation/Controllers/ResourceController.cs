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
    public class ResourceController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: Resource
        public ActionResult Index()
        {
            var mE_RESOURCE = db.ME_RESOURCE.Include(m => m.ME_RESOURCE_STATUS).Include(m => m.ME_RESOURCE_TYPE);
            return View(mE_RESOURCE.ToList());
        }

        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            IQueryable<ME_RESOURCE> mE_RESOURCE = null;
            if (!string.IsNullOrEmpty(txtSearch))
                mE_RESOURCE = db.ME_RESOURCE.Include(m => m.ME_RESOURCE_STATUS).Include(m => m.ME_RESOURCE_TYPE)
                    .Where(resource => resource.NAME.StartsWith(txtSearch));
            else
                mE_RESOURCE = db.ME_RESOURCE.Include(m => m.ME_RESOURCE_STATUS).Include(m => m.ME_RESOURCE_TYPE);
            return View(mE_RESOURCE.ToList());
        }

        public JsonResult GetResources(string term)
        {
            List<string> mE_RESOURCE = null;
            mE_RESOURCE = db.ME_RESOURCE.Include(m => m.ME_RESOURCE_STATUS).Include(m => m.ME_RESOURCE_TYPE)
                    .Where(resource => resource.NAME.StartsWith(term)).Select(resource => resource.NAME).ToList<string>();
            return Json(mE_RESOURCE, JsonRequestBehavior.AllowGet);
        }

        // GET: Resource/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_RESOURCE mE_RESOURCE = db.ME_RESOURCE.Find(id);
            if (mE_RESOURCE == null)
            {
                return HttpNotFound();
            }
            return View(mE_RESOURCE);
        }

        // GET: Resource/Create
        public ActionResult Create()
        {
            ViewBag.STATUS = new SelectList(db.ME_RESOURCE_STATUS, "ID", "NAME");
            ViewBag.TYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE");
            ViewBag.OPERATION = new SelectList(db.ME_OPERATION, "ID", "NAME");
            return View();
        }

        // POST: Resource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,TYPE,OPERATION")] ME_RESOURCE mE_RESOURCE)
        {
            if (ModelState.IsValid)
            {
                mE_RESOURCE.ID = Guid.NewGuid().ToString();
                db.ME_RESOURCE.Add(mE_RESOURCE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STATUS = new SelectList(db.ME_RESOURCE_STATUS, "ID", "NAME", mE_RESOURCE.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_RESOURCE.TYPE);
            return View(mE_RESOURCE);
        }

        // GET: Resource/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_RESOURCE mE_RESOURCE = db.ME_RESOURCE.Find(id);
            if (mE_RESOURCE == null)
            {
                return HttpNotFound();
            }
            ViewBag.STATUS = new SelectList(db.ME_RESOURCE_STATUS, "ID", "NAME", mE_RESOURCE.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_RESOURCE.TYPE);
            return View(mE_RESOURCE);
        }

        // POST: Resource/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,TYPE,OPERATION")] ME_RESOURCE mE_RESOURCE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_RESOURCE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STATUS = new SelectList(db.ME_RESOURCE_STATUS, "ID", "NAME", mE_RESOURCE.STATUS);
            ViewBag.TYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_RESOURCE.TYPE);
            return View(mE_RESOURCE);
        }

        // GET: Resource/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_RESOURCE mE_RESOURCE = db.ME_RESOURCE.Find(id);
            if (mE_RESOURCE == null)
            {
                return HttpNotFound();
            }
            return View(mE_RESOURCE);
        }

        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_RESOURCE mE_RESOURCE = db.ME_RESOURCE.Find(id);
            db.ME_RESOURCE.Remove(mE_RESOURCE);
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
