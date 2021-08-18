using System;
using System.Collections.Generic;

namespace PropertyDemo.Data.Model
{
    public class OwnerDetail
    {
        public int OwnerDetailId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public int ContactNumber { get; set; }

        public string HongKongId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Relationships
        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
