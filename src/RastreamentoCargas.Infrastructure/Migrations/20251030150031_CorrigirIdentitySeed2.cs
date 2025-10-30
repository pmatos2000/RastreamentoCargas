using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirIdentitySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // O último ID de Cliente inserido foi 10.
            // O próximo deve ser 11. O RESEED é o último valor usado.
            migrationBuilder.Sql("DBCC CHECKIDENT('Clients', RESEED, 10);");

            // O último ID de User inserido foi 11 (Admin).
            // O próximo deve ser 12.
            migrationBuilder.Sql("DBCC CHECKIDENT('AspNetUsers', RESEED, 11);");

            // O último ID de Role inserido foi 1 (Admin).
            // O próximo deve ser 2.
            migrationBuilder.Sql("DBCC CHECKIDENT('AspNetRoles', RESEED, 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Se dermos rollback, voltamos ao estado quebrado (necessário para o EF)
            migrationBuilder.Sql("DBCC CHECKIDENT('Clients', RESEED, 0);");
            migrationBuilder.Sql("DBCC CHECKIDENT('AspNetUsers', RESEED, 0);");
            migrationBuilder.Sql("DBCC CHECKIDENT('AspNetRoles', RESEED, 0);");
        }
    }
}