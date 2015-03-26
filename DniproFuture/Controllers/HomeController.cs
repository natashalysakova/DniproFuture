using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [HttpPost]
        public ActionResult Contact(ContactsOutputModel model)
        {
            _repository.SendMessage(model);

            //var fromAddress = new MailAddress("fromemail@gmail.com", "От " + model.Name);
            //var toAddress = new MailAddress("toemail@gmail.com", "Администратору");
            //const string fromPassword = "fromemailpassword";
            //const string subject = "Форма связи \"Будущее Днепра\"";
            //string body = string.Format("От: {0}\nE-mail: {1}\nТелефон: {2}\nСообщение: {3}", model.Name, model.Email, model.Phone, model.Message);

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //};
            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body
            //})
            //{
            //    smtp.Send(message);
            //}


            return new RedirectResult("Index");
        }
    }
}
