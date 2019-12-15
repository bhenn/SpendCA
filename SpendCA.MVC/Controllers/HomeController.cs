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
                MonthDescription = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Key),
                MonthTotal = (double)m.Sum(s => s.Value) / 100,
                Categories = m.GroupBy(c => c.CategoryId).Select(c => new CategoryViewModel()
                {
                    Category = c.First().Category.Description,
                    Total = (double)c.Sum(s => s.Value) / 100
                }).OrderBy(o => o.Category).ToList()
            }).ToList();

            ViewData["monthsSummary"] = months;


            return View(spends);
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
