using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models.Abstract;
using DniproFuture.Models;
using DniproFuture.Models.Concrete;

namespace DniproFuture.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;

        public AccountController()
        {
            authProvider = new FormsAuthProvider();
        }

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl) 
        { 
            if (ModelState.IsValid) 
            { 
                if (authProvider.Authenticate(model.UserName, model.Password)) 
                { 
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin")); 
                } 
                else 
                {
                    ModelState.AddModelError("", "Incorrect username or password"); 
                    return View(); 
                } 
            } 
            else 
            { 
                return View(); 
            } 
        } 
    }
}