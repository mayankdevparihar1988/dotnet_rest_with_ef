using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUD_PRACTIVE_HARSHWARDHAN.Controllers
{
    [Route("api/[controller]")]
    public class PersonsController : Controller
    {
       
       private readonly IPersonsService _personService;

       public PersonsController(IPersonsService personService)
       {
        _personService = personService;
       }
       
        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<List<PersonResponse>>> Get()
        {
            return await _personService.GetAllPersons();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonResponse?>> Get(Guid id)
        {
            return await _personService.GetPersonByPersonID(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<PersonResponse?>> Post([FromBody] PersonAddRequest personAddRequest)
        {
            return await _personService.AddPerson(personAddRequest);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult<PersonResponse?>> Put([FromBody] PersonUpdateRequest personUpdateRequest)
        {

            return await _personService.UpdatePerson(personUpdateRequest);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {

            return await _personService.DeletePerson(id);
        }

        [HttpGet("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }
    }
}

