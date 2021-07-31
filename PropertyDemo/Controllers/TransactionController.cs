using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PropertyDemo.Data.Model;
using Microsoft.AspNetCore.Identity;
using PropertyDemo.Service.ViewModel;
using PropertyDemo.Data.DatabaseService.TransactionService;

namespace PropertyDemo.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ITransactionService transactionService, UserManager<ApplicationUser> userManager)
        {
            _transactionService = transactionService;
            _userManager = userManager;
        }

        // GET: TransactionController
        public async Task<IActionResult> Index()
        {
           var user = await _userManager.GetUserAsync(User);

            var transactions = _transactionService.GetAllPropertyTransactionsByCheckingAdminRight(user.Id);

            ViewBag.Transactions = transactions;

            return View();
        }

        // GET: TransactionController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction, int id)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.GetUserAsync(User);

                transaction.ApplicationUserId = _user.Id;
                transaction.PropertyId = id;

                var saveResult = await _transactionService.SaveChangesAsync(transaction);

                if (saveResult > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(transaction);
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
