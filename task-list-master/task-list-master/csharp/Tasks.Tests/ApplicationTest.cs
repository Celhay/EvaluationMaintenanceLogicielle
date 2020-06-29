using System;
using System.IO;
using NUnit.Framework;

namespace Tasks
{
	[TestFixture]
	public sealed class ApplicationTest
	{
		private FakeConsole console;
		private System.Threading.Thread applicationThread;

		[SetUp]
		public void StartTheApplication()
		{
			this.console = new FakeConsole();
			var taskList = new TaskList(console);
			this.applicationThread = new System.Threading.Thread(() => taskList.Run());
			applicationThread.Start();
		}

		//[TearDown]
		//public void KillTheApplication()
		//{
		//	if (applicationThread == null || !applicationThread.IsAlive)
		//	{
		//		return;
		//	}
			 
		//	applicationThread.Abort();
		//	throw new Exception("The application is still running.");
		//}

		[Test, Timeout(1000)]
		public void ItWorks()
		{
            Execute("show");

            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute("add task secrets Destroy all humans.");

            Execute("show");
            ReadLines(
                "secrets",
                "    [ ] 1: Eat more donuts.",
                "    [ ] 2: Destroy all humans.",
                ""
            );

            Execute("add project training");
            Execute("add task training Four Elements of Simple Design");
            Execute("add task training SOLID");
            Execute("add task training Coupling and Cohesion");
            Execute("add task training Primitive Obsession");
            Execute("add task training Outside-In TDD");
            Execute("add task training Interaction-Driven Design");

            Execute("check 1");
            Execute("check 3");
            Execute("check 5");
            Execute("check 6");

            Execute("show");
            ReadLines(
                "secrets",
                "    [x] 1: Eat more donuts.",
                "    [ ] 2: Destroy all humans.",
                "",
                "training",
                "    [x] 3: Four Elements of Simple Design",
                "    [ ] 4: SOLID",
                "    [x] 5: Coupling and Cohesion",
                "    [x] 6: Primitive Obsession",
                "    [ ] 7: Outside-In TDD",
                "    [ ] 8: Interaction-Driven Design",
                ""
            );

            Execute("quit");
        }

        private void Execute(string command)
        {
			Read("> ");
            Write(command);
        }

        private void Read(string expectedOutput)
        {
            try
            {
				var length = expectedOutput.Length;
				var actualOutput = console.RetrieveOutput(expectedOutput.Length);
				Assert.AreEqual(expectedOutput, actualOutput);
			}
            catch (Exception ex)
			{
				throw ex;
			}
			
		}

		private void ReadLines(params string[] expectedOutput)
		{
			foreach (var line in expectedOutput)
			{
				Read(line + Environment.NewLine);
			}
		}

		private void Write(string input)
		{
			console.SendInput(input + Environment.NewLine);
		}

		[Test, Timeout(1000)]
		public void HelpTest()
		{
			string help = "Commands:" + "\n"
				+ "  show" + "\n"
				+ "  add project <project name>" + "\n"
				+ "  add task <project name> <task description>" + "\n"
				+ "  check <task ID>" + "\n"
				+ "  uncheck <task ID>" + "\n"
				+ "\n";
			Execute("help");

			ReadLines("Commands:" ,
				"  show" ,
				"  add project <project name>",
				"  add task <project name> <task description>",
				"  check <task ID>",
				"  uncheck <task ID>",
				"");

			Assert.That("", Is.EqualTo(""));
		}
		[Test, Timeout(1000)]
		public void ShowTest()
        {
			this.console.SendInput("show" + Environment.NewLine);
			Assert.That(string.Empty, Is.EqualTo(console.RetrieveOutput(0)));
		}

		[Test, Timeout(1000)]
		public void AddTest()
		{
			this.console.SendInput("show" + Environment.NewLine);
			Assert.That(string.Empty, Is.EqualTo(console.RetrieveOutput(0)));
		}

		[Test, Timeout(1000)]
		public void AddTaskTest()
		{
			this.console.SendInput("show" + Environment.NewLine);
			Assert.That(string.Empty, Is.EqualTo(console.RetrieveOutput(0)));
		}
		
		[Test, Timeout(1000)]
		public void SetDoneTest()
		{
			this.console.SendInput("show" + Environment.NewLine);
			Assert.That(string.Empty, Is.EqualTo(console.RetrieveOutput(0)));
		}
		

	}
}
