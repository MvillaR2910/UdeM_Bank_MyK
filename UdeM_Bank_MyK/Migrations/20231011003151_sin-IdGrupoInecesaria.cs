using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    /// <inheritdoc />
    public partial class sinIdGrupoInecesaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_GruposAhorros_IdGrupoAhorro",
                table: "Movimientos");

            migrationBuilder.DropIndex(
                name: "IX_Movimientos_IdGrupoAhorro",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "IdGrupoAhorro",
                table: "Movimientos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGrupoAhorro",
                table: "Movimientos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_IdGrupoAhorro",
                table: "Movimientos",
                column: "IdGrupoAhorro");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_GruposAhorros_IdGrupoAhorro",
                table: "Movimientos",
                column: "IdGrupoAhorro",
                principalTable: "GruposAhorros",
                principalColumn: "IdGrupoAhorro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
