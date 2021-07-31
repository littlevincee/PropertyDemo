using System;

namespace PropertyDemo.Service.ViewModel
{
  public class TransactionViewModel
  {
    public int TransactionId { get; set; }

    public int PropertyId { get; set; }

    public decimal TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string PaymentMethod { get; set; }

    public string BankName { get; set; }

    public bool IsDeposit { get; set; }

    public string UserId { get; set; }
  }
}
