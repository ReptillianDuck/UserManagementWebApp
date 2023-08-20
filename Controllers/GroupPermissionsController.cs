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
    public class GroupPermissionsController : Controller
    {
        private readonly UserManagementContext _context;

        public GroupPermissionsController(UserManagementContext context)
        {
            _context = context;
        }

        // GET: GroupPermissions
        public async Task<IActionResult> Index()
        {
            var userManagementContext = _context.GroupPermissions.Include(g => g.Groups).Include(g => g.Permission);
            return View(await userManagementContext.ToListAsync());
        }

        // GET: GroupPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GroupPermissions == null)
            {
                return NotFound();
            }

            var groupPermission = await _context.GroupPermissions
                .Include(g => g.Groups)
                .Include(g => g.Permission)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groupPermission == null)
            {
                return NotFound();
            }

            return View(groupPermission);
        }

        // GET: GroupPermissions/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupName");
            ViewData["Permissions"] = new MultiSelectList(_context.Permissions, "PermissionId", "PermissionName");
            return View();
        }

        // POST: GroupPermissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("GroupId,PermissionId")] GroupPermission groupPermission)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var permissionId in groupPermission.PermissionId)
        //        {
        //            var newGroupPermission = new GroupPermission
        //            {
        //                GroupId = groupPermission.GroupId,
        //                PermissionId = permissionId
        //            };
        //            _context.Add(newGroupPermission);
        //        }
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupName", groupPermission.GroupId);
        //    ViewData["Permissions"] = new MultiSelectList(_context.Permissions, "PermissionId", "PermissionName", groupPermission.PermissionIds);
        //    return View(groupPermission);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("GroupPermissionId,GroupId,PermissionId")] GroupPermission groupPermission)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(groupPermission);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", groupPermission.GroupId);
        //    ViewData["PermissionId"] = new SelectList(_context.Permissions, "PermissionId", "PermissionId", groupPermission.PermissionId);
        //    return View(groupPermission);
        //}

        // GET: GroupPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GroupPermissions == null)
            {
                return NotFound();
            }

            var groupPermission = await _context.GroupPermissions.FindAsync(id);
            if (groupPermission == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", groupPermission.GroupId);
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "PermissionId", "PermissionId", groupPermission.PermissionId);
            return View(groupPermission);
        }

        // POST: GroupPermissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupPermissionId,GroupId,PermissionId")] GroupPermission groupPermission)
        {
            if (id != groupPermission.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupPermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupPermissionExists(groupPermission.GroupId))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", groupPermission.GroupId);
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "PermissionId", "PermissionId", groupPermission.PermissionId);
            return View(groupPermission);
        }

        // GET: GroupPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GroupPermissions == null)
            {
                return NotFound();
            }

            var groupPermission = await _context.GroupPermissions
                .Include(g => g.Groups)
                .Include(g => g.Permission)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groupPermission == null)
            {
                return NotFound();
            }

            return View(groupPermission);
        }

        // POST: GroupPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GroupPermissions == null)
            {
                return Problem("Entity set 'UserManagementContext.GroupPermissions'  is null.");
            }
            var groupPermission = await _context.GroupPermissions.FindAsync(id);
            if (groupPermission != null)
            {
                _context.GroupPermissions.Remove(groupPermission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupPermissionExists(int id)
        {
          return (_context.GroupPermissions?.Any(e => e.GroupId == id)).GetValueOrDefault();
        }
    }
}
