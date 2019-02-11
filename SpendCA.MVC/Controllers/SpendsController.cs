using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;

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

        public IActionResult Index()
        {

            var spends = _spendService.GetAll(GetUserId());

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
