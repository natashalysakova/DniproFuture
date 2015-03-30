using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using PagedList;

namespace DniproFuture.Models
{
    public class DniproFutureModelRepository : IDisposable
    {
        private const int ClientCount = 3;
        private const int NewsCount = 3;
        private const int PartnersCount = 3;
        private const int DonationCount = 3;
        private readonly DniproFuture_siteEntities _dbContext = new DniproFuture_siteEntities();

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        internal MainPageOutputModel GetMainPageModel()
        {
            //Initialization
            var model = new MainPageOutputModel
            {
                ClientsBlock = new ClientsOutputModel[ClientCount],
                DonationBlock = new DonationOutputModel[DonationCount],
                PartnersBlock = new PartnersOutputModel[PartnersCount]
            };

            //Alredy done clients
            var successClientsId = GetAllSuccessClients();
            var random = new Random();
            for (var i = 0; i < ClientCount; i++)
            {
                if (successClientsId.Count == 0)
                {
                    model.ClientsBlock[i] = new ClientsOutputModel();
                }
                else
                {
                    var index = random.Next(0, successClientsId.Count);
                    model.ClientsBlock[i] = GetClientOutputModelById(successClientsId[index]);
                    successClientsId.Remove(index);
                }
            }

            //Undone clients
            var unsuccessClientsId = GetAllUnsuccessClients();
            for (var i = 0; i < DonationCount; i++)
            {
                if (unsuccessClientsId.Count == 0)
                {
                    model.DonationBlock[i] = new DonationOutputModel();
                }
                else
                {
                    var index = random.Next(0, unsuccessClientsId.Count);
                    model.DonationBlock[i] = GetDonationOutputModelById(unsuccessClientsId[index]);
                    unsuccessClientsId.Remove(unsuccessClientsId[index]);
                }
            }


            //Partners
            var partnersId = GetAllPartners();
            for (var i = 0; i < PartnersCount; i++)
            {
                if (partnersId.Count == 0)
                {
                    model.PartnersBlock[i] = new PartnersOutputModel();
                }
                else
                {
                    var index = random.Next(0, partnersId.Count);
                    model.PartnersBlock[i] = GetPartnersOutputModelById(unsuccessClientsId[index]);
                    partnersId.Remove(index);
                }
            }

            //News
            model.NewsBlock = GetLastNews(256);

            model.ContactsBlock = new ContactsOutputModel();

            return model;
        }

        private NewsOutputModel[] GetLastNews(int shortTextLenght)
        {
            var lastNews =
                (from news in _dbContext.News orderby news.Date descending select news).Take(NewsCount).ToList();
            if (lastNews.Count != 0)
            {
                var model = new NewsOutputModel[NewsCount];
                for (var i = 0; i < model.Length; i++)
                {
                    model[i] = GetNewsOutputModel(lastNews[i].Id, shortTextLenght);
                }
                return model;
            }
            return new NewsOutputModel[NewsCount];
        }


        private PartnersOutputModel GetPartnersOutputModelById(int i)
        {
            var partner = (from p in _dbContext.Partners
                           where p.Id == i
                           select new
                           {
                               p.Logo,
                               Title = (from local in p.PartnersLocal
                                        where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                        select local.Name).FirstOrDefault()
                           }).FirstOrDefault();

            if (partner != null)
                return new PartnersOutputModel
                {
                    Logo = partner.Logo,
                    Title = partner.Title
                };
            return new PartnersOutputModel();
        }

        private List<int> GetAllPartners()
        {
            return (from partner in _dbContext.Partners select partner.Id).ToList();
        }

        public HelpNowOutputModel GetHelpNowOutputModelByClientId(int helpNowrandomClient)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == helpNowrandomClient select c).FirstOrDefault();
            if (client != null)
            {
                var clientInfo = (from local in client.NeedHelpLocal
                                  where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                  select new { FullName = string.Format("{0} {1}", local.FirstName, local.LastName), local.About })
                    .FirstOrDefault();

                if (clientInfo != null)
                    return new HelpNowOutputModel
                    {
                        FullName = clientInfo.FullName,
                        Photos = client.Photos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        About = clientInfo.About,
                        Age = GetAge(client.Birthday),
                        Birthday = client.Birthday,
                        NeedSum = client.NeedSum,
                        Summ = client.Sum,
                        Id = helpNowrandomClient
                    };

                return new HelpNowOutputModel();
            }

