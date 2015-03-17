using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DniproFuture.Models
{
    public class DniproFutureModelRepository
    {
        private const int ClientCount = 3;
        private const int NewsCount = 3;
        private const int PartnersCount = 3;
        private const int DonationCount = 3;
        private readonly DniproFuture_siteEntities _dbContext = new DniproFuture_siteEntities();

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
            int helpNowrandomClient = unsuccessClientsId[random.Next(0, unsuccessClientsId.Count)];
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
                    unsuccessClientsId.Remove(index);
                }
            }

            //HelpNow
            model.HelpNowBlock = GetHelpNowOutputModelByClientId(helpNowrandomClient);
            
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
            model.NewsBlock = GetLastNews();


            return model;
        }

        private NewsOutputModel[] GetLastNews()
        {
            var lastNews = (from news in _dbContext.News orderby news.Date descending select news).Take(NewsCount).ToList();
            if (lastNews.Count != 0)
            {
                NewsOutputModel[] model = new NewsOutputModel[NewsCount];
                for (int i = 0; i < model.Length; i++)
                {
                    var news = (from local in lastNews[i].NewsLocal
                                where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                select new { local.Title, Text = local.Text.Remove(128) }).FirstOrDefault();;
                    model[i] = new NewsOutputModel()
                    {
                        Title = news.Title,
                        ShortText = news.Text,
                        Photo = lastNews[i].Images,
                        Date = lastNews[i].Date
                    };
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

            return new PartnersOutputModel()
            {
                Logo = partner.Logo,
                Title = partner.Title
            };
        }

        private List<int> GetAllPartners()
        {
            return (from partner in _dbContext.Partners select partner.Id).ToList();
        }

        private HelpNowOutputModel GetHelpNowOutputModelByClientId(int helpNowrandomClient)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == helpNowrandomClient select c).FirstOrDefault();
            if (client != null)
            {
                var fullName = (from local in client.NeedHelpLocal
                                where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                                select string.Format("{0} {1}", local.FirstName, local.LastName)).FirstOrDefault();;

                return new HelpNowOutputModel()
                {
                    FullName = fullName,
                    Photo = client.Photos
                };
            }

            return new HelpNowOutputModel();
        }

        private ClientsOutputModel GetClientOutputModelById(int index)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == index select c).FirstOrDefault();
            if (client != null)
            {
                var fullName = (from local in client.NeedHelpLocal
                    where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                    select string.Format("{0} {1}", local.FirstName, local.LastName)).FirstOrDefault();

                return new ClientsOutputModel
                {
                    CompleteSum = client.Sum.ToString(),
                    FullName = fullName,
                    Photo = client.Photos
                };
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
                        new {fullName = string.Format("{0} {1}", local.FirstName, local.LastName), about = local.About})
                    .FirstOrDefault();;


                return new DonationOutputModel
                {
                    About = clientInfo.about,
                    FullName = clientInfo.fullName,
                    Photo = client.Photos
                };
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
    }
}