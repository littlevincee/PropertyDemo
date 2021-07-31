using System.Collections.Generic;

namespace PropertyDemo.Service.ViewModel
{
  public class PropertyTransactionViewModel
  {
    public int PropertyId { get; set; }

    public string PropertyName { get; set; }

    public bool IsOwnProperty { get; set; }

    public string UserId { get; set; }

    public List<TransactionViewModel> TransactionViewModels { get; set; } = new List<TransactionViewModel>();
  }
}
