using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendUserWithOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Discriminator", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, "0b13856b-6700-4984-b298-b7202bf1eb68", new DateTime(2025, 10, 28, 4, 50, 33, 762, DateTimeKind.Utc).AddTicks(8571), "System", "User", "root@sistema.com", true, "Administrador Raiz", true, false, null, "ROOT@SISTEMA.COM", "ROOT", "AQAAAAIAAYagAAAAELwYWRrkBOOuHSnDoZS0k8cLyaYK/xl+GGQ7fKh07BvNyZykheFbMxvTTOTUf5QqNw==", null, false, "79150229-ee80-46b2-8ebf-6660bfb0140c", false, null, null, "root" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4ee70a7-1cb7-4f3e-85f7-2e671ecb20a4", new DateTime(2025, 10, 28, 2, 5, 40, 334, DateTimeKind.Utc).AddTicks(6375), "AQAAAAIAAYagAAAAEN6mBON0sv3WqPUGCuekO+YtSKoNlfN5O0uyKK2pZMJeaS/RrDp4b0NHg4ORSaRTVA==", "5312c8ff-51df-4bf0-8a5b-6a7f0cb3958e" });
        }
    }
}
