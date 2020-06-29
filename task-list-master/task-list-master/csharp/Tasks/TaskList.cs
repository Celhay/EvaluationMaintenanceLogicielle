using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
	public sealed class TaskList
	{
		private const string QUIT = "quit";

		private readonly IDictionary<string, IList<Task>> tasks = new Dictionary<string, IList<Task>>();
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
        //L'application commence ici
        public void Run()
        {
            console.Write("> ");
            var UserCommand = console.ReadLine();
            while (UserCommand != QUIT)
            {
                Execute(UserCommand);
				console.Write("> ");
				UserCommand = console.ReadLine();
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
						Check(commandTableau[1], true);
						break;
					case "uncheck":
						Check(commandTableau[1], false);
						break;
					case "help":
						Help();
						break;
					default:
						Error(UserCommand);
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
			foreach (var project in tasks) {
				console.WriteLine(project.Key);
				foreach (var task in project.Value) {
					console.WriteLine("    [{0}] {1}: {2}", (task.Done ? 'x' : ' '), task.Id, task.Description);
				}
				console.WriteLine();
			}
		}

		private void Add(string commandLine)
		{
			var subcommandRest = commandLine.Split(" ".ToCharArray(), 2);
			var subcommand = subcommandRest[0];
			if (subcommand == "project") {
				AddProject(subcommandRest[1]);
			} 
			if (subcommand == "task") {
				var projectTask = subcommandRest[1].Split(" ".ToCharArray(), 2);
				AddTask(projectTask[0], projectTask[1]);
			}
		}

		private void AddProject(string name)
		{
			tasks[name] = new List<Task>();
		}

		private void AddTask(string project, string description)
		{
			if (!tasks.TryGetValue(project, out IList<Task> projectTasks))
			{
				Console.WriteLine("Could not find a project with the name \"{0}\".", project);
				return;
			}
			projectTasks.Add(new Task { Id = NextId(), Description = description, Done = false });
		}

		private void Check(string idString, bool isCheck) //Check = true  Uncheck = false
		{
			SetDone(idString, isCheck); 
		}

		private void SetDone(string idString, bool done)
		{
			int id = int.Parse(idString);
			var identifiedTask = tasks
				.Select(project => project.Value.FirstOrDefault(task => task.Id == id))
				.Where(task => task != null)
				.FirstOrDefault();
			if (identifiedTask == null) {
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
