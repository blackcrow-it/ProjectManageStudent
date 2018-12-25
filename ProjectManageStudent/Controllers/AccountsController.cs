using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManageStudent.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using System.Net;

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
        public async Task<IActionResult> Index(string sortOrder )
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            IQueryable<Account> studentIQ = from s in _context.Account
                                            select s;
            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentIQ = studentIQ.OrderBy(s => s.BirthDay);
                    break;
                case "date_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.CreateAt);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
            }
            var projectManageStudentContext = _context.Account.Include(a => a.Classroom);
            return View(await projectManageStudentContext.ToListAsync());
        }

        public async Task<IActionResult> AddMark(int id , Mark mark)
        {
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                              .Include(a => a.Classroom)
                              .Include(m => m.Marks).ThenInclude(s => s.Subject)
                              .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            List<Subject> fundList = _context.Subject.ToList();
            ViewBag.Funds = fundList;
            ViewData["userId"] = id;
            ViewData["Subject"] = new SelectList(_context.Subject, "Id", "Name" , mark.SubjectId);
            return View(mark);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMark2(Mark mark ,int Subject ,int Account)
        {
            
            var exisMark = _context.Mark.Where(q=>q.SubjectId == Subject).Where(a=>a.AccountId == Account).Select(nv=>nv.AccountId == Account).FirstOrDefault();
            if (exisMark)
            {
                return Json("Đã có");
            }
            if (ModelState.IsValid )
            {
                int max = 35;
                float total = (mark.Theory + mark.Assignment +mark.Practice)/max*100;
                if (total >= 14)
                {
                    mark.Status = MarkStatus.Pass;
                }
                else
                {
                    mark.Status = MarkStatus.Fail;
                }
                if (mark.Theory == -1 || mark.Assignment == -1 || mark.Practice == -1)
                {
                    mark.Status = MarkStatus.Null;
                }
                _context.Add(mark);
                await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
                return Redirect("/AddMark");
        }
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
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

        public IActionResult Create()
        {

            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classroom, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassroomId,Email,Password,FirstName,LastName,Avartar,Phone,Address,BirthDay,ConfirmPassword,Role")] Account account)
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
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassroomId,Email,FirstName,LastName,Avartar,Phone,Address,BirthDay,Role")] Account account)
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
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (this.checkSession())
            {
                Response.StatusCode = 403;
                return Redirect("/Authentication/Login?Url=" + WebUtility.UrlEncode(Request.GetDisplayUrl()));
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
