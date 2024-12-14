using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class nuevoCampoEnPedidoProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "PedidoProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "PedidoProductos");
        }
    }
}
