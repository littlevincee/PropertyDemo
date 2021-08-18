using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyDemo.Data.DatabaseService.OwnerDetailService;
using PropertyDemo.Data.DatabaseService.TransactionService;
using PropertyDemo.Data.Model;
using PropertyDemo.Service.Enum;
using PropertyDemo.Service.ViewModel;

namespace PropertyDemo.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        private readonly IOwnerDetailService _ownerDetailService;

        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ITransactionService transactionService, IOwnerDetailService ownerDetailService, UserManager<ApplicationUser> userManager)
            => (_transactionService, _ownerDetailService, _userManager) = (transactionService, ownerDetailService, userManager);


        // GET: TransactionController
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.IsAdministrator)
            {
                ViewBag.Transactions = _transactionService.GetAllPropertyTransactionsByCheckingAdminRight(user.Id);
            }
            else
            {
                ViewBag.Transactions = _transactionService.GetAllNonAdminPropertyTransactionsByUserId(user.Id);
            }

            return View();
        }

        // GET: TransactionController/Create
        public IActionResult Create(int ownerId, string propertyName)
        {
            ViewBag.PropertyName = propertyName;
            var ownerDetail = _ownerDetailService.GetOwnerDetailById(ownerId);
            ViewBag.OwnedBy = ownerDetail.Title + " " + ownerDetail.FirstName + " " + ownerDetail.Surname;

            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(TransactionTypeEunm)))
            {
                selectListItems.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewBag.SelectListItems = selectListItems;

            return View();
        }

        // POST: TransactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel transactionViewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var saveResult = await _transactionService.SaveChangesAsync(transactionViewModel, id, user);

                if (saveResult > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(transactionViewModel);
        }

        // GET: TransactionController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is not null)
            {
                var transaction = await _transactionService.GetTransactionByTransactionIdAsync(id.Value);

                if (transaction is not null)
                {
                    return View(transaction);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: TransactionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionViewModel transactionViewModel)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.UpdateAsync(transactionViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TransactionController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is not null)
            {
                var property = await _transactionService.GetTransactionByTransactionIdAsync(id.Value);

                if (property is not null)
                {
                    return View(property);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: TransactionController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
