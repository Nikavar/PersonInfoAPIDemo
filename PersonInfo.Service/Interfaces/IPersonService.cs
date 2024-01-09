using PersonInfo.Model.Models;
using PersonInfo.Model.Models.Enums;
using PersonInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service.Interfaces
{
    public interface IPersonService
    {
        Task<PersonModel> AddPersonAsync(PersonModel model);
        Task UpdatePersonAsync (PersonModel model);
        Task<PersonModel> AddRelatedPersonAsync(int personId, Relation relationType, PersonModel model);
        Task DeleteRelatedPersonAsync(int? personId, int? relatedPersonId);
        Task DeletePersonAsync(int? personId);
        Task<PersonRelatedPeopleModel> GetFullPersonalInfoById(int? personId);
        Task<IEnumerable<PersonModel>> GetAllPeopleByQuickSearch(QuickSearchPayload payload);
        Task<IEnumerable<PersonModel>> GetAllPeopleByDetailSearch(DetailSearchPayload payload);
        Task<PersonModel> GetPersonInfo(Person person);
    }
}
