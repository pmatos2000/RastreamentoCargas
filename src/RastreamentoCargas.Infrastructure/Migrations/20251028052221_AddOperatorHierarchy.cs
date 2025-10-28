using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cf97b44-6355-46ca-8b09-854365615a72", new DateTime(2025, 10, 28, 5, 22, 20, 785, DateTimeKind.Utc).AddTicks(1367), "AQAAAAIAAYagAAAAEI7DEXLRpqJNdXsklN2jlaRgesTkHDUFV+LErvU0PMgeN6SLPtBYQ1M+3beDaJwshw==", "16df264e-c9c3-4b27-ad9f-f45da9cdfd60" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b13856b-6700-4984-b298-b7202bf1eb68", new DateTime(2025, 10, 28, 4, 50, 33, 762, DateTimeKind.Utc).AddTicks(8571), "AQAAAAIAAYagAAAAELwYWRrkBOOuHSnDoZS0k8cLyaYK/xl+GGQ7fKh07BvNyZykheFbMxvTTOTUf5QqNw==", "79150229-ee80-46b2-8ebf-6660bfb0140c" });
        }
    }
}
