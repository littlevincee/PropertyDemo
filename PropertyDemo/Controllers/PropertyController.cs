using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PropertyDemo.Data.Model;
using Microsoft.AspNetCore.Identity;
using PropertyDemo.Service.ViewModel;
using PropertyDemo.Data.DatabaseService.PropertyService;

namespace PropertyDemo.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IPropertyService propertyService, UserManager<ApplicationUser> userManager)
        {
            _propertyService = propertyService;
            _userManager = userManager;
        }

        // GET: PropertyController
        public async Task<IActionResult> Index()
        {
            var _user = await _userManager.GetUserAsync(User);

            var properties = _propertyService.GetAllNonAdminPropertiesByUserId(_user.Id);

            if (_user.IsAdministrator)
            {
                properties = _propertyService.GetAllPropertiesByCheckingAdminRight(_user.Id);
            }

            ViewBag.Properties = properties;
            return View();
        }

        // Get: PropertyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropertyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Property property)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.GetUserAsync(User);
                property.ApplicationUserId = _user.Id;
                var saveResult = await _propertyService.SaveChangesAsync(property);

                if (saveResult > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(property);
        }

        // GET: PropertyController/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
