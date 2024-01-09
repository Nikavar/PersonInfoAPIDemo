using AutoMapper;
using PersonInfo.Model.Models;
using PersonInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonModel, Person>()
                .ForMember(p => p.PersonRelatedPeople, option => option.Ignore())
                .ForMember(p => p.PhoneNumbers, option => option.Ignore());

            CreateMap<Person, PersonModel>()
                .ForMember(pm => pm.PhoneNumbers, option => option.Ignore());

            CreateMap<PhoneNumberModel, PhoneNumber>()
                .ForMember(pn => pn.PhoneTypes, option => option.Ignore())
                .ForMember(pn => pn.Person, option => option.Ignore())
                .ForMember(pn => pn.PersonId, option => option.Ignore())
                .ForMember(pn => pn.Id, option => option.Ignore());

            CreateMap<PhoneNumber, PhoneNumberModel>();

            CreateMap<PersonRelatedPeople, PersonRelatedPeopleModel>().ReverseMap();
        }
    }
}
