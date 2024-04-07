using Microsoft.EntityFrameworkCore.Migrations;

namespace TLA.Identity.Api.Data.Migrations
{
    public partial class AlterTbRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "RefreshTokens",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "RefreshTokens",
                newName: "Username");
        }
    }
}
