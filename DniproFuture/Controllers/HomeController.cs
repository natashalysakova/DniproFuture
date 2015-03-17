using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;

namespace DniproFuture.Controllers
{
    public class HomeController : Controller
    {
        DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            MainPageOutputModel model = _repository.GetMainPageModel();
            return View(model);
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

    }
}
