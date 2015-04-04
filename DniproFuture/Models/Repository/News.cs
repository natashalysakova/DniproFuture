using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        private NewsOutputModel[] GetLastNews(int shortTextLenght, NewsCountEnum count)
        {
            List<News> lastNews = count == NewsCountEnum.All ? (from news in _dbContext.News orderby news.Date descending select news).ToList() : (from news in _dbContext.News orderby news.Date descending select news).Take(NewsCount).ToList();

            if (lastNews.Count != 0)
            {
                var model = new NewsOutputModel[lastNews.Count];
                for (var i = 0; i < model.Length; i++)
                {
                    model[i] = GetNewsOutputModel(lastNews[i].Id, shortTextLenght);
                }
                return model;
            }

            return new NewsOutputModel[lastNews.Count];
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

        public IQueryable<NewsOutputModel> GetQueryOfNews()
        {
            var model = GetLastNews(512, NewsCountEnum.All).ToList();
            return model.AsQueryable();
        }

        internal NewsOutputModel GetNewsOutputModel(int? id, int shortTextLenght = 256)
        {
            var newsEntity = FindInNewsById(id);
            NewsOutputModel model;

            var news = (from local in newsEntity.NewsLocal
                where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                select new {local.Title, local.Text, local.NewsId}).FirstOrDefault();
            if (news != null)
            {
                model = new NewsOutputModel
                {
                    Title = news.Title,
                    Photo = newsEntity.Images.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Date = newsEntity.Date,
                    Text = news.Text,
                    Id = news.NewsId,
                    ShortText = news.Text.Length > shortTextLenght ? news.Text.Remove(shortTextLenght) : news.Text
                };

            }
            else
            {
                model = new NewsOutputModel();
            }

            return model;
        }
    }
}