using Microsoft.EntityFrameworkCore.Migrations;

namespace WziumStars.Data.Migrations
{
    public partial class AddProduktToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produkt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Images = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produkt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produkt_Kategoria_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Kategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Produkt_Podkategoria_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "Podkategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produkt_CategoryId",
                table: "Produkt",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Produkt_SubCategoryId",
                table: "Produkt",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produkt");
        }
    }
}
