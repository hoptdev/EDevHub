using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatService.Migrations
{
    /// <inheritdoc />
    public partial class Initz2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "Messages",
                newName: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Messages",
                newName: "ToId");
        }
    }
}
