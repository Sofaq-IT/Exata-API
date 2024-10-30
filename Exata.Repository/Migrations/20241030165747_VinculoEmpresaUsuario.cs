using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class VinculoEmpresaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpresaID",
                table: "AspNetUsers",
                column: "EmpresaID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaID",
                table: "AspNetUsers",
                column: "EmpresaID",
                principalTable: "Empresa",
                principalColumn: "EmpresaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpresaID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpresaID",
                table: "AspNetUsers");
        }
    }
}
