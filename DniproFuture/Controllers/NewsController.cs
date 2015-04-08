using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.Repository;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        // GET: News
        public ActionResult Index()
        {
            return View(_repository.GetListOfNews());
        }

        // GET: News/Details/5
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

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.Languages = _repository.GetLanguagesList();
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                        try
                        {

                            if (photosList.Count == 0)
                            {
                                photo.CropAndSave(path);
                                Response.Write("Done");
                            }
                            else
                            {
                                photo.SaveAs(path);
                            }
                        }
                        catch (FormatException ex)
                        {
                            return RedirectToAction("Error", ex);
                        }
                        photosList.Add(filename);

                        //return RedirectToAction("Error", "Home", path);

                    }
                }

                if (photosList.Count > 0)
                    news.NewsInfo.Images = String.Join(";", photosList);
                else
                    news.NewsInfo.Images = String.Empty;

                _repository.AddNews(news);
                return RedirectToAction("Index");
            }

            ViewBag.Languages = _repository.GetLanguagesList();
            return View(news);
        }

        // GET: News/Edit/5
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

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: News/Delete/5
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

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string fullPath = Request.MapPath("~/Content/img/News");
            _repository.RemoveNewsById(id, fullPath);
            return RedirectToAction("Index");
        }

        public ActionResult Error(string ex)
        {
            ViewBag.Error = ex;
            return View();
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