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

		private void Write(string input)
		{
			console.SendInput(input + Environment.NewLine);
		}

		[Test, Timeout(1000)]
		public void HelpTest()
		{
			string expected = "Commands:" + "\n" +
				"  show" + "\n" +
				"  add project <project name>" + "\n" +
				"  add task <project name> <task description>" + "\n" +
				"  check <task ID>" + "\n" +
				"  uncheck <task ID>" + "\n" + "";


            console.SendInput("help" + Environment.NewLine);

			var actual = console.RetrieveOutput(expected.Length).ToString();

			Assert.That(expected, Is.EqualTo(actual));
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
			string expected = "secrets" + "\n" + "";
			var actual = console.RetrieveOutput(expected.Length).ToString();
			Assert.That(expected, Is.EqualTo(actual));
		}

        [Test, Timeout(1000)]
        public void AddTaskTest()
        {
			this.console.SendInput("add project secrets" + Environment.NewLine);
			this.console.SendInput("add task secrets Eat more donuts." + Environment.NewLine);
			this.console.SendInput("add task secrets Destroy all humans." + Environment.NewLine);

			string expected = "secrets" + "\n" +
					"    [ ] 1: Eat more donuts." + "\n" +
					"    [ ] 2: Destroy all humans." + "\n" +
					"";
            var actual = console.RetrieveOutput(expected.Length).ToString();
			Assert.That(expected, Is.EqualTo(actual));
		}

        [Test, Timeout(1000)]
        public void SetDoneTest()
        {
			this.console.SendInput("add project secrets" + Environment.NewLine);
			this.console.SendInput("add task secrets Eat more donuts." + Environment.NewLine);
			this.console.SendInput("add task secrets Destroy all humans." + Environment.NewLine);
			this.console.SendInput("check 1" + Environment.NewLine);

			string expected = "secrets" + "\n" +
                    "    [x] 1: Eat more donuts." + "\n" +
                    "    [ ] 2: Destroy all humans." + "\n" +
                    "";
            var actual = console.RetrieveOutput(expected.Length).ToString();
			Assert.That(expected, Is.EqualTo(actual));
		}


    }
}
