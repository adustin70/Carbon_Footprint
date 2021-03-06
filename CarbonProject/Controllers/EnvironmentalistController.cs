using CarbonProject.Data;
using CarbonProject.Models;
using CarbonProject.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                return RedirectToAction(nameof(Create));
            }
            else
            {
                var carbonData = _context.CarbonFootprints.Where(c => c.EnvironmentalistId == user.Id).FirstOrDefault();
                

                return View(carbonData);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, FirstName, LastName, State, IdentityUserId")]Environmentalist environmentalist)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                environmentalist.IdentityUserId = userId;

                _context.Add(environmentalist);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Survey));
            }
            return View(environmentalist);
        }

        public IActionResult Edit()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id, UserName, FirstName, LastName, State, IdentityUserId")] Environmentalist environmentalist)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();

                user.UserName = environmentalist.UserName;
                user.FirstName = environmentalist.FirstName;
                user.LastName = environmentalist.LastName;
                user.State = environmentalist.State;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(environmentalist);
        }

        public IActionResult Survey()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Survey([Bind("Id, MilesDriven, FuelType, PlasticBagsUsed, PlasticBottlesUsed, PowerUsed, EnvironmentalistId")]Survey survey)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();
                survey.EnvironmentalistId = user.Id;

                _context.Add(survey);
                await _context.SaveChangesAsync();
                await CarbonData(survey);

                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        public IActionResult EditSurvey()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();
            var survey = _context.Surveys.Where(s => s.EnvironmentalistId == user.Id).FirstOrDefault();

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSurvey([Bind("Id, MilesDriven, FuelType, PlasticBagsUsed, PlasticBottlesUsed, PowerUsed, EnvironmentalistId")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();
                var chosenSurvey = _context.Surveys.Where(s => s.EnvironmentalistId == user.Id).FirstOrDefault();

                chosenSurvey.MilesDriven = survey.MilesDriven;
                chosenSurvey.FuelType = survey.FuelType;
                chosenSurvey.PlasticBagsUsed = survey.PlasticBagsUsed;
                chosenSurvey.PlasticBottlesUsed = survey.PlasticBottlesUsed;
                chosenSurvey.PowerUsed = survey.PowerUsed;

                _context.Entry(chosenSurvey).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        public async Task<IActionResult> CarbonData(Survey survey)
        {
            var data = new CarbonFootprint();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Environmentalists.Where(e => e.IdentityUserId == userId).FirstOrDefault();

            data.EnvironmentalistId = user.Id;

            if (survey.FuelType == "Gasoline")
            {
                data.FuelType = survey.FuelType;
                data.FuelEmissions = Math.Round(survey.MilesDriven * 8.89);
            }
            else if (survey.FuelType == "Diesel")
            {
                data.FuelType = survey.FuelType;
                data.FuelEmissions = Math.Round(survey.MilesDriven * 10.16);
            }

            data.PlasticBagsEmissions = Math.Round(survey.PlasticBagsUsed * 33);
            data.PlasticBottlesEmissions = Math.Round(survey.PlasticBottlesUsed * 3);
            data.PowerUsedEmissions = Math.Round(survey.PowerUsed * 1.85);

            _context.CarbonFootprints.Add(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public IActionResult UsersChart(int? id)
        {
            var user = _context.CarbonFootprints.Where(c => c.EnvironmentalistId == id).FirstOrDefault();
            var data = _context.CarbonFootprints.FromSqlRaw("SELECT * FROM dbo.CarbonFootprints").ToList();


            var fuel = 0.0;
            var bags = 0.0;
            var bottles = 0.0;
            var power = 0.0;

            foreach (var item in data)
            {
                fuel += item.FuelEmissions;
                bags += item.PlasticBagsEmissions;
                bottles += item.PlasticBottlesEmissions;
                power += item.PowerUsedEmissions;
            }


            var CarbonViewModel = new CarbonFootprintViewModel
            {
                FuelEmissions = fuel,
                PlasticBagsEmissions = bags,
                PlasticBottlesEmissions = bottles,
                PowerUsedEmissions = power,
                SignedInFuelEmissions = user.FuelEmissions,
                SignedInPlasticBagsEmissions = user.PlasticBagsEmissions,
                SignedInPlasticBottlesEmissions = user.PlasticBottlesEmissions,
                SignedInPowerUsedEmissions = user.PowerUsedEmissions
            };
            return View(CarbonViewModel);
        }
    }
}
