using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_AspNetUsers_OperatorId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Clients_ClientId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_TripHistory_Trip_TripId",
                table: "TripHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripHistory",
                table: "TripHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trip",
                table: "Trip");

            migrationBuilder.RenameTable(
                name: "TripHistory",
                newName: "TripHistories");

            migrationBuilder.RenameTable(
                name: "Trip",
                newName: "Trips");

            migrationBuilder.RenameIndex(
                name: "IX_TripHistory_TripId",
                table: "TripHistories",
                newName: "IX_TripHistories_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_TripHistory_ExternalId",
                table: "TripHistories",
                newName: "IX_TripHistories_ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_OperatorId",
                table: "Trips",
                newName: "IX_Trips_OperatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_ExternalId",
                table: "Trips",
                newName: "IX_Trips_ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_ClientId",
                table: "Trips",
                newName: "IX_Trips_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripHistories",
                table: "TripHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripHistories_Trips_TripId",
                table: "TripHistories",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_AspNetUsers_OperatorId",
                table: "Trips",
                column: "OperatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Clients_ClientId",
                table: "Trips",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripHistories_Trips_TripId",
                table: "TripHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_AspNetUsers_OperatorId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Clients_ClientId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripHistories",
                table: "TripHistories");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "Trip");

            migrationBuilder.RenameTable(
                name: "TripHistories",
                newName: "TripHistory");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_OperatorId",
                table: "Trip",
                newName: "IX_Trip_OperatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_ExternalId",
                table: "Trip",
                newName: "IX_Trip_ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_ClientId",
                table: "Trip",
                newName: "IX_Trip_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_TripHistories_TripId",
                table: "TripHistory",
                newName: "IX_TripHistory_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_TripHistories_ExternalId",
                table: "TripHistory",
                newName: "IX_TripHistory_ExternalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trip",
                table: "Trip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripHistory",
                table: "TripHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_AspNetUsers_OperatorId",
                table: "Trip",
                column: "OperatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Clients_ClientId",
                table: "Trip",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripHistory_Trip_TripId",
                table: "TripHistory",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
