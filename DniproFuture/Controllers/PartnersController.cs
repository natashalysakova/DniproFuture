using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.Repository;
using PagedList;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        private DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        // GET: Partners
        public ActionResult Index(int? page)
        {
            var partners = _repository.GetQueryOfPartners();
            var pageNumber = page ?? 1;
            var pagedPartners = partners.ToPagedList(pageNumber, 20);
            return View(pagedPartners);
        }

        // GET: Partners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = _repository.FindPartnerById(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // GET: Partners/Create
        public ActionResult Create()
        {
            ViewBag.Languages = _repository.GetLanguagesList();
            return View();
        }

        // POST: Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartnersInputModel partners, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    var filename =
                        Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + "." +
                        photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var path = Path.Combine(Server.MapPath("~/Content/img/Partners"), filename);
                    photo.SaveAs(path);
                    partners.MainInfo.Logo = filename;
                }

                _repository.AddPartner(partners);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(partners);
        }

        // GET: Partners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = _repository.FindPartnerById(id);
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
                _repository.EditPartner(partners);
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
            Partners partners = _repository.FindPartnerById(id);
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
            string fullPath = Request.MapPath("~/Content/img/Projects");
            _repository.RemovePartnerById(id, fullPath);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
