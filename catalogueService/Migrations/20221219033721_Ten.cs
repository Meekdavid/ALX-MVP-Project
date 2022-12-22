using Microsoft.EntityFrameworkCore.Migrations;

namespace catalogueService.Migrations
{
    public partial class Ten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "_usersuserId",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Customers",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Customers");
        }
    }
}
