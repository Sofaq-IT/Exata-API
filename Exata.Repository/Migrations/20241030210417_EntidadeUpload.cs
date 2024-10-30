using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EntidadeUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Upload",
                columns: table => new
                {
                    UploadID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivoEntrada = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NomeArquivoArmazenado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrlStorage = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    Tamanho = table.Column<long>(type: "bigint", nullable: false),
                    QtdeRegistros = table.Column<int>(type: "int", nullable: false),
                    StatusAtual = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCadastro = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAlteracao = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upload", x => x.UploadID);
                    table.ForeignKey(
                        name: "FK_Upload_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Upload_ClienteId",
                table: "Upload",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Upload");
        }
    }
}
