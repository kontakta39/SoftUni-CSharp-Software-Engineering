using CommandPattern.Core.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace CommandPattern.Core;

public class CommandInterpreter : ICommandInterpreter
{
    public string Read(string args)
    {
        string[] commandParts = args
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string commandName = commandParts[0];
        string[] commandArgs = commandParts.Length > 1 ? commandParts[1].Split() : new string[0];

        // Use reflection to dynamically create the command object
        Type commandType = Assembly.GetEntryAssembly()
            .GetTypes()
            .FirstOrDefault(x => x.Name == $"{commandName}Command");

        if (commandType == null)
        {
            throw new InvalidOperationException("Invalid command!");
        }

        ICommand command = (ICommand)Activator.CreateInstance(commandType);
        return command.Execute(commandArgs);
    }
}