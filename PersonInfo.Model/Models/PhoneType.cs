using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Model.Models
{
    public class PhoneType
    {
        [Key]
        public int Id { get; set; }
        public PhoneNumberType PhoneNumberType { get; set; }

        //relations
        public ICollection<PhoneNumber>? PhoneNumbers { get; set; }
    }
}
