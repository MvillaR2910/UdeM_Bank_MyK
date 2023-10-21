using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    /// <inheritdoc />
    public partial class createprestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    IdPrestamo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdGrupoAhorro = table.Column<int>(type: "INTEGER", nullable: false),
                    Interes = table.Column<string>(type: "TEXT", nullable: false),
                    MesesAPagar = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.IdPrestamo);
                    table.ForeignKey(
                        name: "FK_Prestamos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prestamos_GruposAhorros_IdGrupoAhorro",
                        column: x => x.IdGrupoAhorro,
                        principalTable: "GruposAhorros",
                        principalColumn: "IdGrupoAhorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdCliente",
                table: "Prestamos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdGrupoAhorro",
                table: "Prestamos",
                column: "IdGrupoAhorro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");
        }
    }
}
