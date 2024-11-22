using Blog.Entities.Models;
using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _context;

        public UsersController(IUnitOfWork context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;

            return View(_context.ApplicationUser.GetAll().Where(x => x.Id != userid).ToList());
        }

        public IActionResult LockUnlock(string? id)
        {
            var user = _context.ApplicationUser.GetFirstorDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }

            _context.Complete();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }
    }
}
