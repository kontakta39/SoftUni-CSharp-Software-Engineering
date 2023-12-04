namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            var xmlParser = new XmlParser();

            DateTime givenDate = Convert.ToDateTime(date);

            var patients = context.Patients
                .AsEnumerable()
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > givenDate))
                .Select(p => new ExportPatientDto()
                {
                    Gender = p.Gender.ToString().ToLower(),
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup,
                    Medicines = p.PatientsMedicines
                        .Where(p => p.Medicine.ProductionDate > givenDate)
                        .OrderByDescending(p => p.Medicine.ExpiryDate)
                        .ThenBy(p => p.Medicine.Price)
                        .Select(pm => new MedicineDto()
                        {
                            Category = pm.Medicine.Category.ToString().ToLower(),
                            Name = pm.Medicine.Name,
                            Price = $"{pm.Medicine.Price:f2}",
                            Producer = pm.Medicine.Producer,
                            BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })
                        .ToArray()
                })
                .OrderByDescending(p => p.Medicines.Count())
                .ThenBy(p => p.Name)
                .ToArray();

            var result = xmlParser.Serialize<ExportPatientDto>(patients, "Patients");
            return result;
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            Category categoryEnum = (Category)medicineCategory;

            var medicines = context.Medicines
                .AsEnumerable()
                .Where(m => m.Category == categoryEnum && m.Pharmacy.IsNonStop == true)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = $"{m.Price:f2}",
                    Pharmacy = new ExportPharmacyDto()
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber
                    }
                })
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .ToList();

            return JsonConvert.SerializeObject(medicines, Formatting.Indented);
        }
    }
}
