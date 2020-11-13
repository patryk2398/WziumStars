using Microsoft.EntityFrameworkCore.Migrations;

namespace WziumStars.Data.Migrations
{
    public partial class ChangeStreetAddressInApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
