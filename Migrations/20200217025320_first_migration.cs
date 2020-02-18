using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Razor.Migrations
{
    public partial class first_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    va_numbers = table.Column<int>(nullable: false),
                    bank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ActionModel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    method = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionModel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total = table.Column<double>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    published_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_name = table.Column<string>(nullable: true),
                    url_img = table.Column<string>(nullable: true),
                    price = table.Column<double>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    rating = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    published_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    role = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    published_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction_Details",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<string>(nullable: true),
                    merchant_id = table.Column<string>(nullable: true),
                    order_id = table.Column<string>(nullable: true),
                    currency = table.Column<string>(nullable: true),
                    gross_amount = table.Column<double>(nullable: false),
                    transaction_time = table.Column<DateTime>(nullable: false),
                    transaction_status = table.Column<string>(nullable: true),
                    status_code = table.Column<string>(nullable: true),
                    status_message = table.Column<string>(nullable: true),
                    fraud_status = table.Column<string>(nullable: true),
                    signature_key = table.Column<string>(nullable: true),
                    Account_id = table.Column<int>(nullable: false),
                    Action_id = table.Column<int>(nullable: false),
                    Actionsid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction_Details", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transaction_Details_Account_Account_id",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Details_ActionModel_Actionsid",
                        column: x => x.Actionsid,
                        principalTable: "ActionModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaksi",
                columns: table => new
                {
                    Item_id = table.Column<int>(nullable: false),
                    Cart_id = table.Column<int>(nullable: false),
                    User_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaksi", x => new { x.Item_id, x.Cart_id, x.User_id });
                    table.ForeignKey(
                        name: "FK_Transaksi_Cart_Cart_id",
                        column: x => x.Cart_id,
                        principalTable: "Cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaksi_Item_Item_id",
                        column: x => x.Item_id,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaksi_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nama_pemesan = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    alamat = table.Column<string>(nullable: true),
                    kodepos = table.Column<string>(nullable: true),
                    payment_type = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    published_at = table.Column<DateTime>(nullable: false),
                    transaction_id = table.Column<int>(nullable: false),
                    User_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.id);
                    table.ForeignKey(
                        name: "FK_Purchase_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchase_Transaction_Details_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "Transaction_Details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_User_id",
                table: "Purchase",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_transaction_id",
                table: "Purchase",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Details_Account_id",
                table: "Transaction_Details",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Details_Actionsid",
                table: "Transaction_Details",
                column: "Actionsid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaksi_Cart_id",
                table: "Transaksi",
                column: "Cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaksi_User_id",
                table: "Transaksi",
                column: "User_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "Transaksi");

            migrationBuilder.DropTable(
                name: "Transaction_Details");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ActionModel");
        }
    }
}
