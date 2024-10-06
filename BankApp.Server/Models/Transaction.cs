using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace BankApp.Server.Models
{
	[Table("Transactions")]
	public class Transaction
	{
		[Key]
		public int Id { get; set; }
		public string TransactionUniqueReference { get; set; }
		public string TransactionTitle {  get; set; }
		public string TransactionAccount {  get; set; }
		public decimal AccountBalance { get; set; }
		public decimal TransactionAmmount { get; set; }
		public TranStatus TransactionStatus { get; set; }
		public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
		public string TransactionSourceAccount { get; set; }
		public string TransactionDestinationAccount { get; set; }
		public string TransactionParticulars { get; set; }
		public TranType TransactionType { get; set; }
		public DateTime TransactionDate { get; set; }

        public Transaction()
        {
			TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        }
		public Transaction(Transaction transaction)
		{
			TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";

			TransactionTitle = transaction.TransactionTitle;
			TransactionAccount = transaction.TransactionAccount;
			AccountBalance = transaction.AccountBalance;
			TransactionAmmount = transaction.TransactionAmmount;
			TransactionStatus = transaction.TransactionStatus;
			TransactionSourceAccount = transaction.TransactionSourceAccount;
			TransactionDestinationAccount = transaction.TransactionDestinationAccount;
			TransactionParticulars = transaction.TransactionParticulars;
			TransactionType = transaction.TransactionType;
			TransactionDate = transaction.TransactionDate;
	}

    }
	public enum TranStatus
	{
		Success,
		Failed,
		Error
	}

	public enum TranType
	{
		Deposit,
		Withdrawal,
		Transfer
	}

}
