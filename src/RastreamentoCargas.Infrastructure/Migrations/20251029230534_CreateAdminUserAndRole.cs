using System;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore.Migrations;
using RastreamentoCargas.Domain.Entities; 

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminUserAndRole : Migration
    {
        private const long AdminRoleId = 1L;
        private const long AdminUserId = 11L;
        private const string AdminUserName = "adm";
        private const string AdminEmail = "adm@transporte.com";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var passwordHasher = new PasswordHasher<User>();
            var passwordHash = passwordHasher.HashPassword(null, "Teste123.");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { AdminRoleId, "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "FullName", "IsActive", "CreatedAt", "CreatedBy", "UserName",
                    "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                    "PasswordHash", "SecurityStamp", "PhoneNumberConfirmed",
                    "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount",
                    "Department", "EmployeeId", "Discriminator", "ExternalId"
                },
                values: new object[]
                {
                    AdminUserId,
                    "Administrador do Sistema", 
                    true,
                    new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc), 
                    "SystemMockData", 
                    AdminUserName, 
                    AdminUserName.ToUpper(), 
                    AdminEmail, 
                    AdminEmail.ToUpper(), 
                    true, 
                    passwordHash, 
                    Guid.NewGuid().ToString("D"), 
                    false, 
                    false, 
                    false, 
                    0,
                    "IT", 
                    "A001", 
                    "Operator", 
                    Guid.NewGuid() 
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { AdminUserId, AdminRoleId }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { AdminUserId, AdminRoleId }
            );

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: AdminUserId
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: AdminRoleId
            );
        }
    }
}