using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class corregidbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProducto_Pedidos_PedidoIdPedido",
                table: "PedidoProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProducto_Productos_ProductoIdProductos",
                table: "PedidoProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoProducto",
                table: "PedidoProducto");

            migrationBuilder.RenameTable(
                name: "PedidoProducto",
                newName: "PedidoProductos");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoProducto_ProductoIdProductos",
                table: "PedidoProductos",
                newName: "IX_PedidoProductos_ProductoIdProductos");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoProducto_PedidoIdPedido",
                table: "PedidoProductos",
                newName: "IX_PedidoProductos_PedidoIdPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoProductos",
                table: "PedidoProductos",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Pedidos_PedidoIdPedido",
                table: "PedidoProductos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProductos_Productos_ProductoIdProductos",
                table: "PedidoProductos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoProductos",
                table: "PedidoProductos");

            migrationBuilder.RenameTable(
                name: "PedidoProductos",
                newName: "PedidoProducto");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoProductos_ProductoIdProductos",
                table: "PedidoProducto",
                newName: "IX_PedidoProducto_ProductoIdProductos");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoProductos_PedidoIdPedido",
                table: "PedidoProducto",
                newName: "IX_PedidoProducto_PedidoIdPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoProducto",
                table: "PedidoProducto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProducto_Pedidos_PedidoIdPedido",
                table: "PedidoProducto",
                column: "PedidoIdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProducto_Productos_ProductoIdProductos",
                table: "PedidoProducto",
                column: "ProductoIdProductos",
                principalTable: "Productos",
                principalColumn: "IdProductos",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
