using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyDemo.Service.ViewModel
{
    public class OwnerDetailViewModel
    {
        public int OwnerDetailId { get; set; }

        [Required(ErrorMessage = "Title cannot be empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "FirstName cannot be empty")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname cannot be empty")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Hong Kong Id cannot be empty")]
        [DisplayName("HK Id Number")]
        public string HongKongId { get; set; }

        [Required(ErrorMessage = "Contact number cannot be empty")]
        [DisplayName("Contact Number")]
        public int ContactNumber { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
