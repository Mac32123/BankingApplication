using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class ActualAccountNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualAccountNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualAccountNumber",
                table: "Accounts");
        }
    }
}
