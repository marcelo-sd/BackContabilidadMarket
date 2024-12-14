using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    /// <inheritdoc />
    public partial class corregiDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Productos",
                newName: "Precioo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precioo",
                table: "Productos",
                newName: "Precio");
        }
    }
}
