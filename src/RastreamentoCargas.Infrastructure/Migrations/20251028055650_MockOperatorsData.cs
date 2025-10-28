using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using RastreamentoCargas.Domain.Entities;
using System.Linq; // Necessário para o ToArray()

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    public partial class MockOperatorsData : Migration
    {
        private static readonly (string Id, string UserName, string Email, string FullName, string EmployeeId, string Department)[] UsersToSeed = new (string Id, string UserName, string Email, string FullName, string EmployeeId, string Department)[]
        {
            ("b18be9c0-aa65-4af8-bd17-00bd9344e571", "operador1", "op1@transporte.com", "Alice Logistica", "T001", "Logistica"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e572", "operador2", "op2@transporte.com", "Bob Pátio", "T002", "Pátio"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e573", "operador3", "op3@transporte.com", "Carla Expedição", "T003", "Expedição"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e574", "operador4", "op4@transporte.com", "David Rastreamento", "T004", "Rastreamento"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e575", "operador5", "op5@transporte.com", "Eva Cargas", "T005", "Logistica"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e576", "operador6", "op6@transporte.com", "Felipe Expedição", "T006", "Expedição"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e577", "operador7", "op7@transporte.com", "Gustavo Pátio", "T007", "Pátio"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e578", "operador8", "op8@transporte.com", "Helena Logistica", "T008", "Logistica"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e579", "operador9", "op9@transporte.com", "Igor Rastreamento", "T009", "Rastreamento"),
            ("b18be9c0-aa65-4af8-bd17-00bd9344e57a", "operador10", "op10@transporte.com", "Julia Cargas", "T010", "Logistica")
        };


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var passwordHasher = new PasswordHasher<User>();
            var passwordHash = passwordHasher.HashPassword(null, "Teste123.");

            var createdBy = "SystemMock";
            var createdAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

            foreach (var user in UsersToSeed)
            {
                migrationBuilder.InsertData(
                    table: "AspNetUsers",
                    columns:
                    [
                        "Id", "FullName", "IsActive", "CreatedAt", "CreatedBy", "UserName",
                        "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                        "PasswordHash", "SecurityStamp", "PhoneNumberConfirmed",
                        "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount",
                        "Department", "EmployeeId", "Discriminator", "UpdatedBy", "UpdatedAt"
                    ],
                    values:
                    [
                        user.Id, user.FullName, true, createdAt, createdBy, user.UserName,
                        user.UserName.ToUpper(), user.Email, user.Email.ToUpper(), true,
                        passwordHash, Guid.NewGuid().ToString("D"), false,
                        false, false, 0,
                        user.Department, user.EmployeeId, "Operator", null, DBNull.Value
                    ]
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var userIds = UsersToSeed.Select(u => u.Id).ToArray();

            foreach (var id in userIds)
            {
                migrationBuilder.DeleteData(
                    table: "AspNetUsers",
                    keyColumn: "Id",
                    keyValue: id);
            }
        }
    }
}