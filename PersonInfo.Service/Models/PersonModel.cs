using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonInfo.Service.Models
{
    public class PersonModel
    {
        public int Id { get; set; }

        [Required]
        [NameValidation]
        public string? FirstName { get; set; }

        [Required]
        [NameValidation]
        public string? LastName { get; set; }
        public Gender Gender { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string? PIN { get; set; }

        [Required]
        [MyDateValidation(ErrorMessage = "DoB you entered should be greater than 18 years")]
        public DateTime? DoB  { get; set; }

        public List<PhoneNumberModel>? PhoneNumbers { get; set; }
    }


    public class MyDateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime.TryParse(value.ToString(), out DateTime enteredDateTime);

            if ((DateTime.Now.Year - enteredDateTime.Year) < 18)
                return false;
            return true;
        }
    }

    public class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string name)
            {
                if (name.Length < 2 || name.Length > 50)
                {
                    return new ValidationResult("Name must be between 2 and 50 characters.");
                }

                bool containsGeorgian = Regex.IsMatch(name, @"[\u10A0-\u10FF]+");
                bool containsLatin = Regex.IsMatch(name, @"[A-Za-z]+");

                if (containsGeorgian && containsLatin)
                {
                    return new ValidationResult("Name must contain only Georgian or Latin characters, not both.");
                }

                if (!containsGeorgian && !containsLatin)
                {
                    return new ValidationResult("Name must contain Georgian or Latin characters.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
