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
    public class ApiHandleController : ControllerBase
    {
        private readonly ProjectManageStudentContext _context;

        public ApiHandleController(ProjectManageStudentContext context)
        {
            _context = context;
        }
        // lấy ra thông tin sinh viên
        [HttpGet("Information")]
        public async Task<IActionResult> Information()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var basicToken = Request.Headers["Authorization"].ToString();
            var token = basicToken.Replace("Basic ", "");
            var existToken = _context.Credential.SingleOrDefault(a => a.AccessToken == token);
            if (existToken != null)
            {
                if (existToken.Status == CredentialStatus.Active && existToken.ExpiredAt >= DateTime.Now)
                {
                    var Id = existToken.OwnerId;
                    var existAccount = _context.Account.SingleOrDefault(a => a.Id == Id);
                    if (existAccount != null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return new JsonResult(existAccount);
                    }
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult("Forbidden");
                }
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Not Found");
        }
        [HttpPost("Information")]
        public async Task<IActionResult> UpdatePassword(ChangePassword changePassword)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var basicToken = Request.Headers["Authorization"].ToString();
            var token = basicToken.Replace("Basic ", "");
            var existToken = _context.Credential.SingleOrDefault(a => a.AccessToken == token);
            if (existToken != null)
            {
                var existAccount = _context.Account.SingleOrDefault(i => i.Id == existToken.OwnerId);
                if (existAccount != null)
                {
                    if (existAccount.Password == PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(changePassword.Password, existAccount.Salt))
                    {
                        var encryptNewPassword = PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(changePassword.NewPassword, existAccount.Salt);
                        existAccount.Password = encryptNewPassword;
                        existAccount.UpdateAt = DateTime.Now;
                        _context.Account.Update(existAccount);
                        _context.SaveChanges();
                        return new JsonResult(existAccount);
                    }
                    return new JsonResult(changePassword);
                }
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Not Found");
        }
        // changeInformation
        // còn address và avatar
        [HttpPost("changeInformation")]
        public async Task<IActionResult> ChangeInformation(ChangeInformation changeInformation)
        {

            if (!ModelState.IsValid)
            {
                return new JsonResult("demo");
            }
            var basicToken = Request.Headers["Authorization"].ToString();
            var token = basicToken.Replace("Basic ", "");
            var existToken = _context.Credential.SingleOrDefault(a => a.AccessToken == token);
            if (existToken != null)
            {
                var existAccount = _context.Account.SingleOrDefault(i => i.Id == existToken.OwnerId);

                if (existAccount != null)
                {
                    if (existAccount.Phone != null)
                    {
                        if(changeInformation.Phone == existAccount.Phone)
                        {
                            var phone = changeInformation.NewPhone;
                            existAccount.Phone = phone;
                            _context.Account.Update(existAccount);
                            _context.SaveChanges();
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            return new JsonResult(existAccount);
                        }

                        return new JsonResult("BadRequest");
                    }

                }
                return new JsonResult(existAccount);
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Forbidden");
        }
        //Doing
        //[HttpGet("ListStudentInClass")]
        //public IEnumerable<Account> ListStudent(string classroom)
        //{
        //    return _context.Account;
        //}
        [HttpPost("ListStudentInClass")]
        public async Task<IActionResult> ListStudentInClass(Account account)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var basicToken = Request.Headers["Authorization"].ToString();
            var token = basicToken.Replace("Basic ", "");
            var existToken = _context.Credential.SingleOrDefault(a => a.AccessToken == token);
            if (existToken != null)
            {
                var existAccount = _context.Account.SingleOrDefault(i => i.Id == existToken.OwnerId);
                if (existAccount != null)
                {
                }
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Not Found");
        }
        //[HttpGet("Subject/{account}")]
        //public IEnumerable<Subject> ListSubject(string Account)
        //{
        //    return _context.Subject;
        //}

        //[HttpGet("Mark")]
        //public IEnumerable<Mark> Mark(string Account)
        //{
        //    return _context.Mark;
        //}

    }
}