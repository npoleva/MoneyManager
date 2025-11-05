using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnedTypesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InitialBalance",
                table: "Wallets",
                newName: "InitialBalance_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Wallets",
                newName: "InitialBalance_Currency");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transactions",
                newName: "Description_Value");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Transactions",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "Amount_Value");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Wallets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Description_Value",
                table: "Transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Amount_Value",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                table: "Transactions",
                newName: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Wallets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
