using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyDemo.Data.DatabaseService.PropertyService;
using PropertyDemo.Service.Exceptions;
using PropertyDemo.Service.ViewModel;

namespace PropertyDemo.Data.DatabaseService.TransactionService
{
    public interface ITransactionService
    {
        List<PropertyTransactionViewModel> GetAllNonAdminPropertyTransactionsByUserId(string userId);

        List<PropertyTransactionViewModel> GetAllPropertyTransactionsByCheckingAdminRight(string userId);

        Task<TransactionViewModel> GetTransactionByTransactionIdAsync(int TransactionId);

        Task<int> SaveChangesAsync(Model.Transaction Transaction);

        Task<int> UpdateAsync(TransactionViewModel TransactionVM);

        Task<int> DeleteAsync(int TransactionId);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IDataContext _dataContext;
        private IPropertyService _propertyService;

        public TransactionService(IDataContext dataContext, IPropertyService propertyService)
        {
            _dataContext = dataContext;
            _propertyService = propertyService;
        }

        #region Save, Update, Delete

        public async Task<int> SaveChangesAsync(Model.Transaction transaction)
        {
            transaction.CreatedOn = DateTime.UtcNow;

            _dataContext.Transactions.Add(transaction);

            var saveResult = await _dataContext.SaveChangesAsync(default);

            return saveResult;
        }

        public async Task<int> UpdateAsync(TransactionViewModel transactionVM)
        {
            var transaction = TransactionModelMapper(transactionVM);

            var saveResult = 0;

            try
            {
                transaction.UpdatedOn = DateTime.UtcNow;

                _dataContext.Transactions.Update(transaction);

                saveResult = await _dataContext.SaveChangesAsync(default);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_dataContext.Transactions.Any(x => x.TransactionId == transaction.TransactionId))
                {
                    throw new CustomException($"Transaction not found: {ex.Message}");
                }
            }

            return saveResult;
        }

        /// <summary>
        /// Hard delete a transaction record
        /// </summary>
        /// <param name="TransactionId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int TransactionId)
        {
            var Transaction = await _dataContext.Transactions.FindAsync(TransactionId);

            _dataContext.Transactions.Remove(Transaction);

            return await _dataContext.SaveChangesAsync(default);
        }
        #endregion

        #region Get

        /// <summary>
        /// Get all Transactions with its property information of an non-admin user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of Property Transaction view model</returns>
        public List<PropertyTransactionViewModel> GetAllNonAdminPropertyTransactionsByUserId(string userId)
        {
            var transactions = _dataContext.Properties
                                .Include(x => x.Transactions)
                                .Include(x => x.ApplicationUser)
                                .Where(w => w.ApplicationUserId == userId && w.ApplicationUser.IsAdministrator == false);

            return PropertyTransactionMapper(transactions);
        }

        /// <summary>
        /// Get all Transactions with its property information of an admin user plus all non-admin's ones
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>A list of Property Transaction view model</returns>
        public List<PropertyTransactionViewModel> GetAllPropertyTransactionsByCheckingAdminRight(string userId)
        {
            var adminProperties = _dataContext.Properties
                                    .Include(x => x.Transactions)
                                    .Include(x => x.ApplicationUser)
                                    .Where(w => w.ApplicationUserId == userId && w.ApplicationUser.IsAdministrator == true);

            var nonAdminProperties = _dataContext.Properties
                                    .Include(x => x.Transactions)
                                    .Include(x => x.ApplicationUser)
                                    .Where(w => w.ApplicationUserId != userId && w.ApplicationUser.IsAdministrator == false);

            var propertyTransactionVM = new List<PropertyTransactionViewModel>();

            var adminPropertyTransactionVM = PropertyTransactionMapper(adminProperties);
            var nonAdminPropertyTransactionVM = PropertyTransactionMapper(nonAdminProperties);

            return adminPropertyTransactionVM.Concat(nonAdminPropertyTransactionVM).ToList();
        }

        /// <summary>
        /// Get a single transaction record by its Id
        /// </summary>
        /// <param name="TransactionId">Transaction Id</param>
        /// <returns>A Transaction View Model</returns>
        public async Task<TransactionViewModel> GetTransactionByTransactionIdAsync(int TransactionId)
        {
            var transaction = await _dataContext.Transactions.FindAsync(TransactionId);

            _ = transaction ?? throw new CustomException("Transaction not found");

            return TransactionViewModelMapper(transaction);
        }

        #endregion

        #region private functions

        /// <summary>
        /// A function to map property with its transaction into Property Transaction View Model
        /// </summary>
        /// <param name="properties">IQueryabelk of Model.Properties</param>
        /// <returns>A list of Property Transaction View Model</returns>
        private List<PropertyTransactionViewModel> PropertyTransactionMapper(IQueryable<Model.Property> properties)
        {
            var propertyTransactionVM = new List<PropertyTransactionViewModel>();

            foreach (var prop in properties)
            {
                var property = new PropertyTransactionViewModel()
                {
                    IsOwnProperty = prop.ApplicationUser.IsAdministrator,
                    PropertyName = prop.PropertyName,
                    PropertyId = prop.PropertyId,
                };

                if (prop.Transactions.Count > 0)
                {
                    foreach (var tran in prop.Transactions)
                    {
                        var transactionVM = TransactionViewModelMapper(tran);

                        property.TransactionViewModels.Add(transactionVM);
                    }
                }
                else
                {
                    property.TransactionViewModels = new List<TransactionViewModel>();
                }
                propertyTransactionVM.Add(property);
            }

            return propertyTransactionVM;
        }


        /// <summary>
        /// A function to map a single PropertDemo.Data.Model.Transaction to aPropertyDemo.Service.ViewModel.TransactionViewModel
        /// </summary>
        /// <param name="transaction">A Transaction model</param>
        /// <returns>A Transaction View Model</returns>
        private TransactionViewModel TransactionViewModelMapper(Model.Transaction transaction)
            => new TransactionViewModel()
            {
                TransactionId = transaction.TransactionId,
                PropertyId = transaction.PropertyId,
                TransactionDate = transaction.TransactionDate,
                BankName = transaction.BankName,
                IsDeposit = transaction.IsDeposit,
                PaymentMethod = transaction.PaymentMethod,
                TransactionAmount = transaction.TransactionAmount,
                UserId  = transaction.ApplicationUserId
            };

        /// <summary>
        /// A function to map a single PropertyDemo.Service.ViewModel.TransactionViewModel to PropertDemo.Data.Model.Transaction
        /// </summary>
        /// <param name="transactionViewModel">A Transaction View Model</param>
        /// <returns>A Transaction Mdoel</returns>
        private Model.Transaction TransactionModelMapper(TransactionViewModel transactionViewModel)
            => new Model.Transaction()
            {
                PropertyId = transactionViewModel.PropertyId,
                TransactionId = transactionViewModel.TransactionId,
                TransactionAmount = transactionViewModel.TransactionAmount,
                TransactionDate = transactionViewModel.TransactionDate,
                BankName = transactionViewModel.BankName,
                IsDeposit = transactionViewModel.IsDeposit,
                PaymentMethod = transactionViewModel.PaymentMethod,
                ApplicationUserId = transactionViewModel.UserId
            };

        #endregion

    }
}
