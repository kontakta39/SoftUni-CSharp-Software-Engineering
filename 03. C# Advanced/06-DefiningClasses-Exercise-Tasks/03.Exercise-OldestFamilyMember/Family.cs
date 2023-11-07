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

    public Person GetMember()
    {
        Person oldestPerson = new Person();

        foreach (var item in family.OrderByDescending(x => x.Age))
        {
            oldestPerson.Name = item.Name;
            oldestPerson.Age = item.Age;
            break;
        }

        return oldestPerson;
    }
}
