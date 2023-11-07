namespace RobotService.IO
{
    using RobotService.IO.Contracts;
    using System;
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
