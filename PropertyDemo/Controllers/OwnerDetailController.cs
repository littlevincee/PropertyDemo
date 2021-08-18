using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PropertyDemo.Data.Model;
using Microsoft.AspNetCore.Identity;
using PropertyDemo.Service.ViewModel;
using PropertyDemo.Data.DatabaseService.OwnerDetailService;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyDemo.Service.Enum;
using System;

namespace PropertyDemo.Controllers
{
    [Authorize]
    public class OwnerDetailController : Controller
    {
        private readonly IOwnerDetailService _ownerDetailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerDetailController(IOwnerDetailService ownerDetailService, UserManager<ApplicationUser> userManager)
            => (_ownerDetailService, _userManager) = (ownerDetailService, userManager);


        // GET: OwnerDetailController
        public IActionResult Index()
        {
            ViewBag.OwnerDetails = _ownerDetailService.GetAllOwnerDetails();

            return View();
        }

        // Get: OwnerDetailController/Create
        public IActionResult Create()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(TitleEnum)))
            {
                selectListItems.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewBag.SelectListItems = selectListItems;

            return View();
        }

        // POST: OwnerDetailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerDetailViewModel ownerDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                var saveResult = await _ownerDetailService.SaveChangesAsync(ownerDetailViewModel);

                if (saveResult > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(ownerDetailViewModel);
        }

        // GET: OwnerDetailController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id is not null)
            {
                var ownerDetailViewModel = _ownerDetailService.GetOwnerDetailById(id.Value);

                if (ownerDetailViewModel is not null)
                {
                    List<SelectListItem> selectListItems = new List<SelectListItem>();

                    foreach (var item in Enum.GetValues(typeof(TitleEnum)))
                    {
                        selectListItems.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                    }

                    ViewBag.SelectListItems = selectListItems;

                    return View(ownerDetailViewModel);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: OwnerDetailController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OwnerDetailViewModel ownerDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                await _ownerDetailService.UpdateAsync(ownerDetailViewModel);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
