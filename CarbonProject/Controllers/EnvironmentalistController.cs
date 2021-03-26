using CarbonProject.Data;
using CarbonProject.Models;
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
                return View();
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
                CarbonData(survey, survey.FuelType);
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
        public async Task<IActionResult> EditSurvey(Survey survey)
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

        public async Task<IActionResult> CarbonData(Survey survey, string fuelType)
        {
            var footprint = _context.CarbonFootprints.Where(f => f.FuelTypeData == fuelType).FirstOrDefault();

            footprint.MilesDrivenData = survey.MilesDriven;
            footprint.PlasticBagsUsedData = survey.PlasticBagsUsed;
            footprint.PlasticBottlesUsedData = survey.PlasticBottlesUsed;
            footprint.PowerUsedData = survey.PowerUsed;

            _context.Update(footprint);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult SerializeCarbonData(CarbonFootprint footprint)
        {
            string json = JsonConvert.SerializeObject(footprint);
            return Ok(json);
        }
    }
}
