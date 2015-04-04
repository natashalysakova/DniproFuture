using System.Collections.Generic;
using System.Linq;
using DniproFuture.Models.InputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        internal Mail SendMessage(ContactsInputModel model)
        {
            var newMail = new Mail
            {
                Email = model.Email,
                Message = model.Message,
                Name = model.Name,
                Phone = model.Phone,
                IsRead = false
            };

            _dbContext.Mail.Add(newMail);
            _dbContext.SaveChanges();

            return newMail;
        }

        public List<Mail> GetUnreadMails()
        {
            return (from mail in _dbContext.Mail where mail.IsRead == false select mail).ToList();
        }

        internal List<Mail> GetMails()
        {
            return _dbContext.Mail.OrderByDescending(x => x.Id).ToList();
        }

        public void RemoveMailById(int id)
        {
            var mail = _dbContext.Mail.Find(id);
            _dbContext.Mail.Remove(mail);
            _dbContext.SaveChanges();
        }

        public Mail FindMailById(int? id)
        {
            return _dbContext.Mail.Find(id);
        }

        public Mail ReadMail(int? id)
        {
            var mail = _dbContext.Mail.Find(id);
            mail.IsRead = true;
            _dbContext.SaveChanges();
            return mail;
        }

        internal void ClearMail()
        {
            var allMails = _dbContext.Mail.ToList();
            _dbContext.Mail.RemoveRange(allMails);
            _dbContext.SaveChanges();
        }
    }
}