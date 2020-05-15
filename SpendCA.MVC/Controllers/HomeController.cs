using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.MVC.Models;
using Newtonsoft.Json;

namespace SpendCA.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpendService _spendService;

        public HomeController(ISpendService spendService)
        {
            _spendService = spendService;
        }

        [Authorize]
        public IActionResult Index(FilterModel filter)
        {
            //Last 6 months
            filter.MinDate = new DateTime(DateTime.Now.AddMonths(-5).Year, DateTime.Now.AddMonths(-5).Month, 1);
            filter.MaxDate = DateTime.Now.AddDays(1);

            var spends = _spendService.GetAll(GetUserId(), filter);
            var months = spends.GroupBy(x => x.Date.Month).Select(m => new MonthSummaryViewModel
            {
                Year = m.First().Date.Year,
                Month = m.First().Date.Month,
                MonthDescription = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Key),
                MonthTotal = (double)m.Sum(s => s.Value) / 100,
                Categories = m.GroupBy(c => c.CategoryId).Select(c => new CategoryViewModel()
                {
                    Id = c.First().CategoryId,
                    Category = c.First().Category.Description,
                    Total = (double)c.Sum(s => s.Value) / 100
                }).OrderBy(o => o.Category).ToList()
            }).ToList();

            ViewData["monthsSummary"] = months;


            return View(spends);
        }

        [Authorize]
        public IActionResult SpendsByCategory(FilterModel filter){
            //Last 6 months
            filter.MinDate = new DateTime(DateTime.Now.AddMonths(-11).Year, DateTime.Now.AddMonths(-11).Month, 1);
            filter.MaxDate = DateTime.Now.AddDays(1);

            var spends = _spendService.GetAll(GetUserId(), filter);

            if (spends?.Count > 0){
                var categories = spends.GroupBy(x => x.Category.Description).Select(m => new SummaryByCategoryViewModel
                {
                    CategoryDescription = m.Key,
                    Months = m.GroupBy(c => c.Date.Month).Select(c => new MonthViewModel(){
                        Year = c.First().Date.Year,
                        MonthId = c.First().Date.Month,
                        Description = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.First().Date.Month),
                        Total = (double)c.Sum(s => s.Value) / 100
                    }).OrderBy(o => o.Year).ThenBy(o => o.MonthId).ToList()

                }).ToList();

                var months = categories.FirstOrDefault().Months.Select(x => x.Description).ToList();
                var values = categories.FirstOrDefault().Months.Select(x => x.Total).ToList();
                ViewData["monthsName"] = JsonConvert.SerializeObject(months);
                ViewData["monthsValues"] = JsonConvert.SerializeObject(values);
                ViewData["categoryDescription"] = JsonConvert.SerializeObject(categories.First().CategoryDescription);
                ViewData["total"] = Math.Round(categories.First().Months.Sum(s => s.Total), 2);
                ViewData["average"] = Math.Round(categories.First().Months.Average(s => s.Total), 2);

                return View();
            }
            

            return NotFound();
            
            
        }

        public IActionResult Login()
        {
            return Login(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(userId);
        }
    }
}
