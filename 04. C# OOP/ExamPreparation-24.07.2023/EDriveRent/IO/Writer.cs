using EDriveRent.IO.Contracts;
using System;

namespace EDriveRent.IO
{
    public class Writer : IWriter
    {
        public void Write(string message) => Console.Write(message);

        public void WriteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}
