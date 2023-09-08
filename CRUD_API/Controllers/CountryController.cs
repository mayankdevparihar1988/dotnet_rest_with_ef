using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services;
using ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUD_PRACTIVE_HARSHWARDHAN.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CountryController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        // GET: api/values
        [HttpGet]
        public ActionResult<List<CountryResponse>> Get()
        {
           return _countriesService.GetAllCountries();
        }

        // GET api/values/5
        [HttpGet("{countryGuid}")]
        public ActionResult<CountryResponse?> Get(Guid countryGuid)
        {
            return _countriesService.GetCountryByCountryID(countryGuid);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<CountryResponse>> Post([FromBody] CountryAddRequest countryAddRequest)
        {
            return await _countriesService.AddCountryAsync(countryAddRequest);
        }

     
    }
}

