using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TimeStampedTodos",
                columns: new[] { "Id", "TimeStamp" },
                values: new object[,]
                {
                    { 1, "1724162544" },
                    { 2, "1724162544" },
                    { 3, "1724162544" },
                    { 4, "1724162544" }
                });

            migrationBuilder.InsertData(
                table: "ValueTodos",
                columns: new[] { "Id", "Author", "Description", "Done", "Title", "TodoTimeEntityId" },
                values: new object[,]
                {
                    { 1, "author1", "description1", false, "title1", 1 },
                    { 2, "author2", "description2", false, "title2", 2 },
                    { 3, "author3", "description3", false, "title3", 3 },
                    { 4, "author4", "description4", false, "title4", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ValueTodos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ValueTodos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ValueTodos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ValueTodos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TimeStampedTodos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeStampedTodos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeStampedTodos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TimeStampedTodos",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