            return new HelpNowOutputModel();
        }

        private static int GetAge(DateTime birthday)
        {
            var age = DateTime.Today.Year - birthday.Year;
            var monthdiff = DateTime.Today.Month - birthday.Month;
            var daydiff = DateTime.Today.Day - birthday.Day;
            if (daydiff < 0)
            {
                monthdiff--;
            }
            if (monthdiff < 0)
            {
                age--;
            }
            return age;
        }

        private ClientsOutputModel GetClientOutputModelById(int index)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == index select c).FirstOrDefault();
            if (client != null)
            {
                var clientInfo = (from local in client.NeedHelpLocal
                                  where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                  select
                                      new { fullName = string.Format("{0} {1}", local.FirstName, local.LastName), local.About })
                    .FirstOrDefault();


                if (clientInfo != null)
                    return new ClientsOutputModel
                    {
                        CompleteSum = client.Sum.ToString(),
                        FullName = clientInfo.fullName,
                        Photos = client.Photos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        About = clientInfo.About,
                        Age = GetAge(client.Birthday),
                        Birthday = client.Birthday,
                        NeedSum = client.NeedSum,
                        Summ = client.Sum
                    };
                return new ClientsOutputModel();
            }
            return new ClientsOutputModel();
        }

        private DonationOutputModel GetDonationOutputModelById(int index)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == index select c).FirstOrDefault();
            if (client != null)
            {
                var clientInfo = (from local in client.NeedHelpLocal
                                  where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                  select
                                      new { fullName = string.Format("{0} {1}", local.FirstName, local.LastName), about = local.About })
                    .FirstOrDefault();


                if (clientInfo != null)
                {
                    return new DonationOutputModel
                    {
                        About = clientInfo.about,
                        FullName = clientInfo.fullName,
                        Photos = client.Photos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        Age = GetAge(client.Birthday),
                        Birthday = client.Birthday,
                        NeedSum = client.NeedSum,
                        Summ = client.Sum,
                        Id = index
                    };
                }
                return new DonationOutputModel();
            }
            return new DonationOutputModel();
        }

        private List<int> GetAllSuccessClients()
        {
            return (from client in _dbContext.NeedHelp where client.Done select client.Id).ToList();
        }

        private List<int> GetAllUnsuccessClients()
        {
            return (from client in _dbContext.NeedHelp where !client.Done select client.Id).ToList();
        }

        internal IQueryable<HelpNowOutputModel> GetQueryOfNeedHelp()
        {
            List<HelpNowOutputModel> model = new List<HelpNowOutputModel>();
            var unsuccessClientsId = GetAllUnsuccessClients();
            foreach (int id in unsuccessClientsId)
            {
                model.Add(GetHelpNowOutputModelByClientId(id));
            }
            return model.AsQueryable();
        }

        internal List<NeedHelp> GetListOfNeedHelp()
        {
            return (from help in _dbContext.NeedHelp where !help.Done select help).ToList();
        }

        internal NeedHelp FindInNeedHelpById(int? id)
        {
            return _dbContext.NeedHelp.Find(id);
        }

        public void AddNeedHelp(NeedHelpInputModel needHelp)
        {
            needHelp.WhatNeed.NeedHelpLocal = needHelp.WhoNeed;

            foreach (var helpLocal in needHelp.WhoNeed)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            _dbContext.NeedHelp.Add(needHelp.WhatNeed);
            _dbContext.NeedHelpLocalSet.AddRange(needHelp.WhoNeed);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
        }

        private Language GetLanguageByCode(string languageCode)
        {
            return Enumerable.FirstOrDefault(_dbContext.Language, language => language.LanguageCode == languageCode);
        }

        internal void EditNeedHelp(NeedHelp needHelp)
        {
            _dbContext.Entry(needHelp).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        internal void RemoveNeedHelpById(int id)
        {
            var needHelp = _dbContext.NeedHelp.Find(id);
            _dbContext.NeedHelp.Remove(needHelp);
            _dbContext.SaveChanges();
        }

        internal List<Language> GetLanguagesList()
        {
            return _dbContext.Language.ToList();
        }

        internal List<News> GetListOfNews()
        {
            return _dbContext.News.ToList();
        }

        internal News FindInNewsById(int? id)
        {
            return _dbContext.News.Find(id);
        }

        public void AddNews(NewsInputModel news)
        {
            news.NewsInfo.NewsLocal = news.Locals;

            foreach (var helpLocal in news.Locals)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            _dbContext.News.Add(news.NewsInfo);
            _dbContext.NewsLocalSet.AddRange(news.Locals);

            _dbContext.SaveChanges();

        }

        public void EditNews(News news)
        {
            _dbContext.Entry(news).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void RemoveNewsById(int id)
        {
            var news = _dbContext.News.Find(id);
            _dbContext.News.Remove(news);
            _dbContext.SaveChanges();
        }

        internal Mail SendMessage(ContactsOutputModel model)
        {
            Mail newMail = new Mail()
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

        public bool IsUserExist(string login, string password)
        {
            User user = (from u in _dbContext.User where u.Login == login select u).FirstOrDefault();

            if (user != null)
            {
                if (VerifyMd5Hash(new MD5Cng(), "myBoobs" + password, user.Password))
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            Mail mail = _dbContext.Mail.Find(id);
            _dbContext.Mail.Remove(mail);
            _dbContext.SaveChanges();
        }

        public Mail FindMailById(int? id)
        {
            return _dbContext.Mail.Find(id);
        }

        public Mail ReadMail(int? id)
        {
            Mail mail = _dbContext.Mail.Find(id);
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

        public IQueryable<NewsOutputModel> GetQueryOfNews()
        {
            List<NewsOutputModel> model = GetLastNews(512).ToList();
            return model.AsQueryable();

        }

        public List<NeedHelp> GetListOfDone()
        {
            return (from help in _dbContext.NeedHelp where help.Done select help).ToList();
        }

        internal NewsOutputModel GetNewsOutputModel(int? id, int shortTextLenght = 256)
        {
            News newsEntity = FindInNewsById(id);
            NewsOutputModel model;

            var news = (from local in newsEntity.NewsLocal
                where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                select new {local.Title, Text = local.Text, local.NewsId}).FirstOrDefault();
            if (news != null)
            {
                model = new NewsOutputModel()
                {
                    Title = news.Title,
                    Photo = newsEntity.Images.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Date = newsEntity.Date,
                    Text = news.Text,
                    Id = news.NewsId
                };

                if (news.Text.Length > shortTextLenght)
                {
                    model.ShortText = news.Text.Remove(shortTextLenght);
                }
                else
                {
                    model.ShortText = news.Text;
                }
            }
            else
            {
                model = new NewsOutputModel();
            }

            return model;
        }
    
    }
}