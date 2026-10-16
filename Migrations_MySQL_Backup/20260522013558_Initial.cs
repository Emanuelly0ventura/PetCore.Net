using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "endereco_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cep = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco_petcore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "historico_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataAbertura = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_petcore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "medicamento_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Dosagem = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instrucao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicamento_petcore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "medico_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Especialidade = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Telefone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sexo = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlImg = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medico_petcore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tutor_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Telefone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sexo = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlImg = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutor_petcore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinica_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cnpj = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdEndereco = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinica_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clinica_petcore_endereco_petcore_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "endereco_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pet_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Especie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Raca = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNasc = table.Column<DateOnly>(type: "date", nullable: false),
                    Pelagem = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Porte = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UrlImg = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdHistorico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pet_petcore_historico_petcore_IdHistorico",
                        column: x => x.IdHistorico,
                        principalTable: "historico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prontuario_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataEmissao = table.Column<DateOnly>(type: "date", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdHistorico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdMedico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prontuario_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                        column: x => x.IdHistorico,
                        principalTable: "historico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                        column: x => x.IdMedico,
                        principalTable: "medico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "relatorio_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Observacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdHistorico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdMedicoResponsavel = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorio_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                        column: x => x.IdHistorico,
                        principalTable: "historico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relatorio_petcore_medico_petcore_IdMedicoResponsavel",
                        column: x => x.IdMedicoResponsavel,
                        principalTable: "medico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tut_pet_petcore",
                columns: table => new
                {
                    ID_tut_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ID_pet_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tut_pet_petcore", x => new { x.ID_tut_FK, x.ID_pet_FK });
                    table.ForeignKey(
                        name: "FK_tut_pet_petcore_pet_petcore_ID_pet_FK",
                        column: x => x.ID_pet_FK,
                        principalTable: "pet_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tut_pet_petcore_tutor_petcore_ID_tut_FK",
                        column: x => x.ID_tut_FK,
                        principalTable: "tutor_petcore",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exame_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Tipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdMedico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdProntuario = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exame_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exame_petcore_medico_petcore_IdMedico",
                        column: x => x.IdMedico,
                        principalTable: "medico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exame_petcore_prontuario_petcore_IdProntuario",
                        column: x => x.IdProntuario,
                        principalTable: "prontuario_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "receita_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Validade = table.Column<DateOnly>(type: "date", nullable: false),
                    IdMedicoResponsavel = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdProntuario = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receita_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_receita_petcore_medico_petcore_IdMedicoResponsavel",
                        column: x => x.IdMedicoResponsavel,
                        principalTable: "medico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_receita_petcore_prontuario_petcore_IdProntuario",
                        column: x => x.IdProntuario,
                        principalTable: "prontuario_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rel_cli_petcore",
                columns: table => new
                {
                    ID_cli_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ID_rel_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_cli_petcore", x => new { x.ID_cli_FK, x.ID_rel_FK });
                    table.ForeignKey(
                        name: "FK_rel_cli_petcore_clinica_petcore_ID_cli_FK",
                        column: x => x.ID_cli_FK,
                        principalTable: "clinica_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rel_cli_petcore_relatorio_petcore_ID_rel_FK",
                        column: x => x.ID_rel_FK,
                        principalTable: "relatorio_petcore",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rec_medic_petcore",
                columns: table => new
                {
                    ID_medic_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ID_rec_FK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rec_medic_petcore", x => new { x.ID_medic_FK, x.ID_rec_FK });
                    table.ForeignKey(
                        name: "FK_rec_medic_petcore_medicamento_petcore_ID_medic_FK",
                        column: x => x.ID_medic_FK,
                        principalTable: "medicamento_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rec_medic_petcore_receita_petcore_ID_rec_FK",
                        column: x => x.ID_rec_FK,
                        principalTable: "receita_petcore",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_clinica_petcore_IdEndereco",
                table: "clinica_petcore",
                column: "IdEndereco",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exame_petcore_IdMedico",
                table: "exame_petcore",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_exame_petcore_IdProntuario",
                table: "exame_petcore",
                column: "IdProntuario");

            migrationBuilder.CreateIndex(
                name: "IX_pet_petcore_IdHistorico",
                table: "pet_petcore",
                column: "IdHistorico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prontuario_petcore_IdHistorico",
                table: "prontuario_petcore",
                column: "IdHistorico");

            migrationBuilder.CreateIndex(
                name: "IX_prontuario_petcore_IdMedico",
                table: "prontuario_petcore",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_rec_medic_petcore_ID_rec_FK",
                table: "rec_medic_petcore",
                column: "ID_rec_FK");

            migrationBuilder.CreateIndex(
                name: "IX_receita_petcore_IdMedicoResponsavel",
                table: "receita_petcore",
                column: "IdMedicoResponsavel");

            migrationBuilder.CreateIndex(
                name: "IX_receita_petcore_IdProntuario",
                table: "receita_petcore",
                column: "IdProntuario");

            migrationBuilder.CreateIndex(
                name: "IX_rel_cli_petcore_ID_rel_FK",
                table: "rel_cli_petcore",
                column: "ID_rel_FK");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_petcore_IdHistorico",
                table: "relatorio_petcore",
                column: "IdHistorico");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_petcore_IdMedicoResponsavel",
                table: "relatorio_petcore",
                column: "IdMedicoResponsavel");

            migrationBuilder.CreateIndex(
                name: "IX_tut_pet_petcore_ID_pet_FK",
                table: "tut_pet_petcore",
                column: "ID_pet_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exame_petcore");

            migrationBuilder.DropTable(
                name: "rec_medic_petcore");

            migrationBuilder.DropTable(
                name: "rel_cli_petcore");

            migrationBuilder.DropTable(
                name: "tut_pet_petcore");

            migrationBuilder.DropTable(
                name: "medicamento_petcore");

            migrationBuilder.DropTable(
                name: "receita_petcore");

            migrationBuilder.DropTable(
                name: "clinica_petcore");

            migrationBuilder.DropTable(
                name: "relatorio_petcore");

            migrationBuilder.DropTable(
                name: "pet_petcore");

            migrationBuilder.DropTable(
                name: "tutor_petcore");

            migrationBuilder.DropTable(
                name: "prontuario_petcore");

            migrationBuilder.DropTable(
                name: "endereco_petcore");

            migrationBuilder.DropTable(
                name: "historico_petcore");

            migrationBuilder.DropTable(
                name: "medico_petcore");
        }
    }
}
