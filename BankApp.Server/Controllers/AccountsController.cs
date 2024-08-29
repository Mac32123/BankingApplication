using AutoMapper;
using BankApp.Server.Identity;
using BankApp.Server.Models;
using BankApp.Server.Services.Interfaces;
using BankApp.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace BankApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class AccountsController : ControllerBase
	{
		private IAccountService _accountService;
		private IMapper _mapper;
		private AppSettings _settings;

		public AccountsController(IAccountService accountService, IMapper mapper, IOptions<AppSettings> settings)
        {
			_accountService = accountService;
			_mapper = mapper;
			_settings = settings.Value;
        }


		[HttpPost]
		[Route("register")]
		//[RequiresClaim(ClaimTypes.Role, "Administrator")]
		[AllowAnonymous]
		public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
		{
			if (!ModelState.IsValid) return BadRequest(newAccount);
			try
			{
				var account = _mapper.Map<Account>(newAccount);
				return Ok(_accountService.Create(account));
			}
			catch (Exception ex)
			{
				return BadRequest(new ErrorResponse() {title="Validation error", status=400, message=ex.Message});
			}
		}

		[HttpGet]
		[Route("get_all_accounts")]
		//[RequiresClaim(ClaimTypes.Role, "Administrator")]

		public IActionResult GetAllAccounts()
		{
			var accounts = _accountService.GetAccounts();
			var cleanedAccounts = _mapper.Map<List<GetAccountModel>>(accounts);
			return Ok(cleanedAccounts);
		}

		[HttpPost]
		[Route("authenticate")]
		[AllowAnonymous]

		public IActionResult Authenticate([FromBody] AuthenticateAccountModel model)
		{
			if (!ModelState.IsValid) return BadRequest(model);
			
			Account? acc = _accountService.Authenticate(model.AccountNumber, model.Pin);
			if (acc is null) return Unauthorized();
			else 
			{ 
				AuthenticatedAccountModel AuthAcc = _mapper.Map<AuthenticatedAccountModel>(acc);
				if (AuthAcc.AccountType == AccountType.BankManagement) AuthAcc.AuthorizationToken = _accountService.GenerateToken(AuthAcc.AccountNumberGenerated, "Administrator");
				else AuthAcc.AuthorizationToken = AuthAcc.AuthorizationToken = _accountService.GenerateToken(AuthAcc.AccountNumberGenerated, "User");
				return Ok(AuthAcc);
			}
		}

		[HttpGet]
		[Route("get_by_account_number")]
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult GetByAccountNumber(string accountNumber)
		{
			if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digits");

			var account = _accountService.GetByAccountNumber(accountNumber);
			var cleanedAccount = _mapper.Map<GetAccountModel>(account);
			return Ok(cleanedAccount);
		}
		[HttpGet]
		[Route("get_by_id")]
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult GetById(int id)
		{
			var account = _accountService.GetById(id);
			var cleanedAccount = _mapper.Map<GetAccountModel>(account);
			return Ok(cleanedAccount);
		}

		[HttpPost]
		[Route("update")]
		public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
		{
			if (!ModelState.IsValid) return BadRequest(model);
			var account = _mapper.Map<Account>(model);
			try
			{
				return Ok(_accountService.Update(account, model.Pin, model.PinToBeUpdated));
			}
			catch (Exception ex)
			{
				return Unauthorized(new ErrorResponse() { title = "Validation error", status = 401, message = ex.Message });
			}
		}

		[HttpDelete]
		[Route("delete")]               //zrobić wymaganie żeby nie było pieniędzy na koncie
		[RequiresClaim(ClaimTypes.Role, "Administrator")]
		public IActionResult Delete(int id)
		{
			try
			{
				return Ok(Ok(Ok(Ok(Ok(Ok(_accountService.Delete(id)))))));
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}


	}
}
