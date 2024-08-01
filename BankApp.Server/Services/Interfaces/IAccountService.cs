using BankApp.Server.Models;

namespace BankApp.Server.Services.Interfaces
{
	public interface IAccountService
	{
		Account? Authenticate(string AccountNumber, string Pin);
		IEnumerable<Account> GetAccounts();
		Account Create(Account account, string Pin, string ConfirmPin);
		void Update(Account account, string? Pin = null);
		void Delete(int id);
		Account? GetById(int id);
		Account? GetByAccountNumber(string AccountNumber);
	}
}
