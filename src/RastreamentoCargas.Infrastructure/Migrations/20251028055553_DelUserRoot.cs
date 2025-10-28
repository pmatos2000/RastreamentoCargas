using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DelUserRoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Discriminator", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, "5cf97b44-6355-46ca-8b09-854365615a72", new DateTime(2025, 10, 28, 5, 22, 20, 785, DateTimeKind.Utc).AddTicks(1367), "System", "User", "root@sistema.com", true, "Administrador Raiz", true, false, null, "ROOT@SISTEMA.COM", "ROOT", "AQAAAAIAAYagAAAAEI7DEXLRpqJNdXsklN2jlaRgesTkHDUFV+LErvU0PMgeN6SLPtBYQ1M+3beDaJwshw==", null, false, "16df264e-c9c3-4b27-ad9f-f45da9cdfd60", false, null, null, "root" });
        }
    }
}
