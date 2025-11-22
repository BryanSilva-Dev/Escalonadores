using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Escalonadores.Migrations
{
    /// <inheritdoc />
    public partial class migracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "escalonador",
                columns: table => new
                {
                    id_escalonador = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_escalonador = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escalonador", x => x.id_escalonador);
                });

            migrationBuilder.CreateTable(
                name: "escalonador_execucao",
                columns: table => new
                {
                    id_escalonador_execucao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_execucao = table.Column<long>(type: "bigint", nullable: false),
                    id_paciente = table.Column<long>(type: "bigint", nullable: false),
                    contador_medico = table.Column<long>(type: "bigint", nullable: false),
                    inicio = table.Column<long>(type: "bigint", nullable: false),
                    fim = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escalonador_execucao", x => x.id_escalonador_execucao);
                });

            migrationBuilder.CreateTable(
                name: "execucao",
                columns: table => new
                {
                    id_execucao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_algoritmo = table.Column<long>(type: "bigint", nullable: false),
                    n_medicos = table.Column<long>(type: "bigint", nullable: false),
                    n_trocas_contexto = table.Column<long>(type: "bigint", nullable: false),
                    media_espera = table.Column<double>(type: "double precision", nullable: false),
                    media_execucao = table.Column<double>(type: "double precision", nullable: false),
                    media_cpu = table.Column<double>(type: "double precision", nullable: false),
                    data_execucao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execucao", x => x.id_execucao);
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    id_paciente = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_execucao = table.Column<long>(type: "bigint", nullable: false),
                    tempo_chegada = table.Column<long>(type: "bigint", nullable: false),
                    tempo_saida = table.Column<long>(type: "bigint", nullable: false),
                    duracao = table.Column<long>(type: "bigint", nullable: false),
                    id_prioridade_manchester = table.Column<long>(type: "bigint", nullable: false),
                    quantum = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paciente", x => x.id_paciente);
                });

            migrationBuilder.CreateTable(
                name: "prioridade_manchester",
                columns: table => new
                {
                    id_prioridade_manchester = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_prioridade = table.Column<string>(type: "text", nullable: true),
                    cor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prioridade_manchester", x => x.id_prioridade_manchester);
                });

            migrationBuilder.InsertData(
                table: "escalonador",
                columns: new[] { "id_escalonador", "nome_escalonador" },
                values: new object[,]
                {
                    { 1L, "Round Robin" },
                    { 2L, "Shortest Job First" },
                    { 3L, "Shortest Remaining Time First" },
                    { 4L, "Prioridade Não-Preemptivo" }
                });

            migrationBuilder.InsertData(
                table: "prioridade_manchester",
                columns: new[] { "id_prioridade_manchester", "cor", "nome_prioridade" },
                values: new object[,]
                {
                    { 1L, "Vermelho", "Emergência" },
                    { 2L, "Laranja", "Muito Urgente" },
                    { 3L, "Amarelo", "Urgente" },
                    { 4L, "Verde", "Pouco Urgente" },
                    { 5L, "Azul", "Não Urgente" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "escalonador");

            migrationBuilder.DropTable(
                name: "escalonador_execucao");

            migrationBuilder.DropTable(
                name: "execucao");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "prioridade_manchester");
        }
    }
}
