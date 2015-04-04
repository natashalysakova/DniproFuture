using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
        DniproFutureModelRepository _repository = new DniproFutureModelRepository();

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
            News news = _repository.FindInNewsById(id);
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
        public ActionResult Create(NewsInputModel news, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                List<string> photosList = new List<string>();
                foreach (HttpPostedFileBase photo in images)
                {
                    if (photo != null)
                    {
                        string filename = Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + "." + photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        var path = Path.Combine(Server.MapPath("~/Content/img/News"), filename);

                        
                        if (photosList.Count == 0)
                        {
                            try
                            {
                                photo.CropAndSave(path);
                                Response.Write("Done");

                            }
                            catch (FormatException ex)
                            {
                                Response.Write(ex.Message);
                            }
                        }
                        else
                        {
                            photo.SaveAs(path);
                        }
                        photosList.Add(filename);
                    }
                }

                news.NewsInfo.Images = String.Join(";", photosList);
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
            News news = _repository.FindInNewsById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Images,Date")] News news)
        {
            if (ModelState.IsValid)
            {
                _repository.EditNews(news);
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _repository.FindInNewsById(id);
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
            _repository.RemoveNewsById(id);
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
