using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Model.Models
{
    public class RelationType
    {
        [Key]
        public int Id { get; set; }
        public Relation Relation { get; set; }


        // relations
        public ICollection<PersonRelatedPeople>? PersonRelatedPeoples { get; set; }
    }

}
