//1 Exercise - Class Box Data
using BoxData;

public class StartUp
{
    static void Main()
    {
        double length = double.Parse(Console.ReadLine());
        double width = double.Parse(Console.ReadLine());
        double height = double.Parse(Console.ReadLine());

		try
		{
			Box box = new(length, width, height);
			Console.WriteLine($"Surface Area - {box.SurfaceArea():f2}");
            Console.WriteLine($"Lateral Surface Area - {box.LateralSurfaceArea():f2}");
            Console.WriteLine($"Volume - {box.Volume():f2}");
        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
    }
}