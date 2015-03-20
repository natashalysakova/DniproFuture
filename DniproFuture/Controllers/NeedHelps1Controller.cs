﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;

namespace DniproFuture.Controllers
{
    public class NeedHelps1Controller : Controller
    {
        readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        // GET: NeedHelps1
        public ActionResult Index()
        {

            return View(_repository.GetListOfNeedHelp());
        }

        // GET: NeedHelps1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NeedHelp needHelp = _repository.FindInNeedHelpById(id);
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
        public ActionResult Create(NeedHelpInputModel whoNeedHelp, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                List<string> photosList = new List<string>();
                foreach (HttpPostedFileBase photo in photos)
                {
                    if (photo != null)
                    {
                       string filename = Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] +"." + photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        var path = Path.Combine(Server.MapPath("~/Content/img/NeedHelp"), filename);
                        photo.SaveAs(path);
                        photosList.Add(filename);

                    }
                }

                whoNeedHelp.WhatNeed.Photos = String.Join(";", photosList);

                _repository.AddNeedHelp(whoNeedHelp);
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
            NeedHelp needHelp = _repository.FindInNeedHelpById(id);
            if (needHelp == null)
            {
                return HttpNotFound();
            }
            return View(needHelp);
        }

        // POST: NeedHelps1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sum,NeedSum,Photos,Done,Birthday,StartDate,FinishDate")] NeedHelp needHelp)
        {
            if (ModelState.IsValid)
            {
                _repository.EditNeedHelp(needHelp);
                return RedirectToAction("Index");
            }
            return View(needHelp);
        }

        // GET: NeedHelps1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NeedHelp needHelp = _repository.FindInNeedHelpById(id);
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
            _repository.RemoveNeedHelpById(id);
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