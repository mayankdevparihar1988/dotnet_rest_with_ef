using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

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
        public ActionResult<List<PersonResponse>> Get()
        {
            return _personService.GetAllPersons();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<PersonResponse?> Get(Guid id)
        {
            return _personService.GetPersonByPersonID(id);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<PersonResponse?> Post([FromBody] PersonAddRequest personAddRequest)
        {
            return _personService.AddPerson(personAddRequest);
        }

        // PUT api/values/5
        [HttpPut]
        public ActionResult<PersonResponse?> Put([FromBody] PersonUpdateRequest personUpdateRequest)
        {

            return _personService.UpdatePerson(personUpdateRequest);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {

            return _personService.DeletePerson(id);
        }
    }
}

