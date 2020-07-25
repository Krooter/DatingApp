using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.Data.Migrations
{
    public partial class AvatarPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlPhotoAvatar",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlPhotoAvatar",
                table: "Photos");
        }
    }
}
