using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tasks.Tests
{
    public class Logger : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter textWriter;
        public Logger()
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
