using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service.Models
{
    public class PersonRelatedPeopleModel
    {
        public PersonModel? Person { get; set; }
        public List<PersonModel>? RelatedPeopleList { get; set; }
    }    
}
