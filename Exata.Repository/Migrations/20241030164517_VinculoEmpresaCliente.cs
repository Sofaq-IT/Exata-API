using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class VinculoEmpresaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpresaCliente",
                columns: table => new
                {
                    EmpresaID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_EmpresaCliente_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID");
                    table.ForeignKey(
                        name: "FK_EmpresaCliente_Empresa_EmpresaID",
                        column: x => x.EmpresaID,
                        principalTable: "Empresa",
                        principalColumn: "EmpresaID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaCliente_ClienteID",
                table: "EmpresaCliente",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaCliente_EmpresaID",
                table: "EmpresaCliente",
                column: "EmpresaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaCliente");
        }
    }
}
