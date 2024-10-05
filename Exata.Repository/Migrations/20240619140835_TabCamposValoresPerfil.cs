﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class TabCamposValoresPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'Descricao', 'Descrição', 1, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'DataCadastro', 'Data Cadastro', 0, 1)");
            migrationBuilder.Sql("INSERT INTO Campo VALUES ('Perfil', 'DataAlteracao', 'Data Alteração', 0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Campo WHERE TabelaID = 'Perfil'");
        }
    }
}
