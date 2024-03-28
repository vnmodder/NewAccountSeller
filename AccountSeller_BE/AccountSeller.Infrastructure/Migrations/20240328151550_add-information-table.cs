using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccountSeller.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addinformationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("28f98181-41e0-4f98-b903-67a5601c5459"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9e75580a-fe11-4c54-a845-e1995da01c0c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b760e44c-9537-4a08-8309-0ee277e4a8a2"));

            migrationBuilder.CreateTable(
                name: "InformationTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationTables", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5d222ccb-2b3b-4974-8a01-21e33d8abc58"), "3", "User", "User" },
                    { new Guid("8a93b9f8-4eb4-407f-ac81-56406bc36ae9"), "2", "Admin", "Admin" },
                    { new Guid("c6d793be-04b8-4335-8bcd-c93579ea359f"), "1", "Zero", "Zero" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformationTables");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5d222ccb-2b3b-4974-8a01-21e33d8abc58"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8a93b9f8-4eb4-407f-ac81-56406bc36ae9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c6d793be-04b8-4335-8bcd-c93579ea359f"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("28f98181-41e0-4f98-b903-67a5601c5459"), "1", "Zero", "Zero" },
                    { new Guid("9e75580a-fe11-4c54-a845-e1995da01c0c"), "3", "User", "User" },
                    { new Guid("b760e44c-9537-4a08-8309-0ee277e4a8a2"), "2", "Admin", "Admin" }
                });
        }
    }
}
