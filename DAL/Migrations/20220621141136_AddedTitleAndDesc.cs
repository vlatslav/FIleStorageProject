using Microsoft.EntityFrameworkCore.Migrations;

namespace BAL.Migrations
{
    public partial class AddedTitleAndDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "092e7c40-dcb0-4d02-84f2-56f9105ff78b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9348faef-9fa6-44ab-a79e-ec0cf22c6f73");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Files",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "361f97e2-bcb9-443f-ade5-ba84d260b785", "b38463ba-8b4e-4190-8b11-c1ad35d144c1", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e7c31b95-c564-4775-b886-09704d478cb7", "0943dbef-ef29-4439-aeda-11bb5d165140", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "361f97e2-bcb9-443f-ade5-ba84d260b785");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7c31b95-c564-4775-b886-09704d478cb7");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Files");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "092e7c40-dcb0-4d02-84f2-56f9105ff78b", "16fa99a8-722e-43e7-95b0-33cff0eaf578", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9348faef-9fa6-44ab-a79e-ec0cf22c6f73", "30d0ca9e-2d44-4841-bc73-241ec0f51ada", "Administrator", "ADMINISTRATOR" });
        }
    }
}
