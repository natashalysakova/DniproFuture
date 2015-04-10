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
    public class NewsController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        public ActionResult Index(int? page)
        {
            var news = _repository.GetQueryOfNews();
            int pageNumber = page ?? 1;
            var onePageView = news.ToPagedList(pageNumber, 25);
            return View(onePageView);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = _repository.FindInNewsById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        public ActionResult Create()
        {
            ViewBag.Languages = _repository.GetLanguagesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(NewsInputModel news, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/News");
                var photosList = images.GetPhotosList(path);

                _repository.AddNews(news, photosList);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(news);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = _repository.FindInNewsById(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(News news, HttpPostedFileBase[] newPhotos, OldPhotoModel[] oldPhotos)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Content/img/News");
                var photosList = newPhotos.GetPhotosList(path, oldPhotos);

                _repository.EditNews(news, photosList, oldPhotos);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(news);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = _repository.FindInNewsById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string fullPath = Request.MapPath("~/Content/img/News");
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