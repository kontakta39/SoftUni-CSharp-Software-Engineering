namespace Medicines.DataProcessor
{
    using AutoMapper;
    using Medicines.Utilities;
    using Medicines.Data;
    using Medicines.DataProcessor.ImportDtos;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Medicines.Data.Models;
    using Microsoft.Extensions.Primitives;
    using System;
    using Medicines.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        private static IMapper GetMapper()
        {
            var mapper = new MapperConfiguration(c => c.AddProfile<MedicinesProfile>());
            return new Mapper(mapper);
        }

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var mapper = GetMapper();

            var inputPatients = JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);
            List<Patient> patients = new();
            StringBuilder sb = new();
            List<int> medicinesIds = new();

            foreach (var inputPatient in inputPatients)
            {
                if (inputPatient.AgeGroup >= 3 || inputPatient.AgeGroup < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (inputPatient.Gender >= 2 || inputPatient.Gender < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(inputPatient))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new()
                {
                    FullName = inputPatient.FullName,
                    AgeGroup = (AgeGroup)inputPatient.AgeGroup,
                    Gender = (Gender)inputPatient.Gender
                };

                foreach (var currentMedicineId in inputPatient.Medicines)
                {
                    if (medicinesIds.Contains(currentMedicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    else
                    {
                        patient.PatientsMedicines.Add(new PatientMedicine()
                        {
                            MedicineId = currentMedicineId
                        });

                        medicinesIds.Add(currentMedicineId);
                    }
                }

                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count()));
                patients.Add(patient);
                medicinesIds.Clear();
            }

            context.Patients.AddRange(patients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            IMapper mapper = GetMapper();
            XmlParser xmlParser = new XmlParser();

            ImportPharmacyDto[] pharmacyDTOs = xmlParser.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");
            List<Pharmacy> pharmacies = new();
            List<Medicine> medicines = new();
            StringBuilder sb = new();
            bool ifExists = false;

            foreach (var pharmacyDTO in pharmacyDTOs)
            {
                if (!IsValid(pharmacyDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (pharmacyDTO.IsNonStop != "true" && pharmacyDTO.IsNonStop != "false")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = pharmacyDTO.Name,
                    PhoneNumber = pharmacyDTO.PhoneNumber,
                    IsNonStop = bool.Parse(pharmacyDTO.IsNonStop)
                };

                foreach (var currentMedicine in pharmacyDTO.Medicines)
                {
                    if (!IsValid(currentMedicine) || string.IsNullOrEmpty(currentMedicine.ProductionDate) || string.IsNullOrEmpty(currentMedicine.ExpiryDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (Convert.ToInt32(currentMedicine.Category) >= 5)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currentCategory = (Category)Enum.Parse(typeof(Category), currentMedicine.Category);

                    Medicine medicine = new Medicine()
                    {
                        Name = currentMedicine.Name,
                        Price = currentMedicine.Price,
                        ProductionDate = Convert.ToDateTime(currentMedicine.ProductionDate),
                        ExpiryDate = Convert.ToDateTime(currentMedicine.ExpiryDate),
                        Producer = currentMedicine.Producer,
                        Category = currentCategory
                    };

                    if (medicine.ProductionDate >= medicine.ExpiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    foreach (var medicineCheck in medicines)
                    {
                        if (medicineCheck.Name == medicine.Name && medicineCheck.Producer == medicine.Producer)
                        {
                            sb.AppendLine(ErrorMessage);
                            ifExists = true;
                            break;
                        }
                    }

                    if (ifExists == false)
                    {
                        pharmacy.Medicines.Add(medicine);
                        medicines.Add(medicine);
                    }

                    ifExists = false;
                }

                pharmacies.Add(pharmacy);
                medicines.Clear();
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count()));
            };

            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
