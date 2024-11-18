using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exata.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EntidadeAmostraResultado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AmostraResultado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmostraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoInformacao = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IdAmostraLab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fazenda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Talhao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gleba = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profundidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PontoColeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pHH2O = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pHCaCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pHSMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pmeh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ptotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Na = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    K = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Al = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HplusAl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTCEfetiva = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTCTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    m = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaMg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MgK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaplusMgK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaCTCEfetiva = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MgCTCEfetiva = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaCTCTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MgCTCTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HplusAlT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaplusMgT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaplusMgplusKT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaplusMgplusKplusNaT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Argila = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Silite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreiaTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreiaGrossa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreiaFina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCadastro = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAlteracao = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmostraResultado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmostraResultado_Amostra_AmostraId",
                        column: x => x.AmostraId,
                        principalTable: "Amostra",
                        principalColumn: "AmostraId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmostraResultado_AmostraId",
                table: "AmostraResultado",
                column: "AmostraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmostraResultado");
        }
    }
}
