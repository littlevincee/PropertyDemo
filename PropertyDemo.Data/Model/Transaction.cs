using System;

namespace PropertyDemo.Data.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        
        public int PropertyId { get; set; }

        public string ApplicationUserId { get; set; }

        public decimal TransactionAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string BankName { get; set; }

        public bool IsDeposit { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Relationships
        public virtual Property Property { get; set; }
                
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
