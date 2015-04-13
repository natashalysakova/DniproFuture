using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using DniproFuture.Models.Extentions;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository : IDisposable
    {
        private const int ClientCountConst = 5;
        private const int NewsCountConst = 3;
        private const int PartnersCountConst = 4;
        private const int DonationCountConst = 3;
        private const int ProjectsCountConst = 3;

        public int ClientsCount
        {
            get { return _dbContext.NeedHelp.Count(x => x.Done); }
        }

        public int PartnersCount
        {
            get { return _dbContext.Partners.Count(); }
        }
        public int NewsCount
        {
            get { return _dbContext.News.Count(x => x.Date <= DateTime.Now); }
        }
        public int ProjectsCount
        {
            get { return _dbContext.Projects.Count(x => !x.Done); }
        }
        public int DonationCount
        {
            get { return _dbContext.NeedHelp.Count(x => !x.Done); }
        }



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
                ClientsBlock = new NeedHelpOutputModel[ClientCountConst],
                DonationBlock = new NeedHelpOutputModel[DonationCountConst],
                PartnersBlock = new PartnersModel(),
                ProjectsBlock = new ProjectOutputModel[ProjectsCountConst]
            };

            //Alredy done clients
            var successClientsId = GetAllSuccessClients();
            var random = new Random();
            for (var i = 0; i < ClientCountConst; i++)
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
            for (var i = 0; i < DonationCountConst; i++)
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
            model.PartnersBlock.RandomPartners = new PartnersOutputModel[PartnersCountConst];
            for (var i = 0; i < partnersId.Count; i++)
            {
                model.PartnersBlock.AllPartners[i] = GetPartnersOutputModelById(partnersId[i]);
            }

            for (var i = 0; i < PartnersCountConst; i++)
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


            //partners
            var projectsIds = GetAllProjectsIds();
            for (var i = 0; i < ProjectsCountConst; i++)
            {
                if (projectsIds.Count == 0)
                {
                    model.ProjectsBlock[i] = new ProjectOutputModel();
                }
                else
                {
                    var index = random.Next(0, projectsIds.Count);
                    model.ProjectsBlock[i] = GetProjectOutputModelById(projectsIds[index]);
                    projectsIds.Remove(projectsIds[index]);
                }
            }


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

        public void DeleteAllPhotos(string path, string photosString)
        {
            List<string> photos = photosString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string s in photos)
            {
                string fullPath = Path.Combine(path, s);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }

    }
}