using System.Text;

namespace SoftUniKindergarten;
public class Kindergarten
{
    public Kindergarten(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
        Registry = new();
    }

    public bool AddChild(Child child)
    {
        bool isContained = false;

        if (Registry.Count < Capacity)
        {
            Registry.Add(child);
            isContained = true;
        }

        else
        {
            isContained = false;
        }

        return isContained;
    }

    public bool RemoveChild(string fullName)
    {
        bool isContained = false;

        Child child = Registry.Where(x => $"{x.FirstName} {x.LastName}" == fullName).FirstOrDefault();
        isContained = Registry.Remove(child);
        
        return isContained;
    }

    public Child GetChild(string fullName)
    {
        Child child = Registry.Where(x => $"{x.FirstName} {x.LastName}" == fullName).FirstOrDefault();
        if (child != null)
        {
            return child;
        }

        return null;
    }

    public string RegistryReport()
    {
        StringBuilder sb = new();

        sb.AppendLine($"Registered children in {Name}:");

        foreach (Child child in Registry.OrderByDescending(x => x.Age)
            .ThenBy(x => x.LastName).ThenBy(x => x.FirstName))
        {
            sb.AppendLine(child.ToString());
        }

       return sb.ToString().TrimEnd();
    }

    public string Name { get; set; }
    public int Capacity { get; set; }
    public List<Child> Registry { get; set; }

    public int ChildrenCount { get { return this.Registry.Count; } }
}