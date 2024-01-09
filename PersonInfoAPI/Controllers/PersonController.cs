using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonInfo.Model.Models;
using PersonInfo.Model.Models.Enums;
using PersonInfo.Service.Interfaces;
using PersonInfo.Service.Models;

namespace PersonInfoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly IConfiguration configuration;

        public PersonController(IPersonService personService, IConfiguration configuration)
        {
            this.personService = personService;
            this.configuration = configuration;
        }

        [HttpPost("AddPerson")]
        public async Task<ActionResult> AddPerson([FromBody] PersonModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await personService.AddPersonAsync(model);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, PersonModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                await personService.UpdatePersonAsync(model);
                return StatusCode(201);
            }

            return BadRequest();
        }

        [HttpPost("{personId}/AddRelatedPerson")]
        public async Task<ActionResult> AddRelatedPersonAsync([FromRoute] int personId, Relation relationType, PersonModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await personService.AddRelatedPersonAsync(personId, relationType, model);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpDelete("{personId}/DeleteRelatedPerson/{relatedPersonId}")]
        public async Task<ActionResult> DeleteRelatedPerson([FromRoute] int? personId, [FromRoute] int? relatedPersonId)
        {
            if(personId != null && relatedPersonId != null)
            {
                await personService.DeleteRelatedPersonAsync(personId, relatedPersonId);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("Delete/{personId}")]
        public async Task<ActionResult> DeletePerson([FromRoute] int? personId)
        {
            if(personId != null)
            {
                await personService.DeletePersonAsync(personId);
                return Ok();
            }

            return BadRequest();
        }


        [HttpGet("GetFullPersonalInfo/{personId}")]
        public async Task<ActionResult> GetFullPersonalInfo([FromRoute] int? personId)
        {
            if (personId != null)
            {
                var result = await personService.GetFullPersonalInfoById(personId);
                return Ok(result);
            }

            return BadRequest();
        }


        [HttpPost("GetAllPeopleByQuickSearch")]
        public async Task<ActionResult> GetAllPeopleByQuickSearch([FromBody] QuickSearchPayload quickSearchayload)
        {
            var result = await personService.GetAllPeopleByQuickSearch(quickSearchayload);
            return Ok(result);
        }

        [HttpPost("GetAllPeopleByDetailSearch")]
        public async Task<ActionResult> GetAllPeopleByDetailSearch([FromBody] DetailSearchPayload payload)
        {
            var result = await personService.GetAllPeopleByDetailSearch(payload);
            return Ok(result);
        }

    }
}
