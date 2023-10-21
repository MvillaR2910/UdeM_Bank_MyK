using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Saldo = table.Column<float>(type: "REAL", nullable: false),
                    NroGruposPertenecientes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "GruposAhorros",
                columns: table => new
                {
                    IdGrupoAhorro = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreGrupo = table.Column<string>(type: "TEXT", nullable: false),
                    Capital = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposAhorros", x => x.IdGrupoAhorro);
                });

            migrationBuilder.CreateTable(
                name: "ClienteGrupoAhorro",
                columns: table => new
                {
                    GruposDeAhorrosIdGrupoAhorro = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuariosIdCliente = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteGrupoAhorro", x => new { x.GruposDeAhorrosIdGrupoAhorro, x.UsuariosIdCliente });
                    table.ForeignKey(
                        name: "FK_ClienteGrupoAhorro_Clientes_UsuariosIdCliente",
                        column: x => x.UsuariosIdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteGrupoAhorro_GruposAhorros_GruposDeAhorrosIdGrupoAhorro",
                        column: x => x.GruposDeAhorrosIdGrupoAhorro,
                        principalTable: "GruposAhorros",
                        principalColumn: "IdGrupoAhorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrupoAhorroXCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdGrupoAhorro = table.Column<int>(type: "INTEGER", nullable: false),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    AporteCliente = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAhorroXCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoAhorroXCliente_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoAhorroXCliente_GruposAhorros_IdGrupoAhorro",
                        column: x => x.IdGrupoAhorro,
                        principalTable: "GruposAhorros",
                        principalColumn: "IdGrupoAhorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdGrupoAhorro = table.Column<int>(type: "INTEGER", nullable: false),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdGrupoDestinatario = table.Column<int>(type: "INTEGER", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Hora = table.Column<string>(type: "TEXT", nullable: false),
                    Monto = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_Movimientos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movimientos_GruposAhorros_IdGrupoAhorro",
                        column: x => x.IdGrupoAhorro,
                        principalTable: "GruposAhorros",
                        principalColumn: "IdGrupoAhorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteGrupoAhorro_UsuariosIdCliente",
                table: "ClienteGrupoAhorro",
                column: "UsuariosIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAhorroXCliente_IdCliente",
                table: "GrupoAhorroXCliente",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAhorroXCliente_IdGrupoAhorro",
                table: "GrupoAhorroXCliente",
                column: "IdGrupoAhorro");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_IdCliente",
                table: "Movimientos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_IdGrupoAhorro",
                table: "Movimientos",
                column: "IdGrupoAhorro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteGrupoAhorro");

            migrationBuilder.DropTable(
                name: "GrupoAhorroXCliente");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "GruposAhorros");
        }
    }
}
