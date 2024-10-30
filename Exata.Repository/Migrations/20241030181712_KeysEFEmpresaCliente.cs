using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class KeysEFEmpresaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmpresaCliente_EmpresaID",
                table: "EmpresaCliente");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmpresaCliente",
                table: "EmpresaCliente",
                columns: new[] { "EmpresaID", "ClienteID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmpresaCliente",
                table: "EmpresaCliente");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaCliente_EmpresaID",
                table: "EmpresaCliente",
                column: "EmpresaID");
        }
    }
}
