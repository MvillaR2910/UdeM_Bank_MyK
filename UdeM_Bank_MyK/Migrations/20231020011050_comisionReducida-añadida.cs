using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    /// <inheritdoc />
    public partial class comisionReducidaañadida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ComisionReducida",
                table: "Clientes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComisionReducida",
                table: "Clientes");
        }
    }
}
