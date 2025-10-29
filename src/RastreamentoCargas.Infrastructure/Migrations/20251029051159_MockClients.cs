using System;
using Microsoft.EntityFrameworkCore.Migrations;
using RastreamentoCargas.Domain.Enums; // Para o ClientDocumentType
using System.Reflection;

#nullable disable

namespace RastreamentoCargas.Infrastructure.Migrations
{
    public partial class MockClients : Migration
    {
        private static readonly (long Id, string Name, ClientDocumentType DocumentType, string Document, string Email, string Phone)[] ClientsToSeed = new (long Id, string Name, ClientDocumentType DocumentType, string Document, string Email, string Phone)[]
        {

            (1L, "Transportadora Ágil LTDA", ClientDocumentType.CNPJ, "89137174000104", "contato@agil.com", "551133334444"),
            (2L, "E-commerce Global S.A.", ClientDocumentType.CNPJ, "30465655000104", "logistica@global.com", "552155556666"),
            (3L, "Indústria Metais Alfa", ClientDocumentType.CNPJ, "66297685000198", "cargas@alfa.com", "553177778888"),
            (4L, "Varejo Rápido", ClientDocumentType.CNPJ, "88817909000188", "transp@rapido.com", "551199990000"),
            (5L, "Construtora Forte", ClientDocumentType.CNPJ, "61246040000102", "financeiro@forte.com", "554111112222"),
            
            (6L, "Marina Souza", ClientDocumentType.CPF, "16798985076", "marina.s@cliente.com", "556130303030"),
            (7L, "Ricardo Alves", ClientDocumentType.CPF, "36137006034", "ricardo.a@cliente.com", "5581987654321"),
            (8L, "Patrícia Lima", ClientDocumentType.CPF, "16087121099", "patricia.l@cliente.com", "551988887777"),
            (9L, "Fernando Gomes", ClientDocumentType.CPF, "86700934052", "fernando.g@cliente.com", "555160606060"),
            (10L, "Juliana Santos", ClientDocumentType.CPF, "26978852007", "juliana.s@cliente.com", "551130033003")
        };

        private static readonly string CreatedBySystem = "SystemMockData";
        private static readonly DateTime FixedCreationDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var client in ClientsToSeed)
            {
                migrationBuilder.InsertData(
                    table: "Clients", 
                    columns: new[]
                    {
                        "Id", "Name", "DocumentType", "Document", "ContactEmail", "Phone",
                        "ExternalId", "CreatedAt", "CreatedBy", "IsActive"
                    },
                    values: new object[]
                    {
                        client.Id,
                        client.Name,
                        (int)client.DocumentType, 
                        client.Document,
                        client.Email,
                        client.Phone,
                        Guid.NewGuid(),
                        FixedCreationDate,
                        CreatedBySystem,
                        true 
                    }
                );
            }

            migrationBuilder.Sql("DBCC CHECKIDENT('Clients', RESEED, 0);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var clientIds = ClientsToSeed.Select(c => c.Id).ToArray();

            foreach (var id in clientIds)
            {
                migrationBuilder.DeleteData(
                    table: "Clients",
                    keyColumn: "Id",
                    keyValue: id);
            }
        }
    }
}