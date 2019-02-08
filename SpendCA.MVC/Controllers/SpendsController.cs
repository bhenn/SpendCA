using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpendCA.MVC.Controllers
{
    [Authorize]
    public class SpendsController : Controller
    {
        private readonly ISpendRepository _spendsRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SpendsController(ISpendRepository spendRepository, ICategoryRepository categoryRepository)
        {
            _spendsRepository = spendRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {

            var spends = _spendsRepository.GetAll(GetUserId());

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
                _spendsRepository.Add(spend);

                return RedirectToAction("Index");
            }


            return View(spend);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            _spendsRepository.Delete((int)id, GetUserId());

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
