using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonInfo.Data.Infrastructure;
using PersonInfo.Data.Repositories;
using PersonInfo.Model.Models;
using PersonInfo.Model.Models.Enums;
using PersonInfo.Service.Interfaces;
using PersonInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRelatedPeopleRepository personRelatedPeopleRepository;
        private readonly IPhoneNumberRepository phoneNumberRepository;
        private readonly IPersonRepository personRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public PersonService(IPersonRelatedPeopleRepository personRelatedPeopleRepository, IPhoneNumberRepository phoneNumberRepository, IPersonRepository personRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            this.personRelatedPeopleRepository = personRelatedPeopleRepository;
            this.phoneNumberRepository = phoneNumberRepository; 
            this.personRepository = personRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<PersonModel> AddPersonAsync(PersonModel model)
        {
            var personsWithSamePin = await personRepository.GetManyAsync(p => p.PIN == model.PIN);
            if (personsWithSamePin.Count() > 0)
            {
                throw new AlreadyExistsException(nameof(Person), model.PIN);
            }

            else
            {
                var entity = mapper.Map<Person>(model);
                var addedPerson = await personRepository.AddAsync(entity);

                if (model.PhoneNumbers != null)
                {
                    var phoneEntity = new PhoneNumber();

                    foreach (var phone in model.PhoneNumbers.ToList())
                    {
                        phoneEntity = mapper.Map<PhoneNumber>(phone);
                        phoneEntity.PersonId = addedPerson.Id;
                        await phoneNumberRepository.AddAsync(phoneEntity);
                    }
                }

                model = await GetPersonInfo(addedPerson);
                if(model is null)
                    throw new NotFoundException(nameof(PersonModel), addedPerson.PIN);

            }

            return model;
        }
        public async Task UpdatePersonAsync(PersonModel model)
        {
            var person = await personRepository.GetByIdAsync(model.Id);

            if(person is null)
                throw new NotFoundException(nameof(Person), model.Id);

            else
            {
                var phones = await phoneNumberRepository.GetManyAsync(phone => phone.PersonId == person.Id);
                var entity = mapper.Map<Person>(model);
                await personRepository.UpdateAsync(entity);

                if(phones.ToList().Count > 0)
                {
                    foreach (var phone in phones.ToList())
                    {
                        var phoneNumber = mapper.Map<PhoneNumber>(phone);
                        await phoneNumberRepository.UpdateAsync(phoneNumber);
                    }
                }
            }
        }

        public async Task<PersonModel> AddRelatedPersonAsync(int personId, Relation relationType, PersonModel model)
        {
            var newRelatedPerson = await AddPersonAsync(model);

            var personRelatedPeople = new PersonRelatedPeople()
            { 
                RelatedPersonId = newRelatedPerson.Id,
                ParentId = personId,
                RelationTypeId = (int)relationType      
            };

            await personRelatedPeopleRepository.AddAsync(personRelatedPeople);
            return newRelatedPerson;
        }

        public async Task DeletePersonAsync(int? personId)
        {
            var personForDelete = await personRepository.GetByIdAsync(personId);
            if (personForDelete is null)
            {
                throw new NotFoundException(nameof(Person), personId);
            }

            else
            {
                var relatedPeople = await personRelatedPeopleRepository.GetManyAsync(prp => prp.ParentId == personForDelete.Id); 

                if (relatedPeople.Count() > 0)
                {
                    await phoneNumberRepository.DeleteManyAsync(pn => pn.PersonId == personForDelete.Id);
                    await personRelatedPeopleRepository.DeleteManyAsync(prp => prp.ParentId == personForDelete.Id);
                }

                await phoneNumberRepository.DeleteManyAsync(pn => pn.PersonId == personForDelete.Id);
                await personRepository.DeleteAsync(personForDelete);
            }
        }

        public async Task DeleteRelatedPersonAsync(int? personId, int? relatedPersonId)
        {

            await personRelatedPeopleRepository.DeleteManyAsync(x => x.ParentId == personId && x.RelatedPersonId == relatedPersonId);
            var relatedPeople = await personRelatedPeopleRepository.GetAllAsync();

            bool hasReference = false;

            foreach (var relatedPerson in relatedPeople.ToList())
            {

                if(relatedPerson.RelatedPersonId != relatedPersonId)
                {
                    hasReference = false;
                }

                else
                {
                    hasReference = true;
                    break;
                }                 
            } 
            
            if (!hasReference)
            {
                var entity = await personRepository.GetByIdAsync(relatedPersonId);
                await phoneNumberRepository.DeleteManyAsync(pn => pn.PersonId == relatedPersonId);
                await personRepository.DeleteAsync(entity);
            }

        }

        public async Task<PersonRelatedPeopleModel> GetFullPersonalInfoById(int? Id)
        {
            var personEntity = await personRepository.GetByIdAsync(Id);

            if (personEntity == null)
                throw new NotFoundException(nameof(Person), Id);


            var model = await GetPersonInfo(personEntity);

            var personRelatedPeopleEntities = await personRelatedPeopleRepository.GetManyAsync(prp => prp.ParentId == Id);
            List<PersonModel> relatedPeopleModel = new List<PersonModel>();

            foreach (var entity in personRelatedPeopleEntities)
            {
                var relatedPerson = await personRepository.GetByIdAsync(entity.RelatedPersonId);
                var relatedPersonModel = await GetPersonInfo(relatedPerson);

                relatedPeopleModel.Add(relatedPersonModel);
            }

            var prpModel = new PersonRelatedPeopleModel()
            {
                Person = model,
                RelatedPeopleList = relatedPeopleModel,
            };

            return prpModel;
        }

        public async Task<IEnumerable<PersonModel>> GetAllPeopleByQuickSearch(QuickSearchPayload payload)
        {
            var properties = new List<(string PropertyName, object Value)>
                            {
                                (nameof(QuickSearchPayload.FirstName), payload.FirstName),
                                (nameof(QuickSearchPayload.LastName), payload.LastName),
                                (nameof(QuickSearchPayload.PIN), payload.PIN)
                            };

            var filters = properties
                .Where(prop => !string.IsNullOrEmpty(prop.Value?.ToString()))
                .ToDictionary(prop => prop.PropertyName, prop => prop.Value);

            var filterExpression = BuildFilterExpression(filters);

            var persons = await personRepository.GetManyAsync(filterExpression);
            return mapper.Map<List<PersonModel>>(persons);
        }

        public async Task<IEnumerable<PersonModel>> GetAllPeopleByDetailSearch(DetailSearchPayload payload)
        {
            var properties = new List<(string PropertyName, object Value)>
            {
                (nameof(DetailSearchPayload.FirstName), payload.FirstName?.ToLower()),
                (nameof(DetailSearchPayload.LastName), payload.LastName?.ToLower()),
                (nameof(DetailSearchPayload.PIN), payload.PIN?.ToLower()),
                (nameof(DetailSearchPayload.Gender), payload.Gender != default ? (object)payload.Gender : null),
                (nameof(DetailSearchPayload.DoB), payload.DoB != default(DateTime) ? (object)payload.DoB.Date : null)
                //(nameof(DetailSearchPayload.PhoneNumber), payload.PhoneNumber?.ToLower())
            };

            var filters = properties
                .Where(prop => prop.Value != null)
                .ToDictionary(prop => prop.PropertyName, prop => prop.Value);

            var filterExpression = BuildFilterExpression(filters);

            var allFilteredPersons = await personRepository.GetManyAsync(filterExpression);

            var pagedPersons = allFilteredPersons
                                .Skip((payload.PageNumber - 1) * payload.PageSize)
                                .Take(payload.PageSize);

            return mapper.Map<List<PersonModel>>(pagedPersons);
        }
        private Expression<Func<Person, bool>> BuildFilterExpression(Dictionary<string, object> filters)
        {
            var parameterExp = Expression.Parameter(typeof(Person), "p");
            Expression predicateExp = Expression.Constant(true);

            foreach (var filter in filters)
            {
                var propertyExp = Expression.Property(parameterExp, filter.Key);
                Expression currentExp;

                if (filter.Value is string stringValue)
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var loweredPropertyExp = Expression.Call(propertyExp, toLowerMethod);
                    var loweredValueExp = Expression.Constant(stringValue.ToLower());
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    currentExp = Expression.Call(loweredPropertyExp, containsMethod, loweredValueExp);
                }
                else if (Nullable.GetUnderlyingType(propertyExp.Type) != null && filter.Value != null)
                {
                    var nonNullablePropertyType = Nullable.GetUnderlyingType(propertyExp.Type);
                    var convertedPropertyExp = Expression.Convert(propertyExp, nonNullablePropertyType);
                    var valueExp = Expression.Constant(filter.Value, nonNullablePropertyType);
                    currentExp = Expression.Equal(convertedPropertyExp, valueExp);
                }
                else
                {
                    var valueExp = Expression.Constant(filter.Value, filter.Value.GetType());
                    currentExp = Expression.Equal(propertyExp, valueExp);
                }

                predicateExp = Expression.AndAlso(predicateExp, currentExp);
            }

            return Expression.Lambda<Func<Person, bool>>(predicateExp, parameterExp);
        }

        public async Task<PersonModel> GetPersonInfo(Person person)
        {    
            PersonModel model = mapper.Map<PersonModel>(person);
            var phoneLst = await phoneNumberRepository.GetManyAsync(p => p.PersonId == person.Id);
            model.PhoneNumbers = mapper.Map<List<PhoneNumber>, List<PhoneNumberModel>>(phoneLst.ToList());
            return model;
        }
    }
}




