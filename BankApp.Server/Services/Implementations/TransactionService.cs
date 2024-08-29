using BankApp.Server.Database;
using BankApp.Server.Models;
using BankApp.Server.Services.Interfaces;
using BankApp.Server.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BankApp.Server.Services.Implementations
{
	public class TransactionService : ITransactionService
	{
		private BankDbContext _dbContext;
		private ILogger<TransactionService> _logger;
		private AppSettings _settings;
		private static string _BankSettlementAccount;
		private readonly IAccountService _accountService;

        public TransactionService(BankDbContext dbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
			_dbContext = dbContext;
			_logger = logger;
			_settings = settings.Value;
			_BankSettlementAccount = _settings.BankSettlementAccount;
			_accountService = accountService;
		}

        public Response CreateNewTransaction(Transaction transaction)
		{
			Response response = new Response();
			_dbContext.transactions.Add(transaction);
			_dbContext.SaveChanges();
			response.ResponseCode = "00";
			response.ResponseMessage = "Transaction created successfully!";
			response.Data = null;

			return response;
		}

		public Response FindTransactionByDate(DateTime date)
		{
			Response response = new Response();
			var transaction = _dbContext.transactions.Where(x => x.TransactionDate == date).ToList();
			response.ResponseCode = "00";
			response.ResponseMessage = "Transaction created successfully!";
			response.Data = transaction;

			return response;
		}

		public Response MakeDeposit(string AccountNumber, decimal Amount)
		{
			Response response = new Response();
			Account? sourceAccount;
			Account? DestinationAccount;
			Transaction transaction = new Transaction();

			var authUser = _accountService.GetByAccountNumber(AccountNumber);

			try
			{
				sourceAccount = _accountService.GetByAccountNumber(_BankSettlementAccount);
				DestinationAccount = _accountService.GetByAccountNumber(AccountNumber);

				sourceAccount.CurrentAccountBalance -= Amount;
				DestinationAccount.CurrentAccountBalance += Amount;

				if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
					(_dbContext.Entry(DestinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)) 
				{ 
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "00";
					response.ResponseMessage = "Transaction successfull!";
					response.Data = null;
				}
				else
				{
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "02";
					response.ResponseMessage = "Transaction failed!";
					response.Data = null;
				}

			}
			catch (Exception ex)
			{
				_logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
			}

			transaction.TransactionType = TranType.Deposit;
			transaction.TransactionSourceAccount = _BankSettlementAccount;
			transaction.TransactionDestinationAccount = AccountNumber;
			transaction.TransactionAmmount = Amount;
			transaction.TransactionDate = DateTime.Now;
			transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(
				transaction.TransactionSourceAccount) } TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(
					transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(
						transaction.TransactionAmmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {
							transaction.TransactionStatus}";

			_dbContext.Add(transaction);
			_dbContext.SaveChanges();

			return response;

		}

		public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
		{
			Response response = new Response();
			Account? sourceAccount;
			Account? DestinationAccount;
			Transaction transaction = new Transaction();

			var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
			if (authUser == null) throw new ApplicationException("Inwvalid credentials");
			if (authUser.CurrentAccountBalance < Amount) throw new ApplicationException($"Not enought money: {authUser.CurrentAccountBalance}.");

			try
			{
				DestinationAccount = _accountService.GetByAccountNumber(ToAccount);
				sourceAccount = _accountService.GetByAccountNumber(FromAccount);

				sourceAccount.CurrentAccountBalance -= Amount;
				DestinationAccount.CurrentAccountBalance += Amount;

				if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
					(_dbContext.Entry(DestinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
				{
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "00";
					response.ResponseMessage = "Transaction successfull!";
					response.Data = null;
				}
				else
				{
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "02";
					response.ResponseMessage = "Transaction failed!";
					response.Data = null;
				}

			}
			catch (Exception ex)
			{
				_logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
			}

			transaction.TransactionType = TranType.Transfer;
			transaction.TransactionSourceAccount = FromAccount;
			transaction.TransactionDestinationAccount = ToAccount;
			transaction.TransactionAmmount = Amount;
			transaction.TransactionDate = DateTime.Now;
			transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(
				transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(
					transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(
						transaction.TransactionAmmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {
							transaction.TransactionStatus}";

			_dbContext.Add(transaction);
			_dbContext.SaveChanges();

			return response;
		}

		public Response MakeWithdrawal(string AccountNumber, decimal Amount)
		{
			Response response = new Response();
			Account? sourceAccount;
			Account? DestinationAccount;
			Transaction transaction = new Transaction();

			var authUser = _accountService.GetByAccountNumber(AccountNumber);
			if (authUser.CurrentAccountBalance < Amount) throw new ApplicationException($"Not enought money: {authUser.CurrentAccountBalance}.");

			try
			{
				DestinationAccount = _accountService.GetByAccountNumber(_BankSettlementAccount);
				sourceAccount = _accountService.GetByAccountNumber(AccountNumber);

				sourceAccount.CurrentAccountBalance -= Amount;
				DestinationAccount.CurrentAccountBalance += Amount;

				if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
					(_dbContext.Entry(DestinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
				{
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "00";
					response.ResponseMessage = "Transaction successfull!";
					response.Data = null;
				}
				else
				{
					transaction.TransactionStatus = TranStatus.Success;
					response.ResponseCode = "02";
					response.ResponseMessage = "Transaction failed!";
					response.Data = null;
				}

			}
			catch (Exception ex)
			{
				_logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
			}

			transaction.TransactionType = TranType.Withdrawal;
			transaction.TransactionSourceAccount = AccountNumber;
			transaction.TransactionDestinationAccount = _BankSettlementAccount;
			transaction.TransactionAmmount = Amount;
			transaction.TransactionDate = DateTime.Now;
			transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(
				transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(
					transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(
						transaction.TransactionAmmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {
							transaction.TransactionStatus}";

			_dbContext.Add(transaction);
			_dbContext.SaveChanges();

			return response;
		}
	}
}
