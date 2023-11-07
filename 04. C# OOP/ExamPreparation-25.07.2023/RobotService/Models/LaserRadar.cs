namespace RobotService.Models;

public class LaserRadar : Supplement
{
    public const int LaserRadarInterface = 20082;
    public const int LaserRadarBattery = 5000;

    public LaserRadar() : base(LaserRadarInterface, LaserRadarBattery)
    {
    }
}