using BankApp.Server.Database;
using BankApp.Server.Models;
using BankApp.Server.Services.Interfaces;
using BankApp.Server.Utils;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Fare;
using AutoMapper;

namespace BankApp.Server.Services.Implementations
{
	public class AccountService : IAccountService
	{
		private BankDbContext _dbContext;
		private AppSettings _settings;
		private JwtSettings _jwtSettings;
		private IMapper _mapper;
		private static string _BankManagementAccount;
		private Random rand = new Random();
		private Xeger xeger = new Xeger(@"^(?=.* [a - z])(?=.* [A - Z])(?=.*\d)(?=.* [@$! % *? &])[A - Za - z\d@$! % *? &]{8,20}$");

		public AccountService(BankDbContext dbContext, IOptions<AppSettings> settings, IOptions<JwtSettings> options, IMapper mapper)
        {
            _dbContext = dbContext;
			_settings = settings.Value;
			_jwtSettings = options.Value;
			_BankManagementAccount = _settings.BankManagementAccount;
			_mapper = mapper;
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

		public CreatedAccountModel Create(Account account)
		{
			if (_dbContext.accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account with this email already exists");	//być może usunąć
			byte[] pinHash, pinSalt;
			var Pin = xeger.Generate();
			CreatePinHash(Pin,  out pinHash, out pinSalt);
		
			account.PinHash = pinHash;
			account.PinSalt = pinSalt;

			object? duplicate;
			do
			{
				account.AccountNumberGenerated = Convert.ToString((long)(rand.NextDouble() * 9000000000L + 1000000000L));
				duplicate = _dbContext.accounts.Where(x => x.AccountNumberGenerated == account.AccountNumberGenerated).SingleOrDefault();
			}
			while (duplicate is not null);

			account.AccountName = $"{account.FirstName} {account.LastName}";

			_dbContext.accounts.Add(account);
			_dbContext.SaveChanges();
			

			return _mapper.Map<CreatedAccountModel>(account);

		}

		private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				pinSalt = hmac.Key;
				pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
			}
		}
		public Account Delete(int id)
		{
			var account = _dbContext.accounts.Find(id);
			if (account != null)
			{
				_dbContext.accounts.Remove(account);
				_dbContext.SaveChanges();
				return account;
			}
			else throw new ApplicationException("Account not found");
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

		public Account Update(Account account, string Pin, string? PinToBeUpdated)
		{
			//dodać gdzieś potwierdzenie emaila i telefonu
			
			var accountToBeUpdated = Authenticate(account.AccountNumberGenerated, Pin);

			if (accountToBeUpdated == null) throw new ApplicationException("Account number or password are wrong");
			if (!string.IsNullOrWhiteSpace(account.Email))
			{
				if (_dbContext.accounts.Any(x => x.Email == account.Email && x.AccountNumberGenerated != account.AccountNumberGenerated)) throw new ApplicationException($"This emai: {account.Email} is already taken");
				accountToBeUpdated.Email = account.Email;	
			}
			if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
			{
				if (_dbContext.accounts.Any(x => x.PhoneNumber == account.PhoneNumber && x.AccountNumberGenerated != account.AccountNumberGenerated)) throw new ApplicationException($"This phone number: {account.PhoneNumber} is already taken");
				accountToBeUpdated.PhoneNumber = account.PhoneNumber;
			}
			if (!string.IsNullOrWhiteSpace(PinToBeUpdated))
			{
				byte[] pinHash, pinSalt;
				CreatePinHash(PinToBeUpdated, out pinHash, out pinSalt);

				accountToBeUpdated.PinHash = pinHash;
				accountToBeUpdated.PinSalt = pinSalt;
				
			}
			accountToBeUpdated.DateUpdated = DateTime.UtcNow;
			_dbContext.accounts.Update(accountToBeUpdated);
			_dbContext.SaveChanges();
			return accountToBeUpdated;
		}

		public string GenerateToken(string AccountNumber, string role)
		{

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings.Key)),
				SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, AccountNumber),
				new Claim(ClaimTypes.Role, role)
			};

			var securityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				expires: DateTime.UtcNow.AddHours(1),
				claims: claims,
				signingCredentials: signingCredentials);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}
	}
}
