using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyDemo.Service.ViewModel
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Property name cannot be empty")]
        [DisplayName("Property Name")]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "The number of Bedroom cannot be empty")]
        [DisplayName("Number of Bedroom")]
        public int Bedroom { get; set; }

        public bool IsAvaliable { get; set; }

        [Required(ErrorMessage = "Sales Price cannot be empty")]
        [DisplayName("Sale Price")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "Lease Price cannot be empty")]
        [DisplayName("Lease Price")]
        public decimal LeasePrice { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "You must select a Property Owner")]
        public int OwnerDetailId { get; set; }

        public string OwnerName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
