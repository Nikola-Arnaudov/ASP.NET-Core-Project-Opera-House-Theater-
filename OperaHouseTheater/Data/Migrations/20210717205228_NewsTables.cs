using Microsoft.EntityFrameworkCore.Migrations;

namespace OperaHouseTheater.Data.Migrations
{
    public partial class NewsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsPictureUrl",
                table: "News",
                newName: "NewsImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsImageUrl",
                table: "News",
                newName: "NewsPictureUrl");
        }
    }
}
