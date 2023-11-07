namespace RobotService.Models;

public class SpecializedArm : Supplement
{
    public const int SpecializedArmInterface = 10045;
    public const int SpecializedArmBattery = 10000;

    public SpecializedArm() : base(SpecializedArmInterface, SpecializedArmBattery)
    {
    }
}