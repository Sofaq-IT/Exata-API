using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class TabCamposValoresContato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Contato', 'TelefoneID', 'Telefone', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Contato', 'Nome', 'Nome', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Contato', 'DataCadastro', 'Data Cadastro', 0, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Contato', 'DataAlteracao', 'Data Alteração', 0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Campo WHERE TabelaID = 'Contato'");
        }
    }
}
