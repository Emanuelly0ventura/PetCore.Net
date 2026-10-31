using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "endereco_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medicamento_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Dosagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instrucao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicamento_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medico_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Especialidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clinica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NomeClinica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CnpjClinica = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CepClinica = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ComplementoClinica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medico_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pet_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Raca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNasc = table.Column<DateOnly>(type: "date", nullable: false),
                    Pelagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Porte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ObitoInformado = table.Column<bool>(type: "bit", nullable: false),
                    ComedouroStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UrlImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "protocolo_petcore",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeterinarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VeterinarioNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoRegistro = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_protocolo_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tutor_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutor_petcore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clinica_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEndereco = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "historico_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAbertura = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IdPet = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historico_petcore_pet_petcore_IdPet",
                        column: x => x.IdPet,
                        principalTable: "pet_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tut_pet_petcore",
                columns: table => new
                {
                    ID_tut_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_pet_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "prontuario_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataEmissao = table.Column<DateOnly>(type: "date", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdHistorico = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdMedico = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperatura = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tratamento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prontuario_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                        column: x => x.IdHistorico,
                        principalTable: "historico_petcore",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                        column: x => x.IdMedico,
                        principalTable: "medico_petcore",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "relatorio_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdHistorico = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdMedicoResponsavel = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArquivoHistorico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prontuarios = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorio_petcore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                        column: x => x.IdHistorico,
                        principalTable: "historico_petcore",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_relatorio_petcore_medico_petcore_IdMedicoResponsavel",
                        column: x => x.IdMedicoResponsavel,
                        principalTable: "medico_petcore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exame_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdMedico = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProntuario = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeterinarioNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArquivoSolicitacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "receita_petcore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Validade = table.Column<DateOnly>(type: "date", nullable: true),
                    IdMedicoResponsavel = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProntuario = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PetNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeterinarioNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArquivoReceita = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "rel_cli_petcore",
                columns: table => new
                {
                    ID_cli_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_rel_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "rec_medic_petcore",
                columns: table => new
                {
                    ID_medic_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_rec_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.InsertData(
                table: "endereco_petcore",
                columns: new[] { "Id", "Cep", "Complemento" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "01001-000", "Sala 12 - dados fictícios" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "01415-002", "Bloco B - dados fictícios" }
                });

            migrationBuilder.InsertData(
                table: "medicamento_petcore",
                columns: new[] { "Id", "Dosagem", "Instrucao", "Nome" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), "1 comprimido a cada 24 horas", "Administrar após a alimentação por 5 dias.", "Dermocalm Vet (fictício)" },
                    { new Guid("90000000-0000-0000-0000-000000000002"), "1/2 comprimido a cada 12 horas", "Administrar conforme prescrição veterinária.", "CardioPet (fictício)" }
                });

            migrationBuilder.InsertData(
                table: "medico_petcore",
                columns: new[] { "Id", "CepClinica", "Clinica", "CnpjClinica", "ComplementoClinica", "DataNascimento", "Email", "Especialidade", "Nome", "NomeClinica", "Senha", "Sexo", "Telefone", "UrlImg" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), null, null, null, null, new DateOnly(1985, 8, 14), "helena.duarte@exemplo.test", "Dermatologia", "Dra. Helena Duarte", null, "SenhaDemo#2026", "F", "11987654321", "https://exemplo.test/medicos/helena.jpg" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), null, null, null, null, new DateOnly(1982, 2, 20), "rafael.nogueira@exemplo.test", "Cardiologia", "Dr. Rafael Nogueira", null, "SenhaDemo#2026", "M", "11976543210", "https://exemplo.test/medicos/rafael.jpg" }
                });

            migrationBuilder.InsertData(
                table: "pet_petcore",
                columns: new[] { "Id", "ComedouroStatus", "DataNasc", "Especie", "Nome", "ObitoInformado", "Pelagem", "Porte", "Raca", "Sexo", "Status", "UrlImg" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "vazio", new DateOnly(2021, 6, 18), "Canina", "Luna", false, "Dourada", "Grande", "Golden Retriever", 0, true, "https://exemplo.test/pets/luna.jpg" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "vazio", new DateOnly(2022, 9, 5), "Felina", "Tobias", false, "Creme e marrom", "Pequeno", "Siamês", 1, true, "https://exemplo.test/pets/tobias.jpg" }
                });

            migrationBuilder.InsertData(
                table: "protocolo_petcore",
                columns: new[] { "Id", "DataEnvio", "PetId", "PetNome", "Texto", "TipoRegistro", "Titulo", "TutorId", "TutorNome", "VeterinarioId", "VeterinarioNome" },
                values: new object[,]
                {
                    { "PROTO1", null, null, "", "Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.", "protocolo", "Preventivo contra picadas", null, "", null, "" },
                    { "PROTO2", null, null, "", "A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.", "protocolo", "Vermifugação", null, "", null, "" },
                    { "PROTO3", null, null, "", "FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.", "protocolo", "FIV e FeLV", null, "", null, "" }
                });

            migrationBuilder.InsertData(
                table: "tutor_petcore",
                columns: new[] { "Id", "DataNascimento", "Email", "Nome", "Senha", "Sexo", "Telefone", "UrlImg" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateOnly(1991, 11, 3), "marina.alves@exemplo.test", "Marina Alves", "SenhaDemo#2026", "F", "11991234567", "https://exemplo.test/tutores/marina.jpg" },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateOnly(1988, 5, 25), "bruno.ferreira@exemplo.test", "Bruno Ferreira", "SenhaDemo#2026", "M", "11992345678", "https://exemplo.test/tutores/bruno.jpg" }
                });

            migrationBuilder.InsertData(
                table: "clinica_petcore",
                columns: new[] { "Id", "Cnpj", "IdEndereco", "Nome" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "12.345.678/0001-90", new Guid("10000000-0000-0000-0000-000000000001"), "PetCore Clínica Centro (fictícia)" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "98.765.432/0001-10", new Guid("10000000-0000-0000-0000-000000000002"), "PetCore Vet Jardins (fictícia)" }
                });

            migrationBuilder.InsertData(
                table: "historico_petcore",
                columns: new[] { "Id", "DataAbertura", "IdPet", "Status" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateOnly(2025, 3, 12), new Guid("50000000-0000-0000-0000-000000000001"), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateOnly(2025, 4, 8), new Guid("50000000-0000-0000-0000-000000000002"), true }
                });

            migrationBuilder.InsertData(
                table: "tut_pet_petcore",
                columns: new[] { "ID_pet_FK", "ID_tut_FK" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "prontuario_petcore",
                columns: new[] { "Id", "DataEmissao", "Descricao", "IdHistorico", "IdMedico", "Peso", "PetId", "PetNome", "Temperatura", "Tratamento", "TutorId", "TutorNome" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateOnly(2025, 5, 10), "Consulta preventiva; pele sem alterações relevantes.", new Guid("30000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), null, new Guid("50000000-0000-0000-0000-000000000001"), "Luna", null, "", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves" },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateOnly(2025, 6, 2), "Avaliação de rotina; ausculta cardíaca normal.", new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002"), null, new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", null, "", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira" }
                });

            migrationBuilder.InsertData(
                table: "relatorio_petcore",
                columns: new[] { "Id", "ArquivoHistorico", "IdHistorico", "IdMedicoResponsavel", "Observacao", "PetId", "PetNome", "Prontuarios", "TutorId", "TutorNome" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "", new Guid("30000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), "Retorno sugerido em seis meses para acompanhamento preventivo.", new Guid("50000000-0000-0000-0000-000000000001"), "Luna", "", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves" },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "", new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002"), "Manter rotina de hidratação e retorno anual.", new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", "", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira" }
                });

            migrationBuilder.InsertData(
                table: "exame_petcore",
                columns: new[] { "Id", "ArquivoSolicitacao", "Data", "DataEnvio", "IdMedico", "IdProntuario", "Nome", "PetId", "PetNome", "Tipo", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), "", new DateOnly(2025, 5, 10), null, new Guid("40000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001"), "Hemograma da Luna", new Guid("50000000-0000-0000-0000-000000000001"), "Luna", "Hemograma completo", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves", "Dra. Helena Duarte" },
                    { new Guid("80000000-0000-0000-0000-000000000002"), "", new DateOnly(2025, 6, 2), null, new Guid("40000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000002"), "Ultrassom do Tobias", new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", "Ultrassom abdominal", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira", "Dr. Rafael Nogueira" }
                });

            migrationBuilder.InsertData(
                table: "receita_petcore",
                columns: new[] { "Id", "ArquivoReceita", "DataEnvio", "IdMedicoResponsavel", "IdProntuario", "Nome", "PetId", "PetNome", "TutorId", "TutorNome", "Validade", "VeterinarioNome" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "", null, new Guid("40000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001"), "Receita dermatológica - Luna", new Guid("50000000-0000-0000-0000-000000000001"), "Luna", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves", new DateOnly(2025, 12, 10), "Dra. Helena Duarte" },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "", null, new Guid("40000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000002"), "Receita preventiva - Tobias", new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira", new DateOnly(2025, 12, 2), "Dr. Rafael Nogueira" }
                });

            migrationBuilder.InsertData(
                table: "rel_cli_petcore",
                columns: new[] { "ID_cli_FK", "ID_rel_FK" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "rec_medic_petcore",
                columns: new[] { "ID_medic_FK", "ID_rec_FK" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000002") }
                });

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
                name: "IX_historico_petcore_IdPet",
                table: "historico_petcore",
                column: "IdPet",
                unique: true,
                filter: "[IdPet] IS NOT NULL");

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
                name: "protocolo_petcore");

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
                name: "tutor_petcore");

            migrationBuilder.DropTable(
                name: "prontuario_petcore");

            migrationBuilder.DropTable(
                name: "endereco_petcore");

            migrationBuilder.DropTable(
                name: "historico_petcore");

            migrationBuilder.DropTable(
                name: "medico_petcore");

            migrationBuilder.DropTable(
                name: "pet_petcore");
        }
    }
}
