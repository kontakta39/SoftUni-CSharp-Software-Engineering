namespace RawData;

public class Tire
{
    public Tire(float pressure, int age)
    {
        Pressure = pressure;
        Age = age;
    }
    public int Age { get; set; }
    public float Pressure { get; set; }
}
