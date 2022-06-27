using Microsoft.EntityFrameworkCore.Migrations;

namespace BAL.Migrations
{
    public partial class InitData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1135f8df-c5cc-48e9-b9ae-dcf3bbe2911b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "625cea9e-afdd-49d0-a5d3-db5e1d13df6e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78c01279-371c-435f-afa7-cf2ba2a0555d", "8307b230-866e-46ce-8bad-0fd7fea625d1", "User", "USER" },
                    { "cc3a91bc-68f2-4f9a-b281-4761acba8320", "2e53de8f-20c0-4ebf-bf06-6cb2081cbb18", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "FilesId" },
                values: new object[,]
                {
                    { 1, "Games", 0L },
                    { 2, "Images", 0L },
                    { 3, "Videos", 0L },
                    { 4, "Books", 0L },
                    { 5, "Scripts", 0L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78c01279-371c-435f-afa7-cf2ba2a0555d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc3a91bc-68f2-4f9a-b281-4761acba8320");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "625cea9e-afdd-49d0-a5d3-db5e1d13df6e", "86738dc6-7246-4373-8cfe-78b2c1e890f2", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1135f8df-c5cc-48e9-b9ae-dcf3bbe2911b", "939110cb-0f26-4dd9-939a-084c785e57a4", "Administrator", "ADMINISTRATOR" });
        }
    }
}
