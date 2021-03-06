﻿using System.Globalization;
using System.Net;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;
using DniproFuture.Models.Repository;
using PagedList;
using System.Linq;

namespace DniproFuture.Controllers
{
    public class HomeController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();



        public ActionResult Index()
        {
            FillViewBag();

            var model = _repository.GetMainPageModel();
            return View(model);
        }

        private void FillViewBag()
        {
            ViewBag.ClientsCount = _repository.ClientsCount;
            ViewBag.PartnersCount = _repository.PartnersCount;
            ViewBag.NewsCount = _repository.NewsCount;
            ViewBag.ProjectsCount = _repository.ProjectsCount;
            ViewBag.DonationCount = _repository.DonationCount;
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult Contact(ContactsInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mail = _repository.SendMessage(model);
                return View("Done", mail);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ContactAjax(ContactsInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mail = _repository.SendMessage(model);
                return Json(mail);
            }
            return Json("Undone");
        }

        [HttpPost]
        public ActionResult GetUnread()
        {
            var mail = _repository.GetUnreadMails();
            return Json(mail.Count);
        }

        [HttpPost]
        public ActionResult GetUnreadMailView(Mail mail)
        {
            return PartialView("UnreadMessage", mail);
        }

        public ActionResult NeedHelpIndex(int? page)
        {
            var products = _repository.GetQueryOfNeedHelpOutputModel();
            var pageNumber = page ?? 1;
            var onePageOfProducts = products.ToPagedList(pageNumber, 12);
            FillViewBag();
            return View(onePageOfProducts);
        }

        public ActionResult NewsIndex(int? page)
        {
            var news = _repository.GetQueryOfNewsOutputModel();
            var pageNumber = page ?? 1;
            var onePageOfNews = news.ToPagedList(pageNumber, 10);
            FillViewBag();
            return View(onePageOfNews);
        }

        public ActionResult ProjectIndex(int? page)
        {
            var news = _repository.GetQueryOfProjectsOutputModels();
            var pageNumber = page ?? 1;
            var onePageOfNews = news.ToPagedList(pageNumber, 10);
            FillViewBag();
            return View(onePageOfNews);
        }
        
        public ActionResult NeedHelpDetails(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needHelp = _repository.GetNeedHelpOutputModel(name);
            if (needHelp == null)
            {
                return HttpNotFound();
            }

            FillViewBag();
            return View(needHelp);
        }

        public ActionResult NewsDetails(string title)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = _repository.GetNewsOutputModel(title);
            if (news == null)
            {
                return HttpNotFound();
            }

            FillViewBag();
            return View(news);
        }

        public ActionResult ProjectDetails(string title)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _repository.GetProjectOutputModel(title);
            if (project == null)
            {
                return HttpNotFound();
            }

            FillViewBag();
            return View(project);
        }
    }
}