using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class TabCamposDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'Login', 'Login', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'Nome', 'Nome', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'Email', 'Email', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'Perfil', 'Perfil', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'DataCadastro', 'Data Cadastro', 0, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Usuario', 'DataAlteracao', 'Data Alteração', 0, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'Descricao', 'Descrição', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'DataCadastro', 'Data Cadastro', 0, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'DataAlteracao', 'Data Alteração', 0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Campo");
        }
    }
}
