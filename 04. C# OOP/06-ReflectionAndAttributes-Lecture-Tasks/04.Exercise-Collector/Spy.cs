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

    public void AnalyzeAccessModifiers(string inspectedClass)
    {
        Type className = Type.GetType(inspectedClass);

        FieldInfo[] publicFields = className.GetFields(
        BindingFlags.Instance |
        BindingFlags.Public);

        MethodInfo[] publicMethods = className.GetMethods
        (BindingFlags.Instance |
        BindingFlags.Public);

        MethodInfo[] privateMethods = className.GetMethods
        (BindingFlags.Instance |
        BindingFlags.NonPublic);

        foreach (FieldInfo field in publicFields)
        {
            Console.WriteLine($"{field.Name} must be private!");
        }

        foreach (MethodInfo method in publicMethods.Where(m => m.Name.StartsWith("set")))
        {
            Console.WriteLine($"{method.Name} must be public!");
        }

        foreach (MethodInfo method in privateMethods.Where(m => m.Name.StartsWith("get")))
        {
            Console.WriteLine($"{method.Name} must be private!");
        }
    }

    public void RevealPrivateMethods(string inspectedClass)
    {
        Type className = Type.GetType(inspectedClass);

        MethodInfo[] privateMethods = className.GetMethods
        (BindingFlags.Instance |
        BindingFlags.NonPublic);

        Console.WriteLine($"All Private Methods of Class: {className}");
        Console.WriteLine($"Base Class: {className.BaseType.Name}");

        foreach (var method in privateMethods)
        {
            Console.WriteLine(method.Name);
        }
    }

    public void CollectGetterAndSetters(string inspectedClass)
    {
        Type className = Type.GetType(inspectedClass);

        MethodInfo[] allMethods = className.GetMethods(
        BindingFlags.Instance |
        BindingFlags.Public |
        BindingFlags.NonPublic);

        foreach (var method in allMethods)
        {
            if (method.Name.StartsWith("get"))
            {
                Console.WriteLine($"{method.Name} will return {method.ReturnType.FullName}");
            }

            else if (method.Name.StartsWith("set"))
            {
                Console.WriteLine($"{method.Name} will set field of {method.ReturnType.FullName}");
            }
        }
    }
}