namespace Footballers;

using AutoMapper;
using Footballers.Data.Models;
using Footballers.DataProcessor.ExportDto;
using Footballers.DataProcessor.ImportDto;

// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
public class FootballersProfile : Profile
{
    public FootballersProfile()
    {
        CreateMap<ImportCoachDto, Coach>();
        CreateMap<Coach, ExportCoachDto>();
    }
}