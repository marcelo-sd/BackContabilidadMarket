using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class corregiEstadoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoIdEstado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "EstadoPedidoIdEstado",
                table: "Pedidos",
                newName: "EstadoPedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_EstadoPedidoIdEstado",
                table: "Pedidos",
                newName: "IX_Pedidos_EstadoPedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos",
                column: "EstadoPedidoId",
                principalTable: "EstadoPedidos",
                principalColumn: "IdEstado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "EstadoPedidoId",
                table: "Pedidos",
                newName: "EstadoPedidoIdEstado");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_EstadoPedidoId",
                table: "Pedidos",
                newName: "IX_Pedidos_EstadoPedidoIdEstado");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoIdEstado",
                table: "Pedidos",
                column: "EstadoPedidoIdEstado",
                principalTable: "EstadoPedidos",
                principalColumn: "IdEstado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
