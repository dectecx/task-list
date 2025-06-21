using System;
using System.Linq;
using Tasks.Entity;
using Tasks.Io;
using Tasks.ValueObject;

namespace Tasks
{
    public sealed class TaskList
    {
        private const string QUIT = "quit";

        private readonly ToDoList tasks = new ToDoList();
        private readonly IConsole console;

        private long lastId = 0;

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole()).Run();
        }

        public TaskList(IConsole console)
        {
            this.console = console;
        }

        public void Run()
        {
            while (true)
            {
                console.Write("> ");
                var command = console.ReadLine();
                if (command == QUIT)
                {
                    break;
                }
                Execute(command);
            }
        }

        private void Execute(string commandLine)
        {
            var commandRest = commandLine.Split(" ".ToCharArray(), 2);
            var command = commandRest[0];
            switch (command)
            {
                case "show":
                    Show();
                    break;
                case "add":
                    Add(commandRest[1]);
                    break;
                case "check":
                    Check(commandRest[1]);
                    break;
                case "uncheck":
                    Uncheck(commandRest[1]);
                    break;
                case "help":
                    Help();
                    break;
                default:
                    Error(command);
                    break;
            }
        }

        private void Show()
        {
            foreach (var project in tasks.GetProjects())
            {
                console.WriteLine(project.GetProjectName());
                foreach (var task in project.GetTasks())
                {
                    console.WriteLine("    [{0}] {1}: {2}", (task.Done ? 'x' : ' '), task.Id, task.Description);
                }
                console.WriteLine();
            }
        }

        private void Add(string commandLine)
        {
            var subcommandRest = commandLine.Split(" ".ToCharArray(), 2);
            var subcommand = subcommandRest[0];
            if (subcommand == "project")
            {
                AddProject(new ProjectName(subcommandRest[1]));
            }
            else if (subcommand == "task")
            {
                var projectTask = subcommandRest[1].Split(" ".ToCharArray(), 2);
                AddTask(new ProjectName(projectTask[0]), projectTask[1]);
            }
        }

        private void AddProject(ProjectName projectName)
        {
            tasks.AddProject(projectName);
        }

        private void AddTask(ProjectName projectName, string description)
        {
            var projectTasks = tasks.GetTasks(projectName);
            if (projectTasks == null)
            {
                Console.WriteLine("Could not find a project with the name \"{0}\".", projectName);
                return;
            }
            projectTasks.Add(new Task { Id = NextId(), Description = description, Done = false });
        }

        private void Check(string idString)
        {
            SetDone(idString, true);
        }

        private void Uncheck(string idString)
        {
            SetDone(idString, false);
        }

        private void SetDone(string idString, bool done)
        {
            int id = int.Parse(idString);
            var identifiedTask = tasks
                .GetProjects()
                .SelectMany(project => project.GetTasks())
                .FirstOrDefault(task => task.Id == id);
            if (identifiedTask == null)
            {
                console.WriteLine("Could not find a task with an ID of {0}.", id);
                return;
            }

            identifiedTask.Done = done;
        }

        private void Help()
        {
            console.WriteLine("Commands:");
            console.WriteLine("  show");
            console.WriteLine("  add project <project name>");
            console.WriteLine("  add task <project name> <task description>");
            console.WriteLine("  check <task ID>");
            console.WriteLine("  uncheck <task ID>");
            console.WriteLine();
        }

        private void Error(string command)
        {
            console.WriteLine("I don't know what the command \"{0}\" is.", command);
        }

        private long NextId()
        {
            return ++lastId;
        }
    }
}
