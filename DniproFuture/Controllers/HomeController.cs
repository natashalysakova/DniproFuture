using System.Globalization;
using System.Net;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;
using DniproFuture.Models.Repository;
using PagedList;

namespace DniproFuture.Controllers
{
    public class HomeController : Controller
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = _repository.GetMainPageModel();
            return View(model);
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
            var products = _repository.GetQueryOfNeedHelp();
                //returns IQueryable<Product> representing an unknown number of products. a thousand maybe?

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = products.ToPagedList(pageNumber, 12);
                // will only contain 25 products max because of the pageSize

            return View(onePageOfProducts);
        }

        public ActionResult NewsIndex(int? page)
        {
            var news = _repository.GetQueryOfNews();
                //returns IQueryable<Product> representing an unknown number of products. a thousand maybe?

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfNews = news.ToPagedList(pageNumber, 10);
                // will only contain 25 products max because of the pageSize

            return View(onePageOfNews);
        }

        // GET: NeedHelps1/Details/5
        public ActionResult NeedHelpDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needHelp = _repository.GetNeedHelpOutputModelByClientId(id.GetValueOrDefault());
            if (needHelp == null)
            {
                return HttpNotFound();
            }
            return View(needHelp);
        }

        //public ActionResult NewsIndex()
        //{
        //    return View(_repository.GetListOfNews());
        //}

        // GET: News/Details/5
        public ActionResult NewsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = _repository.GetNewsOutputModel(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
    }
}