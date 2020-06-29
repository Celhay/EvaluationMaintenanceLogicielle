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
			var taskList = new TaskList(this.console);
			this.applicationThread = new System.Threading.Thread(() => taskList.Run());
			applicationThread.Start();
		}

        //[TearDown]
        //public void KillTheApplication()
        //{
        //    if (applicationThread == null || !applicationThread.IsAlive)
        //    {
        //        return;
        //    }

        //    applicationThread.Abort();
        //    throw new Exception("The application is still running.");
        //}

        private void Execute(string command)
        {
			Read("> ");
            Write(command);
        }

        private void Read(string expectedOutput)
        {
            var length = expectedOutput.Length;
            var actualOutput = console.RetrieveOutput(expectedOutput.Length);
            Assert.AreEqual(expectedOutput, actualOutput);
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
			Execute("help");

			ReadLines("Commands:" ,
				"  show" ,
				"  add project <project name>",
				"  add task <project name> <task description>",
				"  check <task ID>",
				"  uncheck <task ID>",
				"");

			Assert.That(true, Is.EqualTo(true));
		}
        [Test, Timeout(1000)]
        public void ShowTest()
        {
            this.console.SendInput("show" + Environment.NewLine);
            Assert.That(string.Empty, Is.EqualTo(console.RetrieveOutput(0)));
        }

        [Test, Timeout(1000)]
        public void AddProjectTest()
        {
			this.console.SendInput("add project secrets" + Environment.NewLine);
			ReadLines("secrets",
					"");

        }

        [Test, Timeout(1000)]
        public void AddTaskTest()
        {
			this.console.SendInput("add project secrets" + Environment.NewLine);
			this.console.SendInput("add task secrets Eat more donuts." + Environment.NewLine);
			this.console.SendInput("add task secrets Destroy all humans." + Environment.NewLine);

			ReadLines("secrets",
					"    [ ] 1: Eat more donuts.",
					"    [ ] 2: Destroy all humans.",
					"");

        }

        [Test, Timeout(1000)]
        public void SetDoneTest()
        {
			this.console.SendInput("add project secrets" + Environment.NewLine);
			this.console.SendInput("add task secrets Eat more donuts." + Environment.NewLine);
			this.console.SendInput("add task secrets Destroy all humans." + Environment.NewLine);
			this.console.SendInput("check 1" + Environment.NewLine);
			ReadLines("secrets",
					"    [x] 1: Eat more donuts.",
					"    [ ] 2: Destroy all humans.",
					"");

        }


    }
}
