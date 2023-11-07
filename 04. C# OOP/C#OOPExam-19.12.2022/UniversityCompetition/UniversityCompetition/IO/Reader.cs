namespace UniversityCompetition.IO;

using System;
using UniversityCompetition.IO.Contracts;

public class Reader : IReader
{
    public string ReadLine()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        return Console.ReadLine();
    }
}