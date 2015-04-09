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

namespace DniproFuture.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        public ActionResult Index()
        {
            return View(_repository.GetListOfNews());
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
                var photosList = new List<string>();
                foreach (var photo in images)
                {
                    if (photo != null)
                    {
                        string[] splited = photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        string extention = splited.Last();
                        var filename =
                            Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + "." +
                            extention;
                        var path = Path.Combine(Server.MapPath("~/Content/img/News"), filename);

                        if (photosList.Count == 0)
                        {
                            photo.CropAndSave(path);
                            Response.Write("Done");
                        }
                        else
                        {
                            photo.SaveAs(path);
                        }

                        photosList.Add(filename);
                    }
                }

                if(photosList.Count > 0)
                    news.NewsInfo.Images = String.Join(";", photosList);
                else
                    news.NewsInfo.Images = String.Empty;

                _repository.AddNews(news);
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
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                _repository.EditNews(news);
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