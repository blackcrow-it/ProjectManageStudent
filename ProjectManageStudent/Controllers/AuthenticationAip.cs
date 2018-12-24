using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManageStudent.Models;

namespace ProjectManageStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAip : ControllerBase
    {
        private readonly ProjectManageStudentContext _context;

        public AuthenticationAip(ProjectManageStudentContext context)
        {
            _context = context;
        }

        // GET: api/AuthenticationAip
        [HttpGet]
        public IEnumerable<Account> GetAccount()
        {
            return _context.Account;
        }

        // GET: api/AuthenticationAip/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/AuthenticationAip/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount([FromRoute] int id, [FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AuthenticationAip
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInformation loginInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existAccount = _context.Account.SingleOrDefault(a => a.Email == loginInformation.Email);
            if (existAccount != null)
            {
                if (existAccount.Password == PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(loginInformation.Password, existAccount.Salt))
                {
                    var credential = new Credential(existAccount.Id);
                    _context.Add(credential);
                    _context.SaveChanges();
                    return new JsonResult(credential);
                }
            }
            return new JsonResult("Not Found");
        }

        // DELETE: api/AuthenticationAip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(account);
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}