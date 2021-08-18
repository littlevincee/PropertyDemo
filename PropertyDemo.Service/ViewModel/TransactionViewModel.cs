using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyDemo.Service.ViewModel
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }

        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Transaction Amount cannot be empty")]
        [DisplayName("Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "You must select a transaction type")]
        [DisplayName("Transaction Type")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "You must select a transaction date")]
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Payment method cannot be empty")]
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }

        public string BankName { get; set; }

        public bool IsDeposit { get; set; }

        public string UserId { get; set; }

        public int OwnerDetailId { get; set; }
    }
}
