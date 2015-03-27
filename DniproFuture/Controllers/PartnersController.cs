using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        private DniproFuture_siteEntities db = new DniproFuture_siteEntities();

        // GET: Partners
        public ActionResult Index()
        {
            return View(db.Partners.ToList());
        }

        // GET: Partners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // GET: Partners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Logo")] Partners partners)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Add(partners);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partners);
        }

        // GET: Partners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // POST: Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Logo")] Partners partners)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partners).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partners);
        }

        // GET: Partners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partners partners = db.Partners.Find(id);
            db.Partners.Remove(partners);
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
