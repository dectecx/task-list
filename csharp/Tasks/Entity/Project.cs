using System.Collections.Generic;
using Tasks.ValueObject;

namespace Tasks.Entity
{
    public class Project
    {
        private ProjectName Name { get; }

        private IList<Task> Tasks { get; }

        public Project(ProjectName name)
        {
            Name = name;
            Tasks = new List<Task>();
        }

        public ProjectName GetProjectName()
        {
            return Name;
        }

        public IList<Task> GetTasks()
        {
            return Tasks;
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }
    }
}