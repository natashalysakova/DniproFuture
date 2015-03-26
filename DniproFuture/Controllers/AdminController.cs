using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;

namespace DniproFuture.Controllers
{
    public class AdminController : Controller
    {
        DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        // GET: Admin
        public ActionResult Index()
        {
            if (Response.Cookies["authCookie"].Expires > DateTime.Now)
                return View("Login");

            return View();
        }



        public ActionResult Login()
        {
            if (Response.Cookies["authCookie"].Expires > DateTime.Now)
                return View();

            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            if (_repository.IsUserExist(Login, Password))
            {
                Response.Cookies["authCookie"].Value = Login;
                Response.Cookies["authCookie"].Expires = DateTime.Now.AddDays(1);
            }
            return View();
        }
    }
}