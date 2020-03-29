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
    public class ShopOrderController : Controller
    {
        private ManufacturingExecutionEntities db = new ManufacturingExecutionEntities();

        // GET: ShopOrder
        public ActionResult Index()
        {
            var mE_SHOP_ORDER = db.ME_SHOP_ORDER.Include(m => m.ME_BOM).Include(m => m.ME_MATERIAL).Include(m => m.ME_ROUTING).Include(m => m.ME_SHOP_ORDER_TYPE).Include(m => m.ME_SHOP_ORDER_STATUS);
            return View(mE_SHOP_ORDER.ToList());
        }

        // GET: ShopOrder/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_SHOP_ORDER mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(id);
            if (mE_SHOP_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(mE_SHOP_ORDER);
        }

        // GET: ShopOrder/Create
        public ActionResult Create()
        {
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME");
            ViewBag.PLANNED_MATERIAL = new SelectList(db.ME_MATERIAL, "ID", "NAME");
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME");
            ViewBag.TYPE = new SelectList(db.ME_SHOP_ORDER_TYPE, "ID", "NAME");
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS.Where(s => !s.NAME.Equals("Released")), "ID", "NAME");
            return View();
        }

        // POST: ShopOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,TYPE,PLANNED_MATERIAL,BOM,ROUTING,BUILD_QUANTITY,CUSTOMER_ORDER,PLANNED_START,PLANNED_COMPLETION,PRIORITY,RELEASE_QUANTITY")] ME_SHOP_ORDER mE_SHOP_ORDER)
        {
            if (ModelState.IsValid)
            {
                mE_SHOP_ORDER.ID = Guid.NewGuid().ToString();
                db.ME_SHOP_ORDER.Add(mE_SHOP_ORDER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_SHOP_ORDER.BOM);
            ViewBag.PLANNED_MATERIAL = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_SHOP_ORDER.PLANNED_MATERIAL);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_SHOP_ORDER.ROUTING);
            ViewBag.TYPE = new SelectList(db.ME_SHOP_ORDER_TYPE, "ID", "NAME", mE_SHOP_ORDER.TYPE);
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS, "ID", "NAME", mE_SHOP_ORDER.STATUS);
            return View(mE_SHOP_ORDER);
        }

        // GET: ShopOrder/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_SHOP_ORDER mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(id);
            if (mE_SHOP_ORDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_SHOP_ORDER.BOM);
            ViewBag.PLANNED_MATERIAL = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_SHOP_ORDER.PLANNED_MATERIAL);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_SHOP_ORDER.ROUTING);
            ViewBag.TYPE = new SelectList(db.ME_SHOP_ORDER_TYPE, "ID", "NAME", mE_SHOP_ORDER.TYPE);
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS.Where(s => !s.NAME.Equals("Released")),
                "ID", "NAME", mE_SHOP_ORDER.STATUS);
            return View(mE_SHOP_ORDER);
        }

        // POST: ShopOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,STATUS,TYPE,PLANNED_MATERIAL,BOM,ROUTING,BUILD_QUANTITY,CUSTOMER_ORDER,TRIMMED_PLANNED_START,TRIMMED_PLANNED_COMPLETION,PRIORITY,RELEASE_QUANTITY")] ME_SHOP_ORDER mE_SHOP_ORDER)
        {
            if (ModelState.IsValid)
            {
                mE_SHOP_ORDER.PLANNED_START = DateTime.ParseExact(mE_SHOP_ORDER.TRIMMED_PLANNED_START, 
                    "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                mE_SHOP_ORDER.PLANNED_COMPLETION = DateTime.ParseExact(mE_SHOP_ORDER.TRIMMED_PLANNED_COMPLETION,
                    "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                db.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_SHOP_ORDER.BOM);
            ViewBag.PLANNED_MATERIAL = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_SHOP_ORDER.PLANNED_MATERIAL);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_SHOP_ORDER.ROUTING);
            ViewBag.TYPE = new SelectList(db.ME_SHOP_ORDER_TYPE, "ID", "NAME", mE_SHOP_ORDER.TYPE);            
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS.Where(s => !s.NAME.Equals("Released")),
                "ID", "NAME", mE_SHOP_ORDER.STATUS);
            return View(mE_SHOP_ORDER);
        }

        // GET: ShopOrder/Edit/5
        public ActionResult ReleaseSFC(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_SHOP_ORDER mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(id);
            if (mE_SHOP_ORDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS.Where(s => s.NAME.Equals("Released")),
                "ID", "NAME", mE_SHOP_ORDER.STATUS);
            return View(mE_SHOP_ORDER);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReleaseSFC([Bind(Include = "ID,NAME,STATUS")] ME_SHOP_ORDER mE_SHOP_ORDER)
        {
            if (ModelState.IsValid)
            {
                int status = mE_SHOP_ORDER.STATUS;
                mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(mE_SHOP_ORDER.ID);
                mE_SHOP_ORDER.STATUS = status;
                db.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BOM = new SelectList(db.ME_BOM, "ID", "NAME", mE_SHOP_ORDER.BOM);
            ViewBag.PLANNED_MATERIAL = new SelectList(db.ME_MATERIAL, "ID", "NAME", mE_SHOP_ORDER.PLANNED_MATERIAL);
            ViewBag.ROUTING = new SelectList(db.ME_ROUTING, "ID", "NAME", mE_SHOP_ORDER.ROUTING);
            ViewBag.TYPE = new SelectList(db.ME_SHOP_ORDER_TYPE, "ID", "NAME", mE_SHOP_ORDER.TYPE);
            ViewBag.STATUS = new SelectList(db.ME_SHOP_ORDER_STATUS.Where(s => s.NAME.Equals("Released")),
                "ID", "NAME", mE_SHOP_ORDER.STATUS);
            return View(mE_SHOP_ORDER);
        }

        // GET: ShopOrder/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ME_SHOP_ORDER mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(id);
            if (mE_SHOP_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(mE_SHOP_ORDER);
        }

        // POST: ShopOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ME_SHOP_ORDER mE_SHOP_ORDER = db.ME_SHOP_ORDER.Find(id);
            db.ME_SHOP_ORDER.Remove(mE_SHOP_ORDER);
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
