using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class VinculoClienteUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClienteID",
                table: "AspNetUsers",
                column: "ClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cliente_ClienteID",
                table: "AspNetUsers",
                column: "ClienteID",
                principalTable: "Cliente",
                principalColumn: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cliente_ClienteID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClienteID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClienteID",
                table: "AspNetUsers");
        }
    }
}
