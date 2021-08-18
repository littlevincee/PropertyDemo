using System;
using System.Collections.Generic;

namespace PropertyDemo.Data.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public decimal TransactionAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string BankName { get; set; }

        public bool IsDeposit { get; set; }

        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Relationships
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int OwnerDetailId { get; set; }
        public virtual OwnerDetail OwnerDetail { get; set; }
    }
}
