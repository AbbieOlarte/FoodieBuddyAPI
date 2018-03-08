using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodieBuddy.API.Utils;
using FoodieBuddy.Domain.MailingList;
using FoodieBuddy.Domain.Models.MailingList;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FoodieBuddy.API.Controllers
{
    [EnableCors("FoodieBuddyApp")]
    [Produces("application/json")]
    [Route("api/Mailing")]
    public class MailingController : Controller
    {
        private IMailingRepository mailingRepository;
        private IMailingService mailingService;

        public MailingController(IMailingRepository mailingRepository, IMailingService mailingService)
        {
            this.mailingRepository = mailingRepository;
            this.mailingService = mailingService;
        }

        [HttpGet]
        public IActionResult GetMail(Guid? id)
        {
            var result = new List<Mail>();
            if (id == null)
            {
                result.AddRange(this.mailingRepository.Retrieve());
            }
            else
            {
                var mail = this.mailingRepository.Retrieve(id.Value);
                result.Add(mail);
            }
            
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateMail([FromBody] Mail mail)
        {
            try
            {
                if (mail == null)
                {
                    return BadRequest();
                }
                var result = this.mailingService.Save(Guid.Empty, mail);
                return CreatedAtAction("GetMail", new { id = mail.MailingId }, mail);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteMail(Guid id)
        {
            var mailToDelete = this.mailingRepository.Retrieve(id);
            if (mailToDelete == null)
            {
                return NotFound();
            }
            this.mailingRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateMail([FromBody] Mail mail, Guid id)
        {
            try
            {
                if (mail == null)
                {
                    return BadRequest();
                }
                var existingMail = this.mailingRepository.Retrieve(id);
                if (existingMail == null)
                {
                    return NotFound();
                }
                existingMail.ApplyChanges(mail);
                var result = this.mailingService.Save(id, existingMail);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public object PatchMail(JsonPatchDocument patchedMail, Guid id)
        {
            if (patchedMail == null)
            {
                return BadRequest();
            }
            var mail = mailingRepository.Retrieve(id);
            if (mail == null)
            {
                return NotFound();
            }
            patchedMail.ApplyTo(mail);
            mailingService.Save(id, mail);
            return Ok(mail);
        }
    }
}