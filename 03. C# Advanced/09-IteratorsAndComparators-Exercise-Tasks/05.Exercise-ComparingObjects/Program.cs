//5 Exercise - Comparing Objects
using ComparingObjects;

List<Person> people = new();

string input = Console.ReadLine();

while (input != "END")
{
    string[] personInfo = input
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string name = personInfo[0];
    int age = int.Parse(personInfo[1]);
    string town = personInfo[2];

    Person person = new(name, age, town);
    people.Add(person);

    input = Console.ReadLine();
}

int matchesCount = 0;
int differenceCount = 0;
int personToCompareIndex = int.Parse(Console.ReadLine()) - 1;

Person personToCompare = people[personToCompareIndex];

for (int i = 0; i < people.Count; i++)
{
    if (people[i].CompareTo(personToCompare) == 0)
    {
        matchesCount++;
    }

    else
    {
        differenceCount++;
    }
}

if (matchesCount == 1)
{
    Console.WriteLine("No matches");
}

else
{
    Console.WriteLine($"{matchesCount} {differenceCount} {people.Count}");
}