﻿using CommandPattern.Core.Contracts;
using System;

namespace CommandPattern.Core;

public class ExitCommand : ICommand
{
    public string Execute(string[] args)
    {
        Environment.Exit(0);
        return null;
    }
}