using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if(existAccount.Role == Role.student)
                {
                    if (existAccount.Password == PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(loginInformation.Password, existAccount.Salt))
                    {
                        var credential = new Credential(existAccount.Id);
                        _context.Add(credential);
                        _context.SaveChanges();
                        return new JsonResult(credential);
                    }
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult("Bad Request");
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return new JsonResult("Not Found");
        }
    }
}