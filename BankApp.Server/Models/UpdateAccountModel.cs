using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Server.Models
{
	public class UpdateAccountModel
	{
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		[Required]
		public string AccountNumberGenerated { get; set; }
		[Required]
		public string Pin {  get; set; }
		[MinLength(8, ErrorMessage = "Password to short (password should have at least 8 characters")]
		[MaxLength(20, ErrorMessage = "Password to long (password should have up to 20 characters")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password too weak")]
		public string PinToBeUpdated { get; set; }
		[Compare("PinToBeUpdated", ErrorMessage = "Pins do not match")]
		public string ConfirmPin { get; set; }
	}
}
