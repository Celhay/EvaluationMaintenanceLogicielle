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
        public string Get()
        {
            string
        }
        public void Dispose()
        {
            Console.SetOut(originalOutput);
        }
    }
}
