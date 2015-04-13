using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class ProjectsController : Controller
    {
        private DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        // GET: Projects
        public ActionResult Index(int? page)
        {
            var news = _repository.GetQueryOfProjects();
            int pageNumber = page ?? 1;
            var onePageView = news.ToPagedList(pageNumber, 25);
            return View(onePageView);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.Languages = _repository.GetLanguagesList();
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult Create(ProjectInputModel projects, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/Projects");
                var photosList = photos.GetPhotosList(path);

                _repository.AddProject(projects, photosList);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = _repository.FindProjectById(id);
            if (projects == null)
            {
                return HttpNotFound();
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult Edit(Projects projects, HttpPostedFileBase[] newPhotos, OldPhotoModel[] oldPhotos)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/Projects");
                var photosList = newPhotos.GetPhotosList(path, oldPhotos);

                _repository.EditProject(projects, photosList, oldPhotos);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = _repository.FindProjectById(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string fullPath = Request.MapPath("~/Content/img/Projects");
            _repository.RemoveNewsById(id, fullPath);
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
