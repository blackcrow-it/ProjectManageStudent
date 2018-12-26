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
        [HttpGet("information-student")]
        public async Task<IActionResult> InformationStudent()
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
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return new JsonResult(existAccount);
                    }
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult("Forbidden");
                }
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Not Found");
        }
//thay đổi thông tin sinh viên
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
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
//thay doi thong tin sinh vien        
        [HttpPost("change-information")]
        public async Task<IActionResult> ChangeInformation(ChangeInformation changeInformation)
        {

            if (!ModelState.IsValid)
            {
                return new JsonResult("BadRequest");
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
                        var address = changeInformation.NewAddress;                        
                        existAccount.Address = address;

                        var phone = changeInformation.Phone;
                        existAccount.Phone = phone;

                        var avatar = changeInformation.Avatar;
                        existAccount.Avartar = avatar;

                        _context.Account.Update(existAccount);
                        _context.SaveChanges();
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return new JsonResult(existAccount);

                    }

                }
                return new JsonResult(existAccount);
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Forbidden");
        }
 // danh sach sinh vien trong 1 lop
        [HttpPost("list-student")]
        public async Task<IActionResult> ListStudent(list_student_classroom classroomId)
        {

            var existClassroom = _context.Classroom.Where(c => c.Id == classroomId.classroomId).Select(n=>n.Accounts).FirstOrDefault();
            
            return new JsonResult(existClassroom);
        }
        //danh sach diem cua 1 sinh vien
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