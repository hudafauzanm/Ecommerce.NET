using Microsoft.EntityFrameworkCore.Migrations;

namespace Razor.Migrations
{
    public partial class change_FK_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Details_Account_Account_id",
                table: "Transaction_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Details_ActionModel_Actionsid",
                table: "Transaction_Details");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ActionModel");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_Details_Account_id",
                table: "Transaction_Details");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_Details_Actionsid",
                table: "Transaction_Details");

            migrationBuilder.DropColumn(
                name: "Account_id",
                table: "Transaction_Details");

            migrationBuilder.DropColumn(
                name: "Action_id",
                table: "Transaction_Details");

            migrationBuilder.DropColumn(
                name: "Actionsid",
                table: "Transaction_Details");

            migrationBuilder.AddColumn<string>(
                name: "_account",
                table: "Transaction_Details",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_actions",
                table: "Transaction_Details",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_account",
                table: "Transaction_Details");

            migrationBuilder.DropColumn(
                name: "_actions",
                table: "Transaction_Details");

            migrationBuilder.AddColumn<int>(
                name: "Account_id",
                table: "Transaction_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Action_id",
                table: "Transaction_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Actionsid",
                table: "Transaction_Details",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    va_numbers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ActionModel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionModel", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Details_Account_id",
                table: "Transaction_Details",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Details_Actionsid",
                table: "Transaction_Details",
                column: "Actionsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Details_Account_Account_id",
                table: "Transaction_Details",
                column: "Account_id",
                principalTable: "Account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Details_ActionModel_Actionsid",
                table: "Transaction_Details",
                column: "Actionsid",
                principalTable: "ActionModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
