using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class usuario_indice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_usuario_eliminado_activo",
                table: "usuarios",
                columns: new[] { "eliminado", "activo" });

            migrationBuilder.CreateIndex(
                name: "udx_usuario_nombre",
                table: "usuarios",
                column: "usuario_nombre",
                unique: true,
                filter: "[Eliminado] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_usuario_eliminado_activo",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "udx_usuario_nombre",
                table: "usuarios");
        }
    }
}
