using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManageStudent.Models;

namespace ProjectManageStudent.Controllers
{
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;

    public class AccountsController : Controller
    {
        private readonly ProjectManageStudentContext _context;

        public AccountsController(ProjectManageStudentContext context)
        {
            _context = context;
        }

        public bool checkSession()
        {
            var ck = false;
            string currentLogin = HttpContext.Session.GetString("currentLogin");
            var account = this._context.Account.SingleOrDefault(a => a.Email == currentLogin);
            if (currentLogin == null || account.checkRoleST())
            {
                ck = true;
            }

            return (ck);
        }
        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            string currentLogin = HttpContext.Session.GetString("currentLogin");
            if (currentLogin == null)
            {
                return Redirect("/authentication/login");
            }
            var accounts = _context.Account.SingleOrDefault(a => a.Email == currentLogin);
            if (accounts == null || accounts.checkRoleST())
            {
                Response.StatusCode = 403;
                
            }
            var projectManageStudentContext = _context.Account.Include(a => a.Classroom);
            return View(await projectManageStudentContext.ToListAsync());
        }

        public async Task<IActionResult> AddMark(int? id)
        {
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["Subject"] = new SelectList(_context.Subject, "Id", "Name");
            return View(account);

        }
        public async Task<IActionResult> AddMark2(Mark mark )
        {
            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login");
            }
                if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.Classroom)
                .Include(m=>m.Marks).ThenInclude(s=>s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
           
            if (this.checkSession())
            {
                ViewData["ClassroomId"] = new SelectList(_context.Classroom, "Id", "Name");
                return View();
               
            }
            Response.StatusCode = 403;
            return Redirect("/Authentication/Login");
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassroomId,Email,Password,FirstName,LastName,Phone,Address,BirthDay,ConfirmPassword,Role")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.Salt = PasswordHandle.PasswordHandle.GetInstance().GenerateSalt();
                account.Password = PasswordHandle.PasswordHandle.GetInstance()
                    .EncryptPassword(account.Password, account.Salt);
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classroom, "Id", "Id", account.ClassroomId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classroom, "Id", "Name", account.ClassroomId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassroomId,Email,FirstName,LastName,Phone,Address,BirthDay,ConfirmPassword,Role")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    account.Salt = PasswordHandle.PasswordHandle.GetInstance().GenerateSalt();
                    account.Password = PasswordHandle.PasswordHandle.GetInstance()
                        .EncryptPassword(account.Password, account.Salt);
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classroom, "Id", "Id", account.ClassroomId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.Classroom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}
