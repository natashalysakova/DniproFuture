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
        private PartnersOutputModel GetPartnersOutputModelById(int i)
        {
            var partner = (from p in _dbContext.Partners
                where p.Id == i
                select new
                {
                    p.Logo,
                    Title = (from local in p.PartnersLocalSet
                        where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                        select local.Name).FirstOrDefault(),
                    p.Link
                }).FirstOrDefault();

            if (partner != null)
                return new PartnersOutputModel
                {
                    Logo = partner.Logo,
                    Title = partner.Title,
                    Link = partner.Link
                    
                };
            return new PartnersOutputModel();
        }

        private List<int> GetAllPartners()
        {
            return (from partner in _dbContext.Partners select partner.Id).ToList();
        }

        internal List<Partners> GetListOfPartners()
        {
            return _dbContext.Partners.ToList();
        }

        internal IQueryable<Partners> GetQueryOfPartners()
        {
            return _dbContext.Partners.ToList().AsQueryable();
        }

        internal Partners FindPartnerById(int? id)
        {
            return _dbContext.Partners.Find(id);
        }

        internal void AddPartner(PartnersInputModel partners)
        {
            partners.MainInfo.PartnersLocalSet = partners.LocalInfo;

            foreach (var helpLocal in partners.LocalInfo)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            _dbContext.Partners.Add(partners.MainInfo);
            _dbContext.PartnersLocalSet.AddRange(partners.LocalInfo);
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

        internal void RemovePartnerById(int id)
        {
            Partners partners = FindPartnerById(id);
            _dbContext.Partners.Remove(partners);
            _dbContext.SaveChanges();
        }

        internal void EditParter(Partners partners)
        {
            _dbContext.Entry(partners).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}