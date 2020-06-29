using System;

namespace Tasks
{
	public class RealConsole : IConsole
	{
		public string ReadLine()
		{
			return Console.ReadLine();
		}

		//dessine les chevrons
		public void WriteChevrons()
        {
            Console.Write("> ");
        }

        public void WriteLine(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}

		public void WriteLine()
		{
			Console.WriteLine();
		}
	}
}
