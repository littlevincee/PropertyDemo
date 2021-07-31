using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace PropertyDemo.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdministrator { get; set; }

        // Relationships property
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
