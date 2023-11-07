using System.Reflection;

namespace Stealer;

public class Spy
{
    public void StealFieldInfo(string inspectedClass, params string[] inspectedFields)
    {
        Type className = Type.GetType(inspectedClass);
        Console.WriteLine($"Class under investigation: {className}");

        Type myType = typeof(Hacker);
        FieldInfo[] allFields = myType.GetFields(
        BindingFlags.Instance |
        BindingFlags.Public |
        BindingFlags.NonPublic);

        object classInstance = Activator.CreateInstance(myType, new object[] { });

        foreach (FieldInfo field in allFields.Where(f => inspectedFields.Contains(f.Name))) 
        {
            Console.WriteLine($"{field.Name} = {field.GetValue(classInstance)}");
        }
    }
}