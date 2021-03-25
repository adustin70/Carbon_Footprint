using CarbonProject.Data;
using CarbonProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarbonProject.Controllers
{
    [Authorize(Roles = "Environmentalist")]
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
        public IActionResult Create([Bind("Id, UserName, FirstName, LastName, State, IdentityUserId")]Environmentalist environmentalist)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                environmentalist.IdentityUserId = userId;
                _context.Add(environmentalist);
                _context.SaveChanges();
                return RedirectToAction("Survey");
            }
            return View(environmentalist);
        }

        public IActionResult Survey()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Survey([Bind("Id, MilesDriven, FuelType, PlasticBagsUsed, PlasticBottlesUsed, PowerUsed, EnvironmentalistId")]Survey survey)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();
                survey.EnvironmentalistId = user.Id;                
                _context.Add(survey);
                _context.SaveChanges();
                CarbonData(survey, survey.FuelType);
                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        public void CarbonData(Survey survey, string fuelType)
        {
            var footprint = _context.CarbonFootprints.Where(f => f.FuelTypeData == fuelType).FirstOrDefault();

            footprint.MilesDrivenData = survey.MilesDriven;
            footprint.PlasticBagsUsedData = survey.PlasticBagsUsed;
            footprint.PlasticBottlesUsedData = survey.PlasticBottlesUsed;
            footprint.PowerUsedData = survey.PowerUsed;

            _context.Update(footprint);
            _context.SaveChanges();
        }

        public IActionResult SerializeCarbonData(CarbonFootprint footprint)
        {
            string json = JsonConvert.SerializeObject(footprint);
            return Ok(json);
        }
    }
}
