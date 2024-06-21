using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class Initz4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_UserId",
                table: "Vacancies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Users_UserId",
                table: "Vacancies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Users_UserId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_UserId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Users");
        }
    }
}
