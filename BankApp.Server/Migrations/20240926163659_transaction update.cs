using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class transactionupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountBalance",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionTitle",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBalance",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionTitle",
                table: "Transactions");
        }
    }
}
