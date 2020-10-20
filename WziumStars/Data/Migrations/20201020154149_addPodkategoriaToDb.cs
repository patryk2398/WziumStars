﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WziumStars.Data.Migrations
{
    public partial class addPodkategoriaToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Podkategoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podkategoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Podkategoria_Kategoria_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Kategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Podkategoria_CategoryId",
                table: "Podkategoria",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Podkategoria");
        }
    }
}
