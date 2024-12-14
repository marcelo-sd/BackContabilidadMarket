using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class anadiForingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Pedidos_PedidoIdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Productos_ProductoIdProductos",
                table: "PedidoProductos");

            migrationBuilder.DropIndex(
                name: "IX_PedidoProductos_PedidoIdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropIndex(
                name: "IX_PedidoProductos_ProductoIdProductos",
                table: "PedidoProductos");

            migrationBuilder.DropColumn(
                name: "PedidoIdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropColumn(
                name: "ProductoIdProductos",
                table: "PedidoProductos");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_IdPedido",
                table: "PedidoProductos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_IdProducto",
                table: "PedidoProductos",
                column: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProductos_Pedidos_IdPedido",
                table: "PedidoProductos",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProductos_Productos_IdProducto",
                table: "PedidoProductos",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProductos",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Pedidos_IdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Productos_IdProducto",
                table: "PedidoProductos");

            migrationBuilder.DropIndex(
                name: "IX_PedidoProductos_IdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropIndex(
                name: "IX_PedidoProductos_IdProducto",
                table: "PedidoProductos");

            migrationBuilder.AddColumn<int>(
                name: "PedidoIdPedido",
                table: "PedidoProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductoIdProductos",
                table: "PedidoProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_PedidoIdPedido",
                table: "PedidoProductos",
                column: "PedidoIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_ProductoIdProductos",
                table: "PedidoProductos",
                column: "ProductoIdProductos");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProductos_Pedidos_PedidoIdPedido",
                table: "PedidoProductos",
                column: "PedidoIdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProductos_Productos_ProductoIdProductos",
                table: "PedidoProductos",
                column: "ProductoIdProductos",
                principalTable: "Productos",
                principalColumn: "IdProductos",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
