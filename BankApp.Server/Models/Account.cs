using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public enum AccountType
    {
        Savings,        //konto oszczędnościowe
        Current,        //konto walutowe
        Corporate,      //konto firmowe
        Government,     //konto rządowe
        BankSettlement, //konto banku do wypłaty i wpłaty pieniędzy
        BankManagement       //konto bankowe do zarządzania innymi kontami (np. do tworzenia kont)
        //można dodawać kolejne
    }

}
