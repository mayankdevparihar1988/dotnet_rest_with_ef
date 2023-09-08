using System;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace CRUD_TESTS
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PesonsDbContext(new DbContextOptionsBuilder<PesonsDbContext>().Options));
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                //Act
               return _countriesService.AddCountryAsync(request);
            });
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _countriesService.AddCountryAsync(request);
            });
        }


        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountryAsync(request1);
                _countriesService.AddCountryAsync(request2);
            });
        }


        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetailsAsync()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse response = await _countriesService.AddCountryAsync(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion


        #region GetAllCountries

        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountriesAsync()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() {
        new CountryAddRequest() { CountryName = "USA" },
        new CountryAddRequest() { CountryName = "UK" }
      };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountryAsync(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }
        #endregion


        #region GetCountryByCountryID

        [Fact]
        //If we supply null as CountryID, it should return null as CountryResponse
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countrID = null;

            //Act
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryID(countrID);


            //Assert
            Assert.Null(country_response_from_get_method);
        }


        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryByCountryID_ValidCountryIDAsync()
        {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add = await _countriesService.AddCountryAsync(country_add_request);

            //Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}

