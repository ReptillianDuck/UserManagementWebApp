using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManagementContext _context;

        public UsersController(UserManagementContext context)
        {
            ViewBag.NoGroups = true;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'UserManagementContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (!_context.Groups.Any())
            {
                ViewBag.NoGroups = true;
                // No groups available, redirect to Groups controller to add a group
                return View();
            }

            ViewBag.NoGroups = false;

            int userCount = _context.Users.Count(); 
            ViewBag.UserCount = userCount;

            ViewBag.GroupList = new SelectList(_context.Groups, "GroupId", "GroupName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Users users, int[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();

                foreach (int groupId in selectedGroups)
                {
                    var userGroup = new UserGroup
                    {
                        UserId = users.Id,
                        GroupId = groupId
                    };
                    _context.UserGroups.Add(userGroup);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

                // Return the view with validation errors
                return View(users);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserGroups) 
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            if (user.UserGroups == null)
            {
                user.UserGroups = new List<UserGroup>(); 
            }

            var selectedGroupIds = user.UserGroups.Select(ug => ug.GroupId).ToList();

            ViewBag.AllGroups = await _context.Groups.ToListAsync();
            ViewBag.SelectedGroupIds = selectedGroupIds;

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Users updatedUser, int[] selectedGroups)
        {
            if (id != updatedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users
                        .Include(u => u.UserGroups)
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.Name = updatedUser.Name;

                    user.UserGroups.Clear(); // Remove existing associations
                    foreach (var groupId in selectedGroups)
                    {
                        user.UserGroups.Add(new UserGroup
                        {
                            UserId = user.Id,
                            GroupId = groupId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(updatedUser.Id))
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
            return View(updatedUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UserManagementContext.Users'  is null.");
            }
            var users = await _context.Users
                .Include(u => u.UserGroups) 
                .FirstOrDefaultAsync(u => u.Id == id);

            if (users != null)
            {
                foreach (var userGroup in users.UserGroups.ToList())
                {
                    _context.UserGroups.Remove(userGroup);
                }
                _context.Users.Remove(users);
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
