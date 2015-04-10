using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.Extentions;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.Repository;
using PagedList;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class NeedHelpsController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        // GET: NeedHelps1
        public ActionResult Index(int? page)
        {
            var help = _repository.GetQueryOfNeedHelp();
            var pageNumber = page ??  1;
            var pagedHelp = help.ToPagedList(pageNumber, 20);
            return View(pagedHelp);
        }

        // GET: NeedHelps1
        public ActionResult Done(int? page)
        {
            var help = _repository.GetQueryOfDone();
            var pageNumber = page ?? 1;
            var pagedHelp = help.ToPagedList(pageNumber, 20);
            return View(pagedHelp);
        }

        // GET: NeedHelps1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needHelp = _repository.FindInNeedHelpById(id);
            if (needHelp == null)
            {
                return HttpNotFound();
            }
            return View(needHelp);
        }

        // GET: NeedHelps1/Create
        public ActionResult Create()
        {
            ViewBag.Languages = _repository.GetLanguagesList();
            return View();
        }

        // POST: NeedHelps1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult Create(NeedHelpInputModel whoNeedHelp, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/NeedHelp");
                var photosList = photos.GetPhotosList(path);

                _repository.AddNeedHelp(whoNeedHelp, photosList);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(whoNeedHelp);
        }

        // GET: NeedHelps1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needHelp = _repository.FindInNeedHelpById(id);
            if (needHelp == null)
            {
                return HttpNotFound();
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(needHelp);
        }

        // POST: NeedHelps1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult Edit(NeedHelp needHelp, HttpPostedFileBase[] newPhotos, OldPhotoModel[] oldPhotos)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/NeedHelp");
                var photosList = newPhotos.GetPhotosList(path, oldPhotos);

                _repository.EditNeedHelp(needHelp, photosList, oldPhotos);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(needHelp);
        }

        // GET: NeedHelps1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needHelp = _repository.FindInNeedHelpById(id);
            if (needHelp == null)
            {
                return HttpNotFound();
            }
            return View(needHelp);
        }

        // POST: NeedHelps1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string fullPath = Request.MapPath("~/Content/img/NeedHelp");
            _repository.RemoveNeedHelpById(id, fullPath);
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