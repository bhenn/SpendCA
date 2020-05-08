using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.MVC.Models;

namespace SpendCA.MVC.Controllers
{
    [Authorize]
    public class SpendsController : Controller
    {
        private readonly ISpendService _spendService;
        private readonly ICategoryRepository _categoryRepository;

        public SpendsController(ISpendService spendService, ICategoryRepository categoryRepository)
        {
            _spendService = spendService;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(FilterModel filter)
        {
            if (filter.MinDate == DateTime.MinValue)
                filter.MinDate = DateTime.Now.AddDays(-30);

            if (filter.MaxDate == DateTime.MinValue)
                filter.MaxDate = DateTime.Now;


            var spends = _spendService.GetAll(GetUserId(), filter);

            ViewBag.filter = filter;
            ViewBag.total = (double)spends.Sum(x => x.Value) / 100;

            ViewData["categoriesSummary"] = spends.GroupBy(x => x.CategoryId)
                                                .Select(c => new CategoryViewModel()
                                                {
                                                    Category = c.First().Category.Description,
                                                    Total = (double)c.Sum(s => s.Value) / 100,
                                                    Id = c.First().Category.Id
                                                }).OrderBy(o => o.Category).ToList();

            return View(spends);
        }

        public IActionResult Create()
        {

            ViewBag.Categories = _categoryRepository.GetAll(GetUserId());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Spend spend)
        {
            if (ModelState.IsValid){

                spend.UserId = GetUserId();
                _spendService.Add(spend);

                return RedirectToAction("Index");
            }


            return View(spend);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            _spendService.Delete((int)id, GetUserId());

            return RedirectToAction("Index");
        }



        private int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(userId);
        }
    }
}
