﻿//3 Exercise - Hierarchical Inheritance
using Farm;

public class StartUp
{
    static void Main()
    {
        Dog dog = new Dog();
        dog.Eat();
        dog.Bark();

        Cat cat = new Cat();
        cat.Eat();
        cat.Meow();
    }
}