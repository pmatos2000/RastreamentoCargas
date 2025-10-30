# RastreamentoCargas API

API RESTful desenvolvida em .NET 8 para o teste prático de desenvolvedor back-end. O sistema permite o gerenciamento e rastreamento de cargas (viagens), incluindo clientes, operadores e um histórico detalhado de movimentações.

[![.NET 8](https://img.shields.io/badge/.NET-8-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-red.svg)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-blue.svg)](https://www.docker.com/)
[![JWT](https://img.shields.io/badge/JWT-black.svg)](https://jwt.io)

## 🚀 Funcionalidades Principais

* **Autenticação:** Sistema de login seguro com JSON Web Tokens (JWT).
* **Gerenciamento (CRUD):** Endpoints completos para gerenciamento de Clientes e Operadores (Usuários).
* **Rastreamento de Cargas:**
    * Registro de novas cargas (viagens) com validação de datas e dados.
    * Atualização de status, localização e marcação de entrega.
    * Cancelamento (soft delete) de cargas.
* **Histórico:**
    * Registro automático de histórico para cada mudança de status.
    * Endpoints para consultar o histórico de uma carga específica.
    * Endpoints de admin para consulta paginada de todo o histórico e adição manual.
* **Integração Externa:**
    * Geocodificação de endereços para coordenadas (Latitude/Longitude) usando a API pública do OpenStreetMap (Nominatim).
* **Monitoramento:** Logs detalhados de todas as ações principais usando Serilog.
* **Documentação:** Geração automática de documentação da API com Swagger (OpenAPI).

## 🏛️ Arquitetura

O projeto foi estruturado seguindo os princípios da **Clean Architecture**. Esta abordagem isola as regras de negócio centrais (Domain) de dependências externas (Infrastructure) e da lógica de apresentação (API).

A solução está dividida em quatro projetos principais:

* `RastreamentoCargas.Domain`: Contém as entidades de negócio (ex: `Trip`, `Client`, `Operator`), enums e as interfaces dos repositórios. Não possui dependências de outros projetos da solução.
* `RastreamentoCargas.Application`: Contém a lógica de aplicação (Services), DTOs (Data Transfer Objects) e validadores (FluentValidation). Depende apenas do `Domain`.
* `RastreamentoCargas.Infrastructure`: Contém as implementações das interfaces definidas no `Domain` e `Application`. Inclui os Repositórios (`TripRepository`), o `AppDbContext` do Entity Framework Core, Migrations e serviços externos (ex: `GeocodingService`, `AuthenticationService`).
* `RastreamentoCargas.API`: Ponto de entrada da aplicação. Contém os Controllers, configuração de serviços (`Program.cs`) e `appsettings.json`.

## 🛠️ Tecnologias Utilizadas

* **.NET 8**
* **ASP.NET Core 8** (para a API RESTful)
* **Entity Framework Core 8** (ORM)
* **SQL Server** (Banco de Dados)
* **ASP.NET Core Identity** (Gerenciamento de usuários)
* **JWT Bearer** (Autenticação)
* **Docker & Docker Compose** (Containerização)
* **Swashbuckle (Swagger)** (Documentação da API)
* **Serilog** (Logging)
* **FluentValidation** (Validação de DTOs)

## 🏁 Como Executar (Usando Docker)

A forma mais simples de executar o projeto (API + Banco de Dados) é utilizando o Docker.

### Pré-requisitos

* [Docker](https://www.docker.com/products/docker-desktop/)
* [Docker Compose](https://docs.docker.com/compose/install/)

### Passos para Executar

1.  Clone o repositório:
    ```sh
    git clone <url-do-seu-repositorio>
    cd rastreamentocargas
    ```

2.  Suba os containers da API e do Banco de Dados:
    ```sh
    docker-compose up -d --build
    ```

    * O comando `docker-compose up` irá:
        * Construir a imagem Docker da API (`rastreamento-api`).
        * Subir um container do SQL Server (`rastreamento-sql-server`).
        * A API aplicará automaticamente as migrações do EF Core e populará o banco com dados iniciais (seed data) na inicialização.

3.  Acesse a API:
    * **API (HTTP):** `http://localhost:8080`
    * **Documentação (Swagger):** `http://localhost:8080/swagger`
    * **Banco de Dados (SQL Server):** Acessível em `localhost,1433` (Senha: `Password@123`)

4.  Para parar a aplicação:
    ```sh
    docker-compose down
    ```

## 🔑 Como Usar (Autenticação)

Todos os endpoints (exceto `/api/autenticacao/login`) são protegidos e exigem um Token JWT.

O banco de dados é populado com usuários de teste (via Migrations) para facilitar a demonstração:

| Usuário | Senha | Role | Fonte |
| :--- | :--- | :--- | :--- |
| `adm` | `Teste123.` | `Admin` | [CreateAdminUserAndRole.cs] |
| `operador1` | `Teste123.` | (Padrão) | [MockOperatorsData.cs] |
| `operador2` | `Teste123.` | (Padrão) | [MockOperatorsData.cs] |
| ... (e assim por diante, de `operador1` a `operador10`) | | | |

**Para testar:**

1.  Acesse a documentação: `http://localhost:8080/swagger`
2.  Use o endpoint `POST /api/autenticacao/login` com um dos usuários acima.
3.  Copie o `token` JWT da resposta.
4.  Clique no botão "Authorize" no topo do Swagger e cole o token no formato: `Bearer <seu-token-aqui>`.
5.  Agora você pode testar todos os endpoints protegidos!
