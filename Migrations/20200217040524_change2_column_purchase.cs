using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Razor.Migrations
{
    public partial class change2_column_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Purchase");

            migrationBuilder.AlterColumn<string>(
                name: "transaction_time",
                table: "Transaction_Details",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "transaction_time",
                table: "Transaction_Details",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Purchase",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
