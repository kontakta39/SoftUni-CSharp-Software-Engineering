using System.Text;

namespace DefiningClasses;

public class Family
{
    private List<Person> family;

    public Family()
    {
        family = new List<Person>();
    }

    public void AddMember(Person person)
    {
        family.Add(person);
    }

    public override string ToString() 
    { 
      StringBuilder sb = new();

        foreach (Person person in family.OrderBy(x => x.Name)) 
        {
            if (person.Age > 30)
            {
                sb.AppendLine($"{person.Name} - {person.Age}");
            }
        }

        return sb.ToString();
    }
}
