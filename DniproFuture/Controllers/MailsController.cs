using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DniproFuture.Models;
using DniproFuture.Models.Repository;

namespace DniproFuture.Controllers
{
    [Authorize]
    public class MailsController : Controller
    {
        private  DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        // GET: Mails
        public ActionResult Index()
        {
            return View(_repository.GetMails());
        }

        // GET: Mails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = _repository.ReadMail(id);
            if (mail == null)
            {
                return HttpNotFound();
            }

            return View(mail);
        }

        // GET: Mails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = _repository.FindMailById(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.RemoveMailById(id);
            return RedirectToAction("Index");
        }

        public ActionResult ClearAll()
        {
            _repository.ClearMail();
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
