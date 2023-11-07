namespace RobotService.Models;

public class DomesticAssistant : Robot
{
    public const int DomesticAssistantBattery = 20000;
    public const int DomesticAssistantCapacityIndex = 2000;

    public DomesticAssistant(string model) : base(model, DomesticAssistantBattery, DomesticAssistantCapacityIndex)
    {
    }
}