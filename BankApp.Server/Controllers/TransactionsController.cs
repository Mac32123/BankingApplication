using AutoMapper;
using BankApp.Server.Identity;
using BankApp.Server.Models;
using BankApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BankApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TransactionsController : ControllerBase
	{

		private ITransactionService _transactionServices;
		private IMapper _mapper;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
			_transactionServices = transactionService;
			_mapper = mapper;
        }

		[HttpPost]
		[Route("create_new_transaction")]
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult CreateNewTransaction([FromBody] TransactionRequestModel transactionRequest)
		{
			if (!ModelState.IsValid) return BadRequest(transactionRequest);
			
			var transaction = _mapper.Map<Transaction>(transactionRequest);
			return Ok(_transactionServices.CreateNewTransaction(transaction));
		}

		[HttpGet]
		[Route("make_deposit")]
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult MakeDeposit(string AccountNumber, decimal Amount)
		{
			if (!Regex.IsMatch(AccountNumber, @"^\d{26}$")) return BadRequest("Account number must be 26-digits");
			try
			{
				return Ok(_transactionServices.MakeDeposit(AccountNumber, Amount));
			}
			catch (Exception ex)
			{
				return Unauthorized(new ErrorResponse() { title = "Validation error", status = 401, message = ex.Message });
			}
		}

		[HttpGet]
		[Route("make_withdrawal")]
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount)
		{
			if (!Regex.IsMatch(AccountNumber, @"^\d{26}$")) return BadRequest("Account number must be 10-digits");
			try
			{
				return Ok(_transactionServices.MakeWithdrawal(AccountNumber, Amount));
			}
			catch (Exception ex)
			{
				return Unauthorized(new ErrorResponse() { title = "Validation error", status = 401, message = ex.Message });
			}
		}

		[HttpPost]
		[Route("make_transfer")]
		public IActionResult MakeTransfer(string ToAccount, decimal Amount, string title)
		{
			var FromAccount = HttpContext.User.FindFirstValue(ClaimTypes.Name);
			try
			{
				return Ok(_transactionServices.MakeFundsTransfer(FromAccount, ToAccount, Amount, title));
			}
			catch (Exception ex)
			{
				return BadRequest(new ErrorResponse() { title = "Validation error", status = 400, message = ex.Message });
			}
		}

		[HttpGet]
		[Route("get_transaction")]
		public IActionResult getTransaction(int toFrom, DateTime? startingDate, DateTime? endingDate)
		{
			var Account = HttpContext.User.FindFirstValue(ClaimTypes.Name);

			return Ok(_transactionServices.GetAllTransactions(Account, toFrom, startingDate, endingDate));
		}
	}
}
