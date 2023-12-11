namespace Cadastre;

using AutoMapper;
using Cadastre.Data.Models;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.DataProcessor.ImportDtos;

// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
public class CadastreProfile : Profile
{
    public CadastreProfile()
    {
        CreateMap<ImportDistrictDto, District>();
        CreateMap<Property, ExportPropertyWithDistrictDto>();
    }
}