using Microsoft.EntityFrameworkCore.Migrations;

namespace WziumStars.Data.Migrations
{
    public partial class AddCountToProduktTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Produkt",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Produkt");
        }
    }
}
