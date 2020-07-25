using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.Data.Migrations
{
    public partial class PhotoIdAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicIdAvatar",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicIdAvatar",
                table: "Photos");
        }
    }
}
