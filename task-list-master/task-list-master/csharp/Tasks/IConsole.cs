using System;

namespace Tasks
{
	public interface IConsole
	{
		string ReadLine();

		void WriteChevrons();

		void WriteLine(string format, params object[] args);

		void WriteLine();
	}
}
