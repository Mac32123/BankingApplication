
namespace BankApp.Server.Models
{
	public class TransactionRequestModel
	{
		public decimal TransactionAmmount { get; set; }
		public string TransactionSourceAccount { get; set; }
		public string TransactionDestinationAccount { get; set; }
		public TranType TransactionType { get; set; }
		public DateTime TransactionDate { get; set; }
	}
}
