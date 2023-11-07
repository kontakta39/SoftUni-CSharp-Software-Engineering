namespace RobotService.Models;

public class IndustrialAssistant : Robot
{
    public const int IndustrialAssistantBattery = 40000;
    public const int IndustrialAssistantCapacityIndex = 5000;

    public IndustrialAssistant(string model) : base(model, IndustrialAssistantBattery, IndustrialAssistantCapacityIndex)
    {
    }
}