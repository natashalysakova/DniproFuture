using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DniproFuture.Models.Extentions;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository : IDisposable
    {
        private const int ClientCount = 5;
        private const int NewsCount = 3;
        private const int PartnersCount = 4;
        private const int DonationCount = 3;
        private readonly uh357966_dbEntities _dbContext = new uh357966_dbEntities();

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        internal MainPageModel GetMainPageModel()
        {
            //Initialization
            var model = new MainPageModel
            {
                ClientsBlock = new NeedHelpOutputModel[ClientCount],
                DonationBlock = new NeedHelpOutputModel[DonationCount],
                PartnersBlock = new PartnersModel()
            };

            //Alredy done clients
            var successClientsId = GetAllSuccessClients();
            var random = new Random();
            for (var i = 0; i < ClientCount; i++)
            {
                if (successClientsId.Count == 0)
                {
                    model.ClientsBlock[i] = new NeedHelpOutputModel();
                }
                else
                {
                    var index = random.Next(0, successClientsId.Count);
                    model.ClientsBlock[i] = GetNeedHelpOutputModelByClientId(successClientsId[index]);
                    successClientsId.Remove(successClientsId[index]);
                }
            }

            //Undone clients
            var unsuccessClientsId = GetAllUnsuccessClients();
            for (var i = 0; i < DonationCount; i++)
            {
                if (unsuccessClientsId.Count == 0)
                {
                    model.DonationBlock[i] = new NeedHelpOutputModel();
                }
                else
                {
                    var index = random.Next(0, unsuccessClientsId.Count);
                    model.DonationBlock[i] = GetNeedHelpOutputModelByClientId(unsuccessClientsId[index]);
                    unsuccessClientsId.Remove(unsuccessClientsId[index]);
                }
            }


            //Partners
            var partnersId = GetAllPartners();
            model.PartnersBlock.AllPartners = new PartnersOutputModel[partnersId.Count];
            model.PartnersBlock.RandomPartners = new PartnersOutputModel[PartnersCount];
            for (var i = 0; i < partnersId.Count; i++)
            {
                model.PartnersBlock.AllPartners[i] = GetPartnersOutputModelById(partnersId[i]);
            }

            for (var i = 0; i < PartnersCount; i++)
            {
                if (partnersId.Count == 0)
                {
                    model.PartnersBlock.RandomPartners[i] = new PartnersOutputModel();
                }
                else
                {
                    var index = random.Next(0, partnersId.Count);
                    model.PartnersBlock.RandomPartners[i] = GetPartnersOutputModelById(partnersId[index]);
                    partnersId.Remove(partnersId[index]);
                }
            }

            //News
            model.NewsBlock = GetLastNews(256, NewsCountEnum.Few);

            model.ContactsBlock = new ContactsInputModel();

            return model;
        }

        private Language GetLanguageByCode(string languageCode)
        {
            return Enumerable.FirstOrDefault(_dbContext.Language, language => language.LanguageCode == languageCode);
        }

        internal List<Language> GetLanguagesList()
        {
            return _dbContext.Language.ToList();
        }

        public bool IsUserExist(string login, string password)
        {
            var user = (from u in _dbContext.User where u.Login == login select u).FirstOrDefault();

            if (user != null)
            {
                if (new MD5Cng().VerifyMd5Hash("myBoobs" + password, user.Password))
                {
                    return true;
                }
            }

            return false;
        }

    }
}