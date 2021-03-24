using CarbonProject.Data;
using CarbonProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarbonProject.Controllers
{
    public class EnvironmentalistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnvironmentalistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Environmentalist environmentalist)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            environmentalist.IdentityUserId = userId;
            _context.Add(environmentalist);
            _context.SaveChanges();
            return RedirectToAction("Survey");
        }

        public IActionResult Survey()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Survey(int? id)
        {
            var user = _context.Environmentalists.Where(u => u.Id == id).FirstOrDefault();
            _context.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
