using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LecturesApp.Data;
using LecturesApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LecturesApp.Controllers
{
    public class LecturesController : Controller
    {
        private readonly AppDbContext _context;

        public LecturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lectures
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Lectures.Include(l => l.HostUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Lectures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.HostUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: Lectures/Create
        [Authorize]
        public IActionResult Create()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //ViewData["HostUserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.HostUserID = claim.Value;
            return View();
        }

        // POST: Lectures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,AgeLimit,UsersNumberLimit,StartDate,EndDate,State,HostUserID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.HostUserID = claim.Value;
            return View(lecture);
        }

        // GET: Lectures/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.HostUserID = claim.Value;

            return View(lecture);
        }

        // POST: Lectures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,AgeLimit,UsersNumberLimit,StartDate,EndDate,State,HostUserID")] Lecture lecture)
        {
            if (id != lecture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.ID))
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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.HostUserID = claim.Value;
            return View(lecture);
        }

        // GET: Lectures/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.HostUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.ID == id);
        }




        [Authorize]
        public async Task<IActionResult> MyHostedLectures()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var appDbContext = _context.Lectures
                .Where(l => l.HostUserID == claim.Value)
                .Include(l => l.HostUser);
            return View(await appDbContext.ToListAsync());
        }

        [Authorize]
        public ActionResult MyRegisteredLectures()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = _context.Users
                .Include(u => u.RegisteredOnLecturesLink)
                .ThenInclude(r => r.Lecture)
                .ThenInclude(l => l.HostUser)
                .Where(u => u.Id == claim.Value)
                .Single();

            var lectures = user.RegisteredOnLecturesLink.Select(r => r.Lecture).ToList();

            return View(lectures);
        }

        [Authorize]
        public async Task<IActionResult> RegisterForLecture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.RegisteredMembersLink)
                .FirstOrDefaultAsync(l => l.ID == id);

            if (lecture == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == claim.Value);

            if (user == null)
            {
                return NotFound();
            }

            UserLecture userLecture = new UserLecture { Lecture = lecture, LectureID = lecture.ID, User = user, UserID = user.Id };

            lecture.RegisteredMembersLink.Add(userLecture);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> UnRegisterForLecture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.RegisteredMembersLink)
                .FirstOrDefaultAsync(l => l.ID == id);

            if (lecture == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.Users
                .Include(u => u.RegisteredOnLecturesLink)
                .FirstOrDefaultAsync(u => u.Id == claim.Value);

            if (user == null)
            {
                return NotFound();
            }

            user.RegisteredOnLecturesLink.Remove(lecture.RegisteredMembersLink.FirstOrDefault(r => r.Lecture.ID == lecture.ID));

            //lecture.RegisteredMembersLink.Remove();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyRegisteredLectures));
        }
    }
}
