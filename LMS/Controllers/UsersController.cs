﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data.Data;

namespace LMS.Web.Controllers
{
    public class UserssController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserssController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Userss
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Users.Include(u => u.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Userss/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // GET: Userss/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description");
            return View();
        }

        // POST: Userss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,CourseId,Id,UsersName,NormalizedUsersName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User Users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description", Users.CourseId);
            return View(Users);
        }

        // GET: Userss/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users.FindAsync(id);
            if (Users == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description", Users.CourseId);
            return View(Users);
        }

        // POST: Userss/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,CourseId,Id,UsersName,NormalizedUsersName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User Users)
        {
            if (id != Users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(Users.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description", Users.CourseId);
            return View(Users);
        }

        // GET: Userss/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // POST: Userss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var Users = await _context.Users.FindAsync(id);
            if (Users != null)
            {
                _context.Users.Remove(Users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
