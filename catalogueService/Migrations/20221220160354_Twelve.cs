using Microsoft.EntityFrameworkCore.Migrations;

namespace catalogueService.Migrations
{
    public partial class Twelve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users__usersuserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers__usersuserId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "_usersuserId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "customerId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_customerId",
                table: "Users",
                column: "customerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Customers_customerId",
                table: "Users",
                column: "customerId",
                principalTable: "Customers",
                principalColumn: "customerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Customers_customerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_customerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "customerId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "_usersuserId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers__usersuserId",
                table: "Customers",
                column: "_usersuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users__usersuserId",
                table: "Customers",
                column: "_usersuserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
