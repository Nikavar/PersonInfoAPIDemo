using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service.Models
{
    public class PhoneNumberModel
    {
        //public int PersonId { get; set; }
        public int? PhoneTypeId { get; set; }       

        [MinLength(4),MaxLength(50)]
        public string? PhoneNum { get; set; }

    }
}
