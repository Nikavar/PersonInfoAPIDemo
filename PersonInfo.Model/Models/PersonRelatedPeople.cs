using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Model.Models
{
    public class PersonRelatedPeople
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Person")]
        public int RelatedPersonId { get; set; }

        [ForeignKey("RelatedPerson")]
        public int ParentId { get; set; }

        [ForeignKey("RelationType")]
        public int RelationTypeId { get; set; }

        //relations

        public virtual Person? Person { get; set; }      
        public virtual Person? RelatedPerson { get; set; }
        public virtual RelationType? RelationType { get; set; }

    }
}
