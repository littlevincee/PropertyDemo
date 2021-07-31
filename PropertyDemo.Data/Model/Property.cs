using System;
using System.Collections.Generic;

namespace PropertyDemo.Data.Model
{
    public class Property
    {
        public int PropertyId { get; set; }

        public string PropertyName { get; set; }

        public int Bedroom { get; set; }

        public bool IsAvaliable { get; set; }

        public decimal SalePrice { get; set; }

        public decimal LeasePrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Relationships
        public virtual ICollection<Transaction> Transactions { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
