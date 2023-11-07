//2 Exercise - Average Student Grades
int studentsCount = int.Parse(Console.ReadLine());
Dictionary<string, List<decimal>> students = new();

for (int i = 0; i < studentsCount; i++)
{
    string[] studentInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

    string studentName = studentInfo[0];
    decimal grade = decimal.Parse(studentInfo[1]);

    if (!students.ContainsKey(studentName))
    {
        students.Add(studentName, new List<decimal>());
        students[studentName].Add(grade);
    }

    else
    {
        students[studentName].Add(grade);
    }
}

decimal average = 0;

foreach (var (name, grades) in students)
{
    average = grades.Average();
    Console.Write($"{name} -> ");

    foreach (var grade in grades)
    {
        Console.Write($"{grade:f2} ");
    }
    Console.WriteLine($"(avg: {average:f2})");
}

