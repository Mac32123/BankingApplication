using System.ComponentModel.DataAnnotations;

namespace BankApp.Server.DataAnnotations
{
	public class PasswordMinLenght : ValidationAttribute
	{
		private int _minLengt {  get; set; }
        public PasswordMinLenght(int minLenght)
        {
            _minLengt = minLenght;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var valueString = value != null ? value.ToString() : null;

			// If value is empty, show Success
			if (string.IsNullOrWhiteSpace(valueString))
			{
				return ValidationResult.Success;
			}
			else if (valueString.Length < _minLengt)
			{
				return new ValidationResult("Password too short");
			}
			else return ValidationResult.Success;
        }
	}
}
