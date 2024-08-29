using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankApp.Server.Models
{
	public class AuthenticatedAccountModel
	{
			public int Id { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string AccountName { get; set; }
			public string PhoneNumber { get; set; }
			public string Email { get; set; }
			public decimal CurrentAccountBalance { get; set; }
			public AccountType AccountType { get; set; }
			public string AccountNumberGenerated { get; set; }
			public DateTime DateCreated { get; set; }
			public DateTime DateUpdated { get; set; }
			public string AuthorizationToken { get; set; }
	}
}
