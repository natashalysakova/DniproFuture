using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}