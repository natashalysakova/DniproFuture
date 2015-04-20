using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using DniproFuture.Models.Extentions;
using DniproFuture.Models.InputModels;
using DniproFuture.Models.OutputModels;

namespace DniproFuture.Models.Repository
{
    public partial class DniproFutureModelRepository
    {
        internal IQueryable<ProjectOutputModel> GetQueryOfProjectsOutputModels()
        {
            var projects = _dbContext.Projects.ToList();
            List<ProjectOutputModel> models = new List<ProjectOutputModel>();
            foreach (Projects project in projects)
            {
                models.Add(GetProjectOutputModel(project));
            }

            return models.AsQueryable();
        }

        public ProjectOutputModel GetProjectOutputModel(string title)
        {
            int? id = null;
            foreach (Projects n in _dbContext.Projects)
            {
                foreach (ProjectsLocalSet localSet in n.ProjectsLocalSet)
                {
                    if (localSet.ProjectName.GetStringForUrl() == title)
                        id = n.Id;
                }
            }

            return GetProjectOutputModelById(id);
        }

        public Projects FindProjectById(int? id)
        {
            return _dbContext.Projects.Find(id);
        }

        public void AddProject(ProjectInputModel projects, List<string> photosList)
        {
            projects.ProjectInfo.ProjectsLocalSet = projects.Local;

            foreach (var helpLocal in projects.Local)
            {
                helpLocal.Language = GetLanguageByCode(helpLocal.Language.LanguageCode);
            }

            if (photosList.Count > 0)
                projects.ProjectInfo.Photos = String.Join(";", photosList);
            else
                projects.ProjectInfo.Photos = String.Empty;


            _dbContext.Projects.Add(projects.ProjectInfo);
            _dbContext.ProjectsLocalSet.AddRange(projects.Local);

            _dbContext.SaveChanges();
        }

        private void EditProject(Projects projects)
        {
            Projects notModified = FindProjectById(projects.Id);


            for (int i = 0; i < notModified.ProjectsLocalSet.Count; i++)
            {
                var localNotModify = notModified.ProjectsLocalSet.ElementAt(i);
                var localModify = projects.ProjectsLocalSet.ElementAt(i);

                localNotModify.ProjectName = localModify.ProjectName;
                localNotModify.AboutProject = localModify.AboutProject;
            }

            notModified.NeedSum = projects.NeedSum;
            notModified.Sum = projects.Sum;
            notModified.Done = projects.Done;
            notModified.StartDate = projects.StartDate;
            notModified.FinishDate = projects.FinishDate;

            notModified.Photos = projects.Photos;
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

        public void EditProject(Projects projects, List<string> newPhotosString, OldPhotoModel[] oldPhotos)
        {
            newPhotosString = ReWritePhotos(newPhotosString, oldPhotos);

            projects.Photos = string.Join(";", newPhotosString);
            EditProject(projects);
        }

        private List<int> GetAllProjectsIds()
        {
            return (from projects in _dbContext.Projects select projects.Id).ToList();
        }

        private ProjectOutputModel GetProjectOutputModelById(int? projectsId, int shortTextLenght = 256)
        {
            Projects projects = FindProjectById(projectsId);
            return GetProjectOutputModel(projects, shortTextLenght);
        }

        private ProjectOutputModel GetProjectOutputModel(Projects projects, int shortTextLenght = 256)
        {

            ProjectOutputModel model;

            var locals = (from local in projects.ProjectsLocalSet
                where local.Language.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name
                select new {local.ProjectName, local.AboutProject}).FirstOrDefault();
            if (locals != null)
            {
                model = new ProjectOutputModel()
                {
                    About = locals.AboutProject,
                    FinishDate = projects.FinishDate,
                    IsDone = projects.Done,
                    NeedSum = projects.NeedSum,
                    Photos = projects.Photos.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    ShortText =
                        locals.AboutProject.Length > shortTextLenght
                            ? locals.AboutProject.GetTextWithoutTags().Remove(shortTextLenght)
                            : locals.AboutProject.GetTextWithoutTags(),
                    StartDate = projects.StartDate,
                    Summ = projects.Sum,
                    Title = locals.ProjectName
                };
            }
            else
            {
                model = new ProjectOutputModel();
            }

            return model;
        }

        public IQueryable<Projects> GetQueryOfProjects()
        {
            return _dbContext.Projects.ToList().AsQueryable();
        }

        public void RemoveProjectById(int id, string fullPath)
        {
            var project = _dbContext.Projects.Find(id);

            for (int i = project.ProjectsLocalSet.Count - 1; i >= 0; i--)
            {
                ProjectsLocalSet local = project.ProjectsLocalSet.ElementAt(i);
                _dbContext.Entry(local).State = EntityState.Deleted;
            }

            DeleteAllPhotos(fullPath, project.Photos);

            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();
        }
    }

}