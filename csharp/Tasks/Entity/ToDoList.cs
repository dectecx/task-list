using System.Collections.Generic;
using System.Linq;
using Tasks.ValueObject;

namespace Tasks.Entity
{
    public class ToDoList
    {
        private IList<Project> Projects { get; }

        public ToDoList()
        {
            Projects = new List<Project>();
        }

        public IList<Project> GetProjects()
        {
            return Projects;
        }

        public void AddProject(ProjectName name)
        {
            if (Projects.Any(p => p.GetProjectName() == name))
            {
                return;
            }
            Projects.Add(new Project(name));
        }

        public IList<Task> GetTasks(ProjectName name)
        {
            return Projects.FirstOrDefault(p => p.GetProjectName() == name).GetTasks();
        }
    }
}
