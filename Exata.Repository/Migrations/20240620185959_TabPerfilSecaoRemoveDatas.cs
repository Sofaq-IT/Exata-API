using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class TabPerfilSecaoRemoveDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAlteracao",
                table: "PerfilSecao");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "PerfilSecao");

            migrationBuilder.DropColumn(
                name: "UserAlteracao",
                table: "PerfilSecao");

            migrationBuilder.DropColumn(
                name: "UserCadastro",
                table: "PerfilSecao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAlteracao",
                table: "PerfilSecao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "PerfilSecao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserAlteracao",
                table: "PerfilSecao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserCadastro",
                table: "PerfilSecao",
                type: "int",
                nullable: true);
        }
    }
}
