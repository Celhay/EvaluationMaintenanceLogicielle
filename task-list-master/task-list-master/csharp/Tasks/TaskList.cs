using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
	public sealed class TaskList
	{
		private readonly IDictionary<string, IList<Task>> tasks = new Dictionary<string, IList<Task>>();
		//private readonly IConsole console;
		private long lastId = 0;

		public static void Main(string[] args)
		{
			new TaskList().Run();
		}

		public TaskList()
		{

		}

		//L'application commence ici
		public void Run()
        {
			//Console.WriteLine("Bienvenue !");
			Console.WriteLine("> ");
            var UserCommand = Console.ReadLine();
            while (UserCommand != "quit")
            {
                Execute(UserCommand);
				Console.WriteLine("> ");
				UserCommand = Console.ReadLine();
			}
        }

        private void Execute(string commandLine)
		{
			var commandTableau = commandLine.Split(" ".ToCharArray(), 2);
			var UserCommand = commandTableau[0]; 
			try
            {
				switch (UserCommand)
				{
					case "show":
						Show();
						break;
					case "add":
						Add(commandTableau[1]);
						break;
					case "check":
						SetDone(commandTableau[1], true);
						break;
					case "uncheck":
						SetDone(commandTableau[1], false);
						break;
					case "help":
						Help();
						break;
					default:
						Console.WriteLine("I don't know what the command \"{0}\" is.", UserCommand);
						break;
				}
			}
			catch(Exception ex)
            {
				throw new Exception (ex.Message);
			}
			
		}

		private void Show()
        {
            if (tasks.Count > 0)
            {
                foreach (var project in tasks)
                {
                    Console.WriteLine(project.Key);
                    foreach (var task in project.Value)
                    {
                        Console.WriteLine("    [{0}] {1}: {2}", (task.IsDone ? 'x' : ' '), task.Id, task.Description);
                    }
                    Console.WriteLine();
                }
				return;
            }
			Console.WriteLine("Nothing to see here ");

        }

        private void Add(string entree)
		{
			var userCommandTab = entree.Split(" ".ToCharArray(), 2);
			var command = userCommandTab[0];
			if (command == "project") {
				tasks[userCommandTab[1]] = new List<Task>();
			} 
			if (command == "task") {
				var projectTask = userCommandTab[1].Split(" ".ToCharArray(), 2);
				AddTask(projectTask[0], projectTask[1]);
			}
		}

		private void AddTask(string project, string description)
		{
			if (tasks.TryGetValue(project, out IList<Task> projectTasks))
			{
				projectTasks.Add(new Task { Id = ++lastId, Description = description, IsDone = false });
				return;
			}
			Console.WriteLine("Could not find a project with the name \"{0}\".", project);
		}

		private void SetDone(string idString, bool done) //Check = true  Uncheck = false
		{
			int id = int.Parse(idString);
			var identifiedTask = tasks
				.Select(project => project.Value.FirstOrDefault(task => task.Id == id))
				.Where(task => task != null)
				.FirstOrDefault();
			if (identifiedTask == null) {
				Console.WriteLine("Could not find a task with an ID of {0}.", id);
				return;
			}

			identifiedTask.IsDone = done;
		}
		
		private void Help()
		{
			Console.WriteLine("Commands:");
			Console.WriteLine("  show");
			Console.WriteLine("  add project <project name>");
			Console.WriteLine("  add task <project name> <task description>");
			Console.WriteLine("  check <task ID>");
			Console.WriteLine("  uncheck <task ID>");
			Console.WriteLine();
		}
	}
}
