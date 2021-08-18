using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyDemo.Service.ViewModel
{
    public class PropertyTransactionViewModel
    {
        public int PropertyId { get; set; }

        [DisplayName("Property Name")]
        public string PropertyName { get; set; }

        public bool IsOwnProperty { get; set; }

        public string UserId { get; set; }

        public int OwnerDetailId { get; set; }

        public List<TransactionViewModel> TransactionViewModels { get; set; } = new List<TransactionViewModel>();
    }
}
