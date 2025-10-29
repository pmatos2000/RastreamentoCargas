using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsUniqueExternalIdAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ExternalId",
                table: "AspNetUsers",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ExternalId",
                table: "AspNetUsers");
        }
    }
}
