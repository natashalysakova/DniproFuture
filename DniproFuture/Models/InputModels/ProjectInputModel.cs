using System.Collections.Generic;

namespace DniproFuture.Models.InputModels
{
    public class ProjectInputModel
    {
        public Projects ProjectInfo { get; set; }
        public List<ProjectsLocalSet> Local { get; set; }
    }
}