using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using DniproFuture.Models.Extentions;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        internal NeedHelpOutputModel GetNeedHelpOutputModel(string title)
        {
            int id = -1;
            foreach (NeedHelp n in _dbContext.NeedHelp)
            {
                foreach (NeedHelpLocalSet localSet in n.NeedHelpLocalSet)
                {
                    string fullName = localSet.GetFullName();
                    if (fullName.GetStringForUrl() == title)
                        id = n.Id;
                }
            }

            return GetNeedHelpOutputModelByClientId(id);
        }

        public NeedHelpOutputModel GetNeedHelpOutputModelByClientId(int helpNowrandomClient)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == helpNowrandomClient select c).FirstOrDefault();
            if (client != null)
            {
                var clientInfo = (from local in client.NeedHelpLocalSet
                    where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                    select new {FullName = local.GetFullName(), local.About})
                    .FirstOrDefault();

                if (clientInfo != null)
                    return new NeedHelpOutputModel
                    {
                        FullName = clientInfo.FullName,
                        Photos = client.Photos.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        About = clientInfo.About,
                        Age = client.Birthday.GetAge(),
                        Birthday = client.Birthday,
                        NeedSum = client.NeedSum,
                        Summ = client.Sum,
                        Id = helpNowrandomClient
                    };

                return new NeedHelpOutputModel();
            }

            return new NeedHelpOutputModel();
        }



        internal IQueryable<NeedHelpOutputModel> GetQueryOfNeedHelp()
        {
            var unsuccessClientsId = GetAllUnsuccessClients();
            var model = unsuccessClientsId.Select(GetNeedHelpOutputModelByClientId).ToList();
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
            needHelp.WhatNeed.NeedHelpLocalSet = needHelp.WhoNeed;

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

        internal void EditNeedHelp(NeedHelp needHelp)
        {
            NeedHelp notModified = FindInNeedHelpById(needHelp.Id);


            for (int i = 0; i < notModified.NeedHelpLocalSet.Count; i++)
            {
                var localNotModify = notModified.NeedHelpLocalSet.ElementAt(i);
                var localModify = needHelp.NeedHelpLocalSet.ElementAt(i);
                
                localNotModify.About = localModify.About;
                localNotModify.Address = localModify.Address;
                localNotModify.FirstName = localModify.FirstName;
                localNotModify.LastName = localModify.LastName;
                localNotModify.Patronymic = localModify.Patronymic;
            }

            notModified.Done = needHelp.Done;
            notModified.FinishDate = needHelp.FinishDate;
            notModified.Birthday = needHelp.Birthday;
            notModified.NeedSum = needHelp.NeedSum;
            notModified.StartDate = needHelp.StartDate;
            notModified.Sum = needHelp.Sum;

            if (needHelp.Photos != notModified.Photos)
                notModified.Photos = needHelp.Photos;

            _dbContext.Entry(notModified).State = EntityState.Modified;
            

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

        internal void RemoveNeedHelpById(int id, string path)
        {
            var needHelp = _dbContext.NeedHelp.Find(id);
            for (int i = needHelp.NeedHelpLocalSet.Count - 1; i >= 0; i--)
            {
                NeedHelpLocalSet local = needHelp.NeedHelpLocalSet.ElementAt(i);
                _dbContext.Entry(local).State = EntityState.Deleted;
            }

            List<string> photos = needHelp.Photos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string s in photos)
            {
                string fullPath = Path.Combine(path, s);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _dbContext.NeedHelp.Remove(needHelp);
            _dbContext.SaveChanges();
        }

        public List<NeedHelp> GetListOfDone()
        {
            return (from help in _dbContext.NeedHelp where help.Done select help).ToList();
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