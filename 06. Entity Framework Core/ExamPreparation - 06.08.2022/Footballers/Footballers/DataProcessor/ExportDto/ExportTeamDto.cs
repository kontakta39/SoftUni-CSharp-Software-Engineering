namespace Footballers.DataProcessor.ExportDto;

public class ExportTeamDto
{
    public string Name { get; set; }
    public FootballersDto[] Footballers { get; set; }
}

public class FootballersDto
{
    public string FootballerName { get; set; }

    public string ContractStartDate { get; set; }

    public string ContractEndDate { get; set; }

    public string BestSkillType { get; set; }

    public string PositionType { get; set; }
}