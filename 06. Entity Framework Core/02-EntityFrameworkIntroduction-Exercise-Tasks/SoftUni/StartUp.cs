using Microsoft.EntityFrameworkCore;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni.Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
        //Console.WriteLine(GetEmployeesFullInformation(context));
        //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
        //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        //Console.WriteLine(AddNewAddressToEmployee(context));
        //Console.WriteLine(GetEmployeesInPeriod(context));
        //Console.WriteLine(GetAddressesByTown(context));
        //Console.WriteLine(GetEmployee147(context));
        //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
        //Console.WriteLine(GetLatestProjects(context));
        //Console.WriteLine(IncreaseSalaries(context));
        //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
        //Console.WriteLine(DeleteProjectById(context));
        Console.WriteLine(RemoveTown(context));
    }

    //3 Exercise - Employees Full Information
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        StringBuilder sb = new();

        foreach (var item in employees)
        {
            sb.AppendLine($"{item.FirstName} {item.LastName} {item.MiddleName} " +
                $"{item.JobTitle} {item.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //4 Exercise - Employees with Salary Over 50 000
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .Where(e => e.Salary > 50000)
            .OrderBy(e => e.FirstName)
            .ToList();

        StringBuilder sb = new();

        foreach (var item in employees)
        {
            sb.AppendLine($"{item.FirstName} - {item.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //5 Exercise - Employees from Research and Development
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
            .ToList();

        StringBuilder sb = new();

        foreach (var item in employees)
        {
            sb.AppendLine($"{item.FirstName} {item.LastName} from {item.Name} - ${item.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //6 Exercise - Adding a New Address and Updating Employee
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address address = new();
        address.AddressText = "Vitoshka 15";
        address.TownId = 4;

        var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
        employee.Address = address;

        context.SaveChanges();

        var employees = context.Employees
            .Select(e => new
            {
                e.AddressId,
                e.Address.AddressText
            })
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .ToList();

        StringBuilder sb = new();

        foreach (var item in employees)
        {
            sb.AppendLine($"{item.AddressText}");
        }

        return sb.ToString().TrimEnd();
    }

    //7 Exercise - Employees and Projects
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
              .Take(10)
              .Select(e => new
              {
                  e.FirstName,
                  e.LastName,
                  ManagerFirstName = e.Manager.FirstName,
                  ManagerLastName = e.Manager.LastName,
                  Projects = e.EmployeesProjects
                  .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                  .Select(ep => new
                  {
                      ProjectName = ep.Project.Name,
                      ProjectStartDate = ep.Project.StartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                      ProjectEndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                  })
              });

        StringBuilder employeeManagerResult = new();

        foreach (var employee in employees)
        {
            employeeManagerResult.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

            foreach (var project in employee.Projects)
            {
                employeeManagerResult.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate} - {project.ProjectEndDate}");
            }
        }

        return employeeManagerResult.ToString().TrimEnd();
    }

    //8 Exercise - Addresses by Town
    public static string GetAddressesByTown(SoftUniContext context)
    {
        var employees = context.Employees
            .GroupBy(e => new
            {
                e.Address.AddressText,
                e.Address.Town.Name
            })
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key.Name)
            .ThenBy(group => group.Key.AddressText)
            .Take(10)
            .Select(e => new
            {
                e.Key.AddressText,
                e.Key.Name,
                Count = e.Count()
            });

        StringBuilder addressesByTown = new();

        foreach (var employee in employees)
        {
            addressesByTown.AppendLine($"{employee.AddressText}, {employee.Name} - {employee.Count} employees");
        }

        return addressesByTown.ToString().TrimEnd();
    }

    //9 Exercise - Employee 147
    public static string GetEmployee147(SoftUniContext context)
    {
        var currentEmployees = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                Projects = e.EmployeesProjects.OrderBy(p => p.Project.Name).Select(p => p.Project.Name)
            });


        StringBuilder employeeWithProjects = new();

        foreach (var employee in currentEmployees)
        {
            employeeWithProjects.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                employeeWithProjects.AppendLine(project);
            }
        }

        return employeeWithProjects.ToString().TrimEnd();
    }

    //10 Exercise - Departments with More Than 5 Employees
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var departments = context.Departments
        .Where(d => d.Employees.Count() > 5)
        .OrderBy(d => d.Employees.Count())
        .ThenBy(d => d.Name)
        .Select(d => new
        {
            DepartmentName = d.Name,
            ManagerFirstName = d.Manager.FirstName,
            ManagerLastName = d.Manager.LastName,
            Employees = d.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    EmployeeFirstName = e.FirstName,
                    EmployeeLastName = e.LastName,
                    e.JobTitle
                })
        });

        StringBuilder departmentsWithMoreThanFiveEmployees = new();

        foreach (var department in departments)
        {
            departmentsWithMoreThanFiveEmployees.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

            foreach (var currentEmployee in department.Employees)
            {
                departmentsWithMoreThanFiveEmployees.AppendLine($"{currentEmployee.EmployeeFirstName} {currentEmployee.EmployeeLastName} - {currentEmployee.JobTitle}");
            }
        }

        return departmentsWithMoreThanFiveEmployees.ToString().TrimEnd();
    }

    //11 Exercise - Find Latest 10 Projects
    public static string GetLatestProjects(SoftUniContext context)
    {
        var projects = context.Projects
            .OrderByDescending(p => p.StartDate)
            .Take(10)
            .OrderBy(p => p.Name)
            .Select(p => new
            {
                p.Name,
                p.Description,
                ProjectStartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
            });

        StringBuilder latestProjects = new();

        foreach (var project in projects)
        {
            latestProjects.AppendLine(project.Name);
            latestProjects.AppendLine(project.Description);
            latestProjects.AppendLine(project.ProjectStartDate);
        }

        return latestProjects.ToString().TrimEnd();
    }

    //12 Exercise - Increase Salaries
    public static string IncreaseSalaries(SoftUniContext context)
    {
        var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Where(e => e.Department.Name == "Engineering" ||
                 e.Department.Name == "Tool Design" ||
                 e.Department.Name == "Marketing" ||
                 e.Department.Name == "Information Services")
                 .ToList();

        foreach (var employee in employees)
        {
            employee.Salary *= 1.12m;
        }

        context.SaveChanges();

        StringBuilder employeesWithIncreasedSalaries = new();

        foreach (var employee in employees)
        {
            employeesWithIncreasedSalaries.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
        }

        return employeesWithIncreasedSalaries.ToString().TrimEnd();
    }

    //13 Exercise - Find Employees by First Name Starting with "Sa"
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                });

        StringBuilder employeesWithNamesSa = new();

        foreach (var employee in employees)
        {
            employeesWithNamesSa.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
        }

        return employeesWithNamesSa.ToString().TrimEnd();
    }

    //14 Exercise - Delete Project by Id
    public static string DeleteProjectById(SoftUniContext context)
    {
        var projectToDelete = context.Projects.FirstOrDefault(p => p.ProjectId == 2);

        var employeesReferencingProject = context.EmployeesProjects
            .Where(ep => ep.ProjectId == projectToDelete.ProjectId)
            .ToList();

        context.EmployeesProjects.RemoveRange(employeesReferencingProject);
        context.Projects.Remove(projectToDelete);
        context.SaveChanges();

        var projects = context.Projects
            .Select(p => p.Name);

        StringBuilder projectsNames = new();

        foreach (var currentProjectName in projects)
        {
            projectsNames.AppendLine(currentProjectName);
        }

        return projectsNames.ToString().TrimEnd();
    }

    //15 Exercise - Remove Town
    public static string RemoveTown(SoftUniContext context)
    {
        var town = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

        var employees = context.Employees
            .Where(e => e.Address.Town.Name == town.Name)
            .ToList();

        foreach (var employee in employees)
        {
            employee.AddressId = null;
        }

        var addresses = context.Addresses
            .Where(a => a.Town.Name == town.Name)
            .ToList();

        int addressesCount = addresses.Count;
        context.Addresses.RemoveRange(addresses);
        context.Towns.Remove(town);
        context.SaveChanges();

        return $"{addressesCount} addresses in Seattle were deleted";
    }
}