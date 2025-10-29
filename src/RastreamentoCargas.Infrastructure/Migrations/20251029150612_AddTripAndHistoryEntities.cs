using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripAndHistoryEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DestinationLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    OriginLatitude = table.Column<double>(type: "float", nullable: false),
                    OriginLongitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    CurrentLatitude = table.Column<double>(type: "float", nullable: false),
                    CurrentLongitude = table.Column<double>(type: "float", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    OperatorId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trip_AspNetUsers_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trip_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccurrenceStatus = table.Column<int>(type: "int", nullable: false),
                    OccurrenceDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationDetails = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TripId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripHistory_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ClientId",
                table: "Trip",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ExternalId",
                table: "Trip",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_OperatorId",
                table: "Trip",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TripHistory_ExternalId",
                table: "TripHistory",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripHistory_TripId",
                table: "TripHistory",
                column: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripHistory");

            migrationBuilder.DropTable(
                name: "Trip");
        }
    }
}
