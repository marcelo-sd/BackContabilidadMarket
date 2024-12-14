using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class addNewTablePedidoProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Productos_idProducto",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_idProducto",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "idProducto",
                table: "Pedidos");

            migrationBuilder.AddColumn<double>(
                name: "Precio",
                table: "Productos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "PedidoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    PedidoIdPedido = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    ProductoIdProductos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoProducto_Pedidos_PedidoIdPedido",
                        column: x => x.PedidoIdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoProducto_Productos_ProductoIdProductos",
                        column: x => x.ProductoIdProductos,
                        principalTable: "Productos",
                        principalColumn: "IdProductos",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProducto_PedidoIdPedido",
                table: "PedidoProducto",
                column: "PedidoIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProducto_ProductoIdProductos",
                table: "PedidoProducto",
                column: "ProductoIdProductos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoProducto");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "idProducto",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_idProducto",
                table: "Pedidos",
                column: "idProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Productos_idProducto",
                table: "Pedidos",
                column: "idProducto",
                principalTable: "Productos",
                principalColumn: "IdProductos",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
