using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    /// <inheritdoc />
    public partial class prestamoestado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Prestamos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Prestamos");
        }
    }
}
