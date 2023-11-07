namespace RobotService.IO
{
    using RobotService.IO.Contracts;
    using System;
    public class Reader : IReader
    {
        public string ReadLine()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            return Console.ReadLine();
        }
    }
}
