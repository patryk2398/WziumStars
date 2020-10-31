using Microsoft.EntityFrameworkCore.Migrations;

namespace WziumStars.Data.Migrations
{
    public partial class AddNewFieldsInOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Firm",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "OrderHeader");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OrderHeader",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "AnotherDeliveryAddress",
                table: "OrderHeader",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryApartmentNumber",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCity",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCompany",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCountry",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryFirstName",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLastName",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryPostalCode",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryState",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStreet",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Paczkomat",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "AnotherDeliveryAddress",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryApartmentNumber",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryCity",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryCompany",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryCountry",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryFirstName",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryLastName",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryPostalCode",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryState",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "DeliveryStreet",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Paczkomat",
                table: "OrderHeader");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OrderHeader",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delivery",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Firm",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
