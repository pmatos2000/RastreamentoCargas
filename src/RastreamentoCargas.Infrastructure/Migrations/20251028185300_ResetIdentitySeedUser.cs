using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ResetIdentitySeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DBCC CHECKIDENT('AspNetUsers', RESEED, 0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
