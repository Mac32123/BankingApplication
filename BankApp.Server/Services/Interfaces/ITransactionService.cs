using BankApp.Server.Models;

namespace BankApp.Server.Services.Interfaces
{
	public interface ITransactionService
	{
		Response CreateNewTransaction(Transaction transaction);
		Response FindTransactionByDate(DateTime date);
		Response MakeDeposit(string AccountNumber, decimal Amount);
		Response MakeWithdrawal(string AccountNumber, decimal Amount);
		Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string title);
		List<Response> GetAllTransactions(string AccountNumber, int toFrom, DateTime? startingDate, DateTime? endingDate);
	}
}
