using BankApp.Server.Database;
using BankApp.Server.Models;
using BankApp.Server.Services.Interfaces;
using System.Text;

namespace BankApp.Server.Services.Implementations
{
	public class AccountService : IAccountService
	{
		private BankDbContext _dbContext;

        public AccountService(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public Account? Authenticate(string AccountNumber, string Pin)
		{
			var account = _dbContext.accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
			if (account == null)
			{
				return null;
			}
			if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
			{
				return null;
			}
			return account;
		}

		private static bool VerifyPinHash(string  pin, byte[] pinHash, byte[] pinSalt)
		{
			if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentNullException("Pin");
			using(var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
			{
				var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pin));
				if (computedPinHash.Length != pinHash.Length) return false;
				for (int i = 0; i < computedPinHash.Length; i++)
				{
					if (computedPinHash[i] != pinHash[i]) return false;
				}

				return true;
			}
		}

		public Account Create(Account account, string Pin, string ConfirmPin)
		{
			if (_dbContext.accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account with this email already exists");
			if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match", "Pin");
			byte[] pinHash, pinSalt;
			CreatePinHash(Pin,  out pinHash, out pinSalt);
		
			account.PinHash = pinHash;
			account.PinSalt = pinSalt;

			_dbContext.accounts.Add(account);
			_dbContext.SaveChanges();

			return account;

		}

		private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512)
			{
				pinSalt = hmac.Key;
				pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
			}
		}
		public void Delete(int id)
		{
			var account = _dbContext.accounts.Find(id);
            if (account != null)
            {
				_dbContext.accounts.Remove(account);
				_dbContext.SaveChanges();
            }
        }

		public IEnumerable<Account> GetAccounts()
		{
			return _dbContext.accounts.ToList();
		}

		public Account? GetByAccountNumber(string AccountNumber)
		{
			var account = _dbContext.accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
			if (account == null) return null;
			return account;
		}

		public Account? GetById(int id)
		{
			var account = _dbContext.accounts.Find(id);
			if (account != null)
			{
				return null;
			}
			return account;
		}

		public void Update(Account account, string? Pin = null)
		{
			//dodać gdzieś potwierdzenie emaila i telefonu
			var accountToBeUpdated = _dbContext.accounts.Where(x => x.Email == account.Email).SingleOrDefault();
			if (accountToBeUpdated == null) throw new ApplicationException("Account does not exists");
			if (!string.IsNullOrWhiteSpace(account.Email))
			{
				if (_dbContext.accounts.Any(x => x.Email == account.Email)) throw new ApplicationException($"This emai: {account.Email} is already taken");
				accountToBeUpdated.Email = account.Email;	
			}
			if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
			{
				if (_dbContext.accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException($"This phone number: {account.PhoneNumber} is already taken");
				accountToBeUpdated.Email = account.Email;
			}
			if (!string.IsNullOrWhiteSpace(Pin))
			{
				byte[] pinHash, pinSalt;
				CreatePinHash(Pin, out pinHash, out pinSalt);

				accountToBeUpdated.PinHash = pinHash;
				accountToBeUpdated.PinSalt = pinSalt;
			}

			_dbContext.accounts.Update(accountToBeUpdated);
			_dbContext.SaveChanges();

		}
	}
}
