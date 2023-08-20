using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.DTOs;

namespace UserManagement.Controllers
{
    public class UserGroupsController : Controller
    {
        private readonly UserManagementContext _context;

        public UserGroupsController(UserManagementContext context)
        {
            _context = context;
        }

        // GET: UserGroups
        public async Task<IActionResult> Index()
        {
            var userManagementContext = _context.UserGroups.Include(u => u.Groups).Include(u => u.Users);
            return View(await userManagementContext.ToListAsync());
        }

        // GET: UserGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserGroups == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroups
                .Include(u => u.Groups)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userGroup == null)
            {
                return NotFound();
            }

            return View(userGroup);
        }

        // GET: UserGroups/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserGroupId,UserId,GroupId")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGroup.UserId);
            return View(userGroup);
        }

        // GET: UserGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserGroups == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGroup.UserId);
            return View(userGroup);
        }

        // POST: UserGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserGroupId,UserId,GroupId")] UserGroup userGroup)
        {
            if (id != userGroup.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserGroupExists(userGroup.UserId))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGroup.UserId);
            return View(userGroup);
        }

        // GET: UserGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserGroups == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroups
                .Include(u => u.Groups)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userGroup == null)
            {
                return NotFound();
            }

            return View(userGroup);
        }

        // POST: UserGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserGroups == null)
            {
                return Problem("Entity set 'UserManagementContext.UserGroups'  is null.");
            }
            var userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup != null)
            {
                _context.UserGroups.Remove(userGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET:
        public async Task<IActionResult> UserCountPerGroup()
        {
            var groupUserCounts = await _context.Groups
                .Select(g => new GroupWithUserCount
                {
                    GroupName = g.GroupName,
                    UserCount = g.UserGroups.Count
                })
                .ToListAsync();

            return View(groupUserCounts);
        }


        public async Task<IActionResult> ListGroups()
        {
            var groups = await _context.Groups.ToListAsync();
            return View(groups);
        }

        //GET:
        public async Task<IActionResult> UsersByGroup(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            var usersInGroup = await _context.UserGroups
                .Where(ug => ug.GroupId == groupId)
                .Include(ug => ug.Users)
                .Select(ug => ug.Users)
                .ToListAsync();

            ViewData["GroupName"] = group.GroupName;
            return View(usersInGroup);
        }


        private bool UserGroupExists(int id)
        {
          return (_context.UserGroups?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
