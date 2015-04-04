using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    }
}