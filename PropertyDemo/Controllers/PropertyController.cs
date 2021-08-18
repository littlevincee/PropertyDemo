using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PropertyDemo.Data.Model;
using Microsoft.AspNetCore.Identity;
using PropertyDemo.Service.ViewModel;
using PropertyDemo.Data.DatabaseService.PropertyService;
using PropertyDemo.Data.DatabaseService.OwnerDetailService;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PropertyDemo.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        
        private readonly IOwnerDetailService _ownerDetailService;
        
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IPropertyService propertyService, IOwnerDetailService ownerDetailService, UserManager<ApplicationUser> userManager)
            => (_propertyService, _ownerDetailService, _userManager) = (propertyService, ownerDetailService, userManager);
       

        // GET: PropertyController
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.IsAdministrator)
            {
                ViewBag.Properties = _propertyService.GetAllPropertiesByCheckingAdminRight(user.Id);
            }
            else
            {
                ViewBag.Properties = _propertyService.GetAllNonAdminPropertiesByUserId(user.Id);
            };

            return View();
        }

        // Get: PropertyController/Create
        public IActionResult Create()
        {            
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var item in _ownerDetailService.GetAllOwnerDetails())
            {
                selectListItems.Add(new SelectListItem { Text = item.Title + " " + item.FirstName + " " + item.Surname, Value = item.OwnerDetailId.ToString() });
            }

            ViewBag.SelectListItems = selectListItems;

            return View();
        }

        // POST: PropertyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel propertyViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                var saveResult = await _propertyService.SaveChangesAsync(propertyViewModel, user);

                if (saveResult > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(propertyViewModel);
        }

        // GET: PropertyController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is not null)
            {
                var property = await _propertyService.GetPropertyByPropertyIdAsync(id.Value);

                if (property is not null)
                {
                    List<SelectListItem> selectListItems = new List<SelectListItem>();
                    foreach (var item in _ownerDetailService.GetAllOwnerDetails())
                    {
                        selectListItems.Add(new SelectListItem { Text = item.FirstName + " " + item.Surname, Value = item.OwnerDetailId.ToString() });
                    }

                    ViewBag.SelectListItems = selectListItems;

                    return View(property);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: PropertyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel propertyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _propertyService.UpdateAsync(propertyViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PropertyController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is not null)
            {
                var property = await _propertyService.GetPropertyByPropertyIdAsync(id.Value);

                if (property is not null)
                {
                    return View(property);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: PropertyController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _propertyService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
