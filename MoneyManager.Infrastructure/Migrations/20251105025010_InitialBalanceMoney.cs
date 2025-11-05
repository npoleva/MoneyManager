using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialBalanceMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InitialBalance_Currency",
                table: "Wallets",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "InitialBalance_Amount",
                table: "Wallets",
                newName: "InitialBalance");

            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                table: "Transactions",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "InitialBalance",
                table: "Wallets",
                newName: "InitialBalance_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Wallets",
                newName: "InitialBalance_Currency");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Transactions",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "Amount_Amount");
        }
    }
}
