using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EntidadeAmostra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Cliente_ClienteId",
                table: "Upload");

            migrationBuilder.DropIndex(
                name: "IX_Upload_ClienteId",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Upload");

            migrationBuilder.AddColumn<Guid>(
                name: "AmostraId",
                table: "Upload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Amostra",
                columns: table => new
                {
                    AmostraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCadastro = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAlteracao = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amostra", x => x.AmostraId);
                    table.ForeignKey(
                        name: "FK_Amostra_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Upload_AmostraId",
                table: "Upload",
                column: "AmostraId");

            migrationBuilder.CreateIndex(
                name: "IX_Amostra_ClienteId",
                table: "Amostra",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Amostra_AmostraId",
                table: "Upload",
                column: "AmostraId",
                principalTable: "Amostra",
                principalColumn: "AmostraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Amostra_AmostraId",
                table: "Upload");

            migrationBuilder.DropTable(
                name: "Amostra");

            migrationBuilder.DropIndex(
                name: "IX_Upload_AmostraId",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "AmostraId",
                table: "Upload");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Upload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Upload_ClienteId",
                table: "Upload",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Cliente_ClienteId",
                table: "Upload",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "ClienteID");
        }
    }
}
