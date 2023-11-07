using CommandPattern.Core.Contracts;
using System;

namespace CommandPattern.Core;

public class Engine : IEngine
{
    public Engine(ICommandInterpreter command)
    {
        Command = command;
    }

    public ICommandInterpreter Command { get; }

    public void Run()
    {
        while (true)
        {
            string input = Console.ReadLine();

            try
            {
                string result = Command.Read(input);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}