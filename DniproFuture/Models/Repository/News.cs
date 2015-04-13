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
using Microsoft.Ajax.Utilities;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        private NewsOutputModel[] GetLastNews(int shortTextLenght, NewsCountEnum count)
        {
            var newsQuery =
                (from news in _dbContext.News where news.Date <= DateTime.Now orderby news.Date descending select news);

            List<News> lastNews = count == NewsCountEnum.All ? newsQuery.ToList() : newsQuery.Take(NewsCountConst).ToList();

            if (lastNews.Count != 0)
            {
                var model = new NewsOutputModel[lastNews.Count];
                for (var i = 0; i < model.Length; i++)
                {
                    model[i] = GetNewsOutputModelById(lastNews[i].Id, shortTextLenght);
                }
                return model;
            }

            if (count == NewsCountEnum.Few)
                return new NewsOutputModel[NewsCountConst];

            return new NewsOutputModel[0];
        }

        internal List<News> GetListOfNews()
        {
            return _dbContext.News.OrderByDescending(x => x.Date).ToList();
        }

        internal News FindInNewsById(int? id)
        {
            return _dbContext.News.Find(id);
        }

        public void AddNews(NewsInputModel news, List<string> photosList)
        {
            news.NewsInfo.NewsLocalSet = news.Locals;

            foreach (var helpLocal in news.Locals)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            if (photosList.Count > 0)
                news.NewsInfo.Images = String.Join(";", photosList);
            else
                news.NewsInfo.Images = String.Empty;


            _dbContext.News.Add(news.NewsInfo);
            _dbContext.NewsLocalSet.AddRange(news.Locals);

            _dbContext.SaveChanges();
        }

        public void EditNews(News news)
        {
            News notModified = FindInNewsById(news.Id);


            for (int i = 0; i < notModified.NewsLocalSet.Count; i++)
            {
                var localNotModify = notModified.NewsLocalSet.ElementAt(i);
                var localModify = news.NewsLocalSet.ElementAt(i);

                localNotModify.Title = localModify.Title;
                localNotModify.Text = localModify.Text;
            }

            notModified.Date = news.Date;
            notModified.Images = news.Images;
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

        public void RemoveNewsById(int id, string path)
        {
            var news = _dbContext.News.Find(id);

            for (int i = news.NewsLocalSet.Count - 1; i >= 0; i--)
            {
                NewsLocalSet local = news.NewsLocalSet.ElementAt(i);
                _dbContext.Entry(local).State = EntityState.Deleted;
            }

            DeleteAllPhotos(path, news.Images);

            _dbContext.News.Remove(news);
            _dbContext.SaveChanges();
        }

        public IQueryable<NewsOutputModel> GetQueryOfNewsOutputModel()
        {
            var model = GetLastNews(512, NewsCountEnum.All).ToList();
            return model.AsQueryable();
        }

        public IQueryable<News> GetQueryOfNews()
        {
            var model = GetListOfNews();
            return model.AsQueryable();
        }

        internal NewsOutputModel GetNewsOutputModel(string title)
        {
            int? id = null;
            foreach (News n in _dbContext.News)
            {
                foreach (NewsLocalSet localSet in n.NewsLocalSet)
                {
                    if (localSet.Title.GetStringForUrl() == title)
                        id = n.Id;
                }
            }

            return GetNewsOutputModelById(id);
        }


        internal NewsOutputModel GetNewsOutputModelById(int? id, int shortTextLenght = 256)
        {
            var newsEntity = FindInNewsById(id);
            NewsOutputModel model;

            var news = (from local in newsEntity.NewsLocalSet
                        where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                        select new { local.Title, local.Text, local.NewsId }).FirstOrDefault();
            if (news != null)
            {
                model = new NewsOutputModel
                {
                    Title = news.Title,
                    Photo = newsEntity.Images.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Date = newsEntity.Date,
                    Text = news.Text,
                    Id = news.NewsId,
                    ShortText = news.Text.Length > shortTextLenght ? news.Text.GetTextWithoutTags().Remove(shortTextLenght) : news.Text.GetTextWithoutTags()
                };

            }
            else
            {
                model = new NewsOutputModel();
            }

            return model;
        }

        public void EditNews(News news, List<string> newPhotosString, OldPhotoModel[] oldPhotos)
        {
            bool firsPhotoisDeleted = false;
            if (oldPhotos != null)
            {
                for (int i = 0; i < oldPhotos.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!oldPhotos[i].IsLeave)
                        {
                            firsPhotoisDeleted = true;
                        }
                    }

                    if (oldPhotos[i].IsLeave)
                    {
                        if (firsPhotoisDeleted)
                        {
                            newPhotosString.Add(oldPhotos[i].Path);
                        }
                        else
                        {
                            newPhotosString.Insert(0, oldPhotos[i].Path);
                        }
                    }
                }
            }

            news.Images = string.Join(";", newPhotosString);
            EditNews(news);
        }
    }
}