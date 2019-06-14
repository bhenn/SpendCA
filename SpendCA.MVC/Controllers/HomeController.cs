using System;
using System.Diagnostics;
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
            //if (filter.MaxDate != DateTime.MinValue)
            //filter.MaxDate = filter.MaxDate.AddTicks(-1);

            //Whole month
            filter.MinDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 1);
            filter.MaxDate = filter.MinDate.AddMonths(1).AddTicks(-1);

            var spends = _spendService.GetAll(GetUserId(), filter);

            ViewBag.filter = filter;
            ViewBag.total = (double)spends.Sum(x => x.Value) / 100;

            ViewData["categoriesSummary"] = spends.GroupBy(x => x.CategoryId)
                                                .Select(c => new CategoryViewModel()
                                                {
                                                    Category = c.First().Category.Description,
                                                    Total = (double)c.Sum(s => s.Value) / 100
                                                }).ToList();

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
