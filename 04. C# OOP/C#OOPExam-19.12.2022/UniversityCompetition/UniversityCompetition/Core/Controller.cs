using System.Text;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Core;

public class Controller : IController
{
    private IRepository<ISubject> subjects;
    private IRepository<IStudent> students;
    private IRepository<IUniversity> universities;

    public Controller()
    {
        subjects = new SubjectRepository();
        students = new StudentRepository();
        universities = new UniversityRepository();
    }

    public string AddSubject(string subjectName, string subjectType)
    {
        ISubject subject = null;

        if (subjectType == nameof(TechnicalSubject))
        {
            subject = new TechnicalSubject(subjects.Models.Count + 1, subjectName);
        }

        else if (subjectType == nameof(EconomicalSubject))
        {
            subject = new EconomicalSubject(subjects.Models.Count + 1, subjectName);
        }

        else if (subjectType == nameof(HumanitySubject))
        {
            subject = new HumanitySubject(subjects.Models.Count + 1, subjectName);
        }

        else
        {
            return $"Subject type {subjectType} is not available in the application!";
        }

        ISubject subjectCheck = subjects.Models.FirstOrDefault(x => x.Name == subjectName);

        if (subjectCheck != null)
        {
            return $"{subjectName} is already added in the repository.";
        }

        subjects.AddModel(subject);
        return $"{subjectType} {subjectName} is created and added to the {subjects.GetType().Name}!";
    }

    public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
    {
        IUniversity university = universities.Models.FirstOrDefault(x => x.Name == universityName);

        if (university != null)
        {
            return $"{universityName} is already added in the repository.";
        }

        List<int> requiredSubjectsIds = new();

        foreach (string subject in requiredSubjects)
        {
            requiredSubjectsIds.Add(subjects.FindByName(subject).Id);
        }

        university = new University(universities.Models.Count + 1, universityName, category, capacity, requiredSubjectsIds);
        universities.AddModel(university);
        return $"{universityName} university is created and added to the {universities.GetType().Name}!";
    }

    public string AddStudent(string firstName, string lastName)
    {
        IStudent student = students.Models.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

        if (student != null)
        {
            return $"{firstName} {lastName} is already added in the repository.";
        }

        student = new Student(students.Models.Count + 1, firstName, lastName);
        students.AddModel(student);
        return $"Student {firstName} {lastName} is added to the {students.GetType().Name}!";
    }

    public string TakeExam(int studentId, int subjectId)
    {
        IStudent student = students.Models.FirstOrDefault(x => x.Id == studentId);

        if (student == null)
        {
            return $"Invalid student ID!";
        }

        ISubject subject = subjects.Models.FirstOrDefault(x => x.Id == subjectId);

        if (subject == null)
        {
            return $"Invalid subject ID!";
        }

        if (student.CoveredExams.Contains(subjectId))
        {
            return $"{student.FirstName} {student.LastName} has already covered exam of {subject.Name}.";
        }

        student.CoverExam(subject);
        return $"{student.FirstName} {student.LastName} covered {subject.Name} exam!";
    }

    public string ApplyToUniversity(string studentName, string universityName)
    {
        string[] studentNames = studentName.Split(" ");

        IStudent student = students.Models.FirstOrDefault(x => x.FirstName == studentNames[0] && x.LastName == studentNames[1]);

        if (student == null)
        {
            return $"{studentNames[0]} {studentNames[1]} is not registered in the application!";
        }

        IUniversity university = universities.Models.FirstOrDefault(x => x.Name == universityName);

        if (university == null)
        {
            return $"{universityName} is not registered in the application!";
        }

        if (student.University == null)
        {
            if (student.CoveredExams.Count != university.RequiredSubjects.Count)
            {
                return $"{studentName} has not covered all the required exams for {universityName} university!";
            }

            else
            {
                foreach (var item in student.CoveredExams)
                {
                    if (university.RequiredSubjects.Contains(item))
                    {
                        continue;
                    }

                    else
                    {
                        return $"{studentName} has not covered all the required exams for {universityName} university!";
                    }
                }
            }
        }

        else if (student.University.Name == universityName)
        {
            return $"{student.FirstName} {student.LastName} has already joined {university.Name}.";
        }

        student.JoinUniversity(university);
        return $"{student.FirstName} {student.LastName} joined {university.Name} university!";
    }

    public string UniversityReport(int universityId)
    {
        IUniversity university = universities.Models.FirstOrDefault(x => x.Id == universityId);

        StringBuilder sb = new();

        sb.AppendLine($"*** {university.Name} ***");
        sb.AppendLine($"Profile: {university.Category}");

        int studentsCount = 0;

        foreach (var student in students.Models)
        {
            if (student.University == null)
            {
                continue;
            }

            else if (student.University.Name == university.Name)
            {
                studentsCount++;
            }
        }

        sb.AppendLine($"Students admitted: {studentsCount}");
        sb.AppendLine($"University vacancy: {university.Capacity - studentsCount}");

        return sb.ToString().TrimEnd();
    }
}
