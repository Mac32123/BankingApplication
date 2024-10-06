using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class transactionupdateaddedaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionAccount",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionAccount",
                table: "Transactions");
        }
    }
}
