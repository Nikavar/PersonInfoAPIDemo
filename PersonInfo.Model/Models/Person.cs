using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Model.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public string? PIN { get; set; }

        [Required]
        public DateTime? DoB  { get; set; }      


        // relations
        public ICollection<PhoneNumber>? PhoneNumbers { get; set; }
        public ICollection<PersonRelatedPeople>? PersonRelatedPeople { get; set; }
    }

}
