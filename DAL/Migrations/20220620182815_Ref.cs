using Microsoft.EntityFrameworkCore.Migrations;

namespace BAL.Migrations
{
    public partial class Ref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_CategoryId",
                table: "Files");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78c01279-371c-435f-afa7-cf2ba2a0555d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc3a91bc-68f2-4f9a-b281-4761acba8320");

            migrationBuilder.DropColumn(
                name: "FilesId",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "092e7c40-dcb0-4d02-84f2-56f9105ff78b", "16fa99a8-722e-43e7-95b0-33cff0eaf578", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9348faef-9fa6-44ab-a79e-ec0cf22c6f73", "30d0ca9e-2d44-4841-bc73-241ec0f51ada", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_CategoryId",
                table: "Files",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_CategoryId",
                table: "Files");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "092e7c40-dcb0-4d02-84f2-56f9105ff78b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9348faef-9fa6-44ab-a79e-ec0cf22c6f73");

            migrationBuilder.AddColumn<long>(
                name: "FilesId",
                table: "Categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78c01279-371c-435f-afa7-cf2ba2a0555d", "8307b230-866e-46ce-8bad-0fd7fea625d1", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc3a91bc-68f2-4f9a-b281-4761acba8320", "2e53de8f-20c0-4ebf-bf06-6cb2081cbb18", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_CategoryId",
                table: "Files",
                column: "CategoryId",
                unique: true);
        }
    }
}
