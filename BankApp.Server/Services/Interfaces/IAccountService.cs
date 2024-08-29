using BankApp.Server.Models;

namespace BankApp.Server.Services.Interfaces
{
	public interface IAccountService
	{
		Account? Authenticate(string AccountNumber, string Pin);
		IEnumerable<Account> GetAccounts();
		CreatedAccountModel Create(Account account);
		Account Update(Account account, string Pin, string? PinToBeUpdated);
		string GenerateToken(string AccountNumber, string role);
		Account Delete(int id);
		Account? GetById(int id);
		Account? GetByAccountNumber(string AccountNumber);
	}
}
