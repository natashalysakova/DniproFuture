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
                    select new {FullName = local.GetFullName(), local.FirstName, local.LastName, local.About})
                    .FirstOrDefault();

                if (clientInfo != null)
                    return new NeedHelpOutputModel
                    {
                        FirstName = clientInfo.FirstName,
                        LastName = clientInfo.LastName,
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



        internal IQueryable<NeedHelpOutputModel> GetQueryOfNeedHelpOutputModel()
        {
            var unsuccessClientsId = GetAllUnsuccessClients();
            var model = unsuccessClientsId.Select(GetNeedHelpOutputModelByClientId).ToList();
            return model.AsQueryable();
        }

        internal IQueryable<NeedHelp> GetQueryOfNeedHelp()
        {
            var model = (from help in _dbContext.NeedHelp
                where !help.Done
                select help).ToList();
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

        public void AddNeedHelp(NeedHelpInputModel needHelp, List<string> photosList )
        {
            needHelp.WhatNeed.NeedHelpLocalSet = needHelp.WhoNeed;

            foreach (var helpLocal in needHelp.WhoNeed)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            if (photosList.Count > 0)
                needHelp.WhatNeed.Photos = String.Join(";", photosList);
            else
                needHelp.WhatNeed.Photos = String.Empty;


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

        private void EditNeedHelp(NeedHelp needHelp)
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

            DeleteAllPhotos(path, needHelp.Photos);

            _dbContext.NeedHelp.Remove(needHelp);
            _dbContext.SaveChanges();
        }

        public IQueryable<NeedHelp> GetQueryOfDone()
        {
            return (from help in _dbContext.NeedHelp where help.Done select help).ToList().AsQueryable();
        }

        private List<int> GetAllSuccessClients()
        {
            return (from client in _dbContext.NeedHelp where client.Done select client.Id).ToList();
        }

        private List<int> GetAllUnsuccessClients()
        {
            return (from client in _dbContext.NeedHelp where !client.Done select client.Id).ToList();
        }

        public void EditNeedHelp(NeedHelp needHelp, List<string> newPhotosString, OldPhotoModel[] oldPhotos)
        {
            newPhotosString = ReWritePhotos(newPhotosString, oldPhotos);

            needHelp.Photos = string.Join(";", newPhotosString);
            EditNeedHelp(needHelp);
        }

        public void CloseHelp(NeedHelp needHelp)
        {
            needHelp.Sum = needHelp.NeedSum;
            needHelp.Done = true;
            needHelp.FinishDate = DateTime.Now;
            _dbContext.Entry(needHelp).State = EntityState.Modified;
            _dbContext.SaveChanges();

        }

        public class AddSummResult
        {
            public AddSummResult(bool result, int newSumm, int id)
            {
                Result = result;
                NewSumm = newSumm;
                Id = id;
            }

            public bool Result { get; set; }
            public int NewSumm { get; set; }
            public int Id { get; set; }
        }

        public AddSummResult AddSummToNeedHelp(int id, string changeType, int[] summ)
        {
            NeedHelp help = FindInNeedHelpById(id);
            bool result = false;

            if (changeType == "add")
            {
                if ((help.Sum + summ[0]) < help.NeedSum)
                {
                    help.Sum += summ[0];
                    result = true;
                }
            }
            else if (changeType == "change")
            {
                if (summ[1] < help.NeedSum)
                {
                    help.Sum = summ[1];
                    result = true;
                }
            }

            if (result)
            {
                _dbContext.Entry(help).State = EntityState.Modified;
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

            return new AddSummResult(result, help.Sum, help.Id);
        }
    }
}