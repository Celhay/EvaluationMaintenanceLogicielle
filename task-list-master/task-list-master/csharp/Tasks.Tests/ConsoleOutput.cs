using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tasks.Tests
{
    public class ConsoleOutput : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter textWriter;
        public ConsoleOutput()
        {
            stringWriter = new StringWriter();
            textWriter = Console.Out;
            Console.SetOut(stringWriter);
        }
        public string GetOutput()
        {
            return stringWriter.ToString();
        }
        public void Dispose()
        {
            Console.SetOut(textWriter);
        }
    }
}
