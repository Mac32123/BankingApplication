using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApp.Server.Models
{
    [Table("Accounts")]
	public class Account
	{
		[Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        Random rand = new Random();

        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long) rand.NextDouble() * 9000000000L + 1000000000L);
            AccountName = $"{FirstName} {LastName}";
        }
    }

    public enum AccountType
    {
        Savings,        //konto oszczędnościowe
        Current,        //konto walutowe
        Corporate,      //konto firmowe
        Government      //konto rządowe
        //można dodawać kolejne
    }

}
