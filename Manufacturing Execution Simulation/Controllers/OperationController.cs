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
    public class OperationController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: Operation
        public ActionResult Index()
        {
            var mE_OPERATION = db.ME_OPERATION.Include(m => m.ME_OPERATION_STATUS).Include(m => m.ME_WORKCENTER);
            return View(mE_OPERATION.ToList());
        }

        public JsonResult GetOperations(string term)
        {
            List<string> mE_OPERATION = db.ME_OPERATION.Include(m => m.ME_OPERATION_STATUS).Include(m => m.ME_WORKCENTER)
                .Where(operation => operation.NAME.StartsWith(term))
                .Select(operation => operation.NAME).ToList();
            return Json(mE_OPERATION, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            IQueryable<ME_OPERATION> mE_OPERATION = null;
            if (string.IsNullOrEmpty(txtSearch))
                mE_OPERATION = db.ME_OPERATION.Include(m => m.ME_OPERATION_STATUS).Include(m => m.ME_WORKCENTER);
            else
                mE_OPERATION = db.ME_OPERATION.Include(m => m.ME_OPERATION_STATUS).Include(m => m.ME_WORKCENTER)
                    .Where(operation => operation.NAME.StartsWith(txtSearch));
            return View(mE_OPERATION.ToList());
        }

        // GET: Operation/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_OPERATION mE_OPERATION = db.ME_OPERATION.Find(id);
            if (mE_OPERATION == null)
            {
                return HttpNotFound();
            }
            return View(mE_OPERATION);
        }

        // GET: Operation/Create
        public ActionResult Create()
        {
            ViewBag.STATUS = new SelectList(db.ME_OPERATION_STATUS, "ID", "NAME");
            ViewBag.WORKCENTER = new SelectList(db.ME_WORKCENTER, "ID", "NAME");
            ViewBag.RESOURCE = new SelectList(db.ME_RESOURCE, "ID", "NAME");
            ViewBag.RESOURCETYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE");
            ViewBag.ACTIVITY = new SelectList(db.ME_ACTIVITY, "ID", "NAME");
            ViewBag.OPERATIONTYPE = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE");
            return View();
        }

        public ActionResult GetFilteredResources(string workCenterID)
        {
            List<SelectListItem> filteredResourceItems = new List<SelectListItem>();
            List<ME_RESOURCE> filteredResources = null;
            if (!string.IsNullOrEmpty(workCenterID))
            {
                List<string> filteredResourcesIDs = db.ME_WORKCENTER_RESOURCE
                    .Where(workcenter_resource => workcenter_resource.WORKCENTER_ID.Equals(workCenterID))
                    .Select(workcenter_resource => workcenter_resource.RESOURCE_ID).ToList<string>();

                filteredResources = db.ME_RESOURCE.Where(res =>
                    filteredResourcesIDs.Any(resourceID => resourceID == res.ID)).ToList<ME_RESOURCE>();
            }
            else
            {
                filteredResources = db.ME_RESOURCE.ToList<ME_RESOURCE>();
            }
            foreach (ME_RESOURCE mE_RESOURCE in filteredResources)
                filteredResourceItems.Add(new SelectListItem { Text = mE_RESOURCE.NAME, Value = mE_RESOURCE.ID });

            return Json(filteredResourceItems, JsonRequestBehavior.AllowGet);
        }

        // POST: Operation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,OPERATIONTYPE,DESCRIPTION,STATUS,RESOURCE,WORKCENTER,ACTIVITY,RESOURCETYPE")] ME_OPERATION mE_OPERATION)
        {
            if (ModelState.IsValid)
            {
                mE_OPERATION.ID = Guid.NewGuid().ToString();
                db.ME_OPERATION.Add(mE_OPERATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STATUS = new SelectList(db.ME_OPERATION_STATUS, "ID", "NAME", mE_OPERATION.STATUS);
            ViewBag.WORKCENTER = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_OPERATION.WORKCENTER);
            ViewBag.RESOURCE = new SelectList(db.ME_RESOURCE, "ID", "NAME", mE_OPERATION.RESOURCE);
            ViewBag.RESOURCETYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_OPERATION.RESOURCETYPE);
            ViewBag.ACTIVITY = new SelectList(db.ME_ACTIVITY, "ID", "NAME", mE_OPERATION.ACTIVITY);
            ViewBag.OPERATIONTYPE = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_OPERATION.OPERATIONTYPE);
            return View(mE_OPERATION);
        }

        // GET: Operation/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_OPERATION mE_OPERATION = db.ME_OPERATION.Find(id);
            if (mE_OPERATION == null)
            {
                return HttpNotFound();
            }

            ViewBag.RESOURCE = new SelectList(db.ME_RESOURCE.Where(resource =>
            db.ME_WORKCENTER_RESOURCE.Any(workcenter_res => (workcenter_res.RESOURCE_ID == resource.ID && 
            mE_OPERATION.WORKCENTER == workcenter_res.WORKCENTER_ID))), "ID", "NAME", mE_OPERATION.ME_RESOURCE.NAME);

            ViewBag.RESOURCETYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_OPERATION.ME_RESOURCE_TYPE.ID);
            ViewBag.STATUS = new SelectList(db.ME_OPERATION_STATUS, "ID", "NAME", mE_OPERATION.STATUS);
            ViewBag.WORKCENTER = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_OPERATION.WORKCENTER);
            ViewBag.OPERATIONTYPE = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_OPERATION.OPERATIONTYPE);
            return View(mE_OPERATION);
        }

        [HttpPost]
        public JsonResult getResource(string operationID)
        {
            string resourceID = db.ME_OPERATION.Where(operation => operation.ID.Equals(operationID)).FirstOrDefault().RESOURCE;
            return Json(new { resourceID }, JsonRequestBehavior.AllowGet);
        }

        // POST: Operation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,OPERATIONTYPE,DESCRIPTION,STATUS,RESOURCE,WORKCENTER,ACTIVITY,RESOURCETYPE")] ME_OPERATION mE_OPERATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mE_OPERATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STATUS = new SelectList(db.ME_OPERATION_STATUS, "ID", "NAME", mE_OPERATION.STATUS);
            ViewBag.WORKCENTER = new SelectList(db.ME_WORKCENTER, "ID", "NAME", mE_OPERATION.WORKCENTER);
            ViewBag.RESOURCE = new SelectList(db.ME_RESOURCE, "ID", "NAME", mE_OPERATION.RESOURCE);
            ViewBag.RESOURCETYPE = new SelectList(db.ME_RESOURCE_TYPE, "ID", "TYPE", mE_OPERATION.ME_RESOURCE_TYPE);
            ViewBag.OPERATIONTYPE = new SelectList(db.ME_OPERATION_TYPE, "ID", "TYPE", mE_OPERATION.OPERATIONTYPE);
            return View(mE_OPERATION);
        }

        // GET: Operation/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_OPERATION mE_OPERATION = db.ME_OPERATION.Find(id);
            if (mE_OPERATION == null)
            {
                return HttpNotFound();
            }
            return View(mE_OPERATION);
        }

        // POST: Operation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_OPERATION mE_OPERATION = db.ME_OPERATION.Find(id);
            db.ME_OPERATION.Remove(mE_OPERATION);
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
