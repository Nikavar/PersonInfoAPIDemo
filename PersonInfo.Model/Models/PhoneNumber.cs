using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Model.Models
{
    public class PhoneNumber
    {
        [Key]
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public int? PhoneTypeId { get; set; }

        [MinLength(4),MaxLength(50)]
        public string? PhoneNum { get; set; }


        //relations
        public virtual Person? Person { get; set; }
        public virtual PhoneType? PhoneTypes { get; set; }

    }
}
