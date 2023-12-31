﻿using System;
using Entity;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;

namespace Services
{
	
        public class PersonsService : IPersonsService
        {
            //private field
            private readonly PesonsDbContext _pesonsDbContext;
            private readonly ICountriesService _countriesService;

            //constructor
            public PersonsService(PesonsDbContext pesonsDbContext, ICountriesService countriesService)
            {

            _pesonsDbContext = pesonsDbContext;
            _countriesService = countriesService;
                
            }

        


        private PersonResponse ConvertPersonToPersonResponse(Person person)
            {
                PersonResponse personResponse = person.ToPersonResponse();
                personResponse.Country = person.Country?.CountryName;
                return personResponse;
            }

            public async Task< PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
            {
                //check if PersonAddRequest is not null
                if (personAddRequest == null)
                {
                    throw new ArgumentNullException(nameof(personAddRequest));
                }

                //Model validation
                ValidationHelper.ModelValidation(personAddRequest);

                //convert personAddRequest into Person type
                Person person = personAddRequest.ToPerson();

                //generate PersonID
                person.PersonID = Guid.NewGuid();

            //add person object to persons list
             _pesonsDbContext.Persons.Add(person);
             await _pesonsDbContext.SaveChangesAsync();

            // USING STORED PROCEDURE
            //_pesonsDbContext.sp_InsertPerson(person);

            //convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }


            public async Task<List<PersonResponse>> GetAllPersons()
            {

            // By Default Country object is null
            // var persons = _pesonsDbContext.Persons.ToList();

            // Include Countries

            var persons = await _pesonsDbContext.Persons.Include("Country").ToListAsync();

            // return persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            // return _pesonsDbContext.sp_GetAllPersons().Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return persons.Select(person => person.ToPersonResponse()).ToList();
            }


            public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
            {
                if (personID == null)
                    return null;

                Person? person = await _pesonsDbContext.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);
                if (person == null)
                    return null;

                return person.ToPersonResponse();
            }

            public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
            {
                List<PersonResponse> allPersons = await GetAllPersons();
                List<PersonResponse> matchingPersons = allPersons;

                if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                    return matchingPersons;


                switch (searchBy)
                {
                    case nameof(Person.PersonName):
                        matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.PersonName) ?
                        temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                        break;

                    case nameof(Person.Email):
                        matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Email) ?
                        temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                        break;


                    case nameof(Person.DateOfBirth):
                        matchingPersons = allPersons.Where(temp =>
                        (temp.DateOfBirth != null) ?
                        temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;

                    case nameof(Person.Gender):
                        matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Gender) ?
                        temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                        break;

                    case nameof(Person.CountryID):
                        matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Country) ?
                        temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                        break;

                    case nameof(Person.Address):
                        matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Address) ?
                        temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                        break;

                    default: matchingPersons = allPersons; break;
                }
                return matchingPersons;
            }


            public  List<PersonResponse>GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
            {
                if (string.IsNullOrEmpty(sortBy))
                    return allPersons;

                List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                    _ => allPersons
                };

                return sortedPersons;
            }


            public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
            {
                if (personUpdateRequest == null)
                    throw new ArgumentNullException(nameof(Person));

                //validation
                ValidationHelper.ModelValidation(personUpdateRequest);

                //get matching person object to update
                Person? matchingPerson = await _pesonsDbContext.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);
                if (matchingPerson == null)
                {
                    throw new ArgumentException("Given person id doesn't exist");
                }

                //update all details
                matchingPerson.PersonName = personUpdateRequest.PersonName;
                matchingPerson.Email = personUpdateRequest.Email;
                matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
                matchingPerson.Gender = personUpdateRequest.Gender.ToString();
                matchingPerson.CountryID = personUpdateRequest.CountryID;
                matchingPerson.Address = personUpdateRequest.Address;
                matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

                await _pesonsDbContext.SaveChangesAsync();
                return matchingPerson.ToPersonResponse();
            }

            public async Task<bool> DeletePerson(Guid? personID)
            {
                if (personID == null)
                {
                    throw new ArgumentNullException(nameof(personID));
                }

                Person? person = await _pesonsDbContext.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);
                if (person == null)
                    return false;

                _pesonsDbContext.Persons.Remove(person);


                await _pesonsDbContext.SaveChangesAsync();

                return true;
            }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>(); //PersonID,PersonName,...
            csvWriter.NextRecord();

            List<PersonResponse> persons = await _pesonsDbContext.Persons
              .Include("Country")
              .Select(temp => temp.ToPersonResponse()).ToListAsync();

            await csvWriter.WriteRecordsAsync(persons);
            //1,abc,....


            // Before sending put the buffer reader position at start
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
    }


