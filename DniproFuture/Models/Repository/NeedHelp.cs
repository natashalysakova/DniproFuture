using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        public NeedHelpOutputModel GetNeedHelpOutputModelByClientId(int helpNowrandomClient)
        {
            var client = (from c in _dbContext.NeedHelp where c.Id == helpNowrandomClient select c).FirstOrDefault();
            if (client != null)
            {
                var clientInfo = (from local in client.NeedHelpLocal
                    where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                    select new {FullName = string.Format("{0} {1}", local.FirstName, local.LastName), local.About})
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