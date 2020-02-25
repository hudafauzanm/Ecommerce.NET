using Microsoft.EntityFrameworkCore.Migrations;

namespace Razor.Migrations
{
    public partial class edit_column_notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "receiver_id",
                table: "Notification",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "Notification",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiver_id",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "Notification");
        }
    }
}
