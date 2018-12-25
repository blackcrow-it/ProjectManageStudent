using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManageStudent.Controllers
{
    using ProjectManageStudent.Models;

    public class AuthenticationController : Controller
    {
        private readonly ProjectManageStudentContext _context;

        public AuthenticationController(ProjectManageStudentContext context)
        {
            _context = context;
        }
        
        public ActionResult Login(string Url)
        {
            string Login = HttpContext.Session.GetString("currentLogin");
            if (Login != null)
            {
                return Redirect("/Home/About");
            }

            ViewData["Url"] = Url;
            return View();
                
        }

        // GET: Authentication/Details/5
        [HttpPost]
        public IActionResult Login(Account account , string Url )
        {
            var existAccount = _context.Account.SingleOrDefault(a => a.Email == account.Email);
            if (existAccount != null)
            {
                if (existAccount.Password == PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(account.Password, existAccount.Salt))
                {
                    HttpContext.Session.SetString("currentLogin", existAccount.Email);
                    HttpContext.Session.SetString("currentLoginId", existAccount.Id.ToString());
                    HttpContext.Session.SetString("currentLoginRole", existAccount.Role.ToString());
                    if (Url !=null)
                    {
                        return Redirect(Url);
                    }
                    return Redirect("/accounts/Index");
                }
            }
         return View(account);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("currentLogin");
            HttpContext.Session.Remove("currentLoginId");
            HttpContext.Session.Remove("currentLoginRole");
            return Redirect("/Home/Index");
        }
    }
}