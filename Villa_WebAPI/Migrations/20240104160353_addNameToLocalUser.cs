using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaAppWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addNameToLocalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LocalUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 17, 3, 53, 386, DateTimeKind.Local).AddTicks(9596));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 17, 3, 53, 386, DateTimeKind.Local).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 17, 3, 53, 386, DateTimeKind.Local).AddTicks(9657));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 17, 3, 53, 386, DateTimeKind.Local).AddTicks(9659));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 17, 3, 53, 386, DateTimeKind.Local).AddTicks(9661));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LocalUser");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 16, 10, 10, 8, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 16, 10, 10, 8, DateTimeKind.Local).AddTicks(7161));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 16, 10, 10, 8, DateTimeKind.Local).AddTicks(7163));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 16, 10, 10, 8, DateTimeKind.Local).AddTicks(7165));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 4, 16, 10, 10, 8, DateTimeKind.Local).AddTicks(7167));
        }
    }
}
