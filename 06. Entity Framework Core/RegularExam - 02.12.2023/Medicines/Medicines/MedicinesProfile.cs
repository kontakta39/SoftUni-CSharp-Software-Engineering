namespace Medicines
{
    using AutoMapper;
    using Medicines.Data.Models;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.DataProcessor.ImportDtos;

    public class MedicinesProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public MedicinesProfile()
        {
            CreateMap<ImportPharmacyDto, Pharmacy>();
            CreateMap<Pharmacy, ExportPharmacyDto>();
            CreateMap<Patient, ExportPatientDto>();
        }
    }
}
