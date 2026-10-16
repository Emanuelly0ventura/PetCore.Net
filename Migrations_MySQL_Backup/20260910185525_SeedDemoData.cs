using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "endereco_petcore",
                columns: new[] { "Id", "Cep", "Complemento" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "01001-000", "Sala 12 - dados fictícios" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "01415-002", "Bloco B - dados fictícios" }
                });

            migrationBuilder.InsertData(
                table: "historico_petcore",
                columns: new[] { "Id", "DataAbertura", "Status" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateOnly(2025, 3, 12), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateOnly(2025, 4, 8), true }
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
                columns: new[] { "Id", "DataNascimento", "Email", "Especialidade", "Nome", "Senha", "Sexo", "Telefone", "UrlImg" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateOnly(1985, 8, 14), "helena.duarte@exemplo.test", "Dermatologia", "Dra. Helena Duarte", "SenhaDemo#2026", "F", "11987654321", "https://exemplo.test/medicos/helena.jpg" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateOnly(1982, 2, 20), "rafael.nogueira@exemplo.test", "Cardiologia", "Dr. Rafael Nogueira", "SenhaDemo#2026", "M", "11976543210", "https://exemplo.test/medicos/rafael.jpg" }
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
                table: "pet_petcore",
                columns: new[] { "Id", "DataNasc", "Especie", "IdHistorico", "Nome", "Pelagem", "Porte", "Raca", "Sexo", "Status", "UrlImg" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateOnly(2021, 6, 18), "Canina", new Guid("30000000-0000-0000-0000-000000000001"), "Luna", "Dourada", "Grande", "Golden Retriever", 0, true, "https://exemplo.test/pets/luna.jpg" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new DateOnly(2022, 9, 5), "Felina", new Guid("30000000-0000-0000-0000-000000000002"), "Tobias", "Creme e marrom", "Pequeno", "Siamês", 1, true, "https://exemplo.test/pets/tobias.jpg" }
                });

            migrationBuilder.InsertData(
                table: "prontuario_petcore",
                columns: new[] { "Id", "DataEmissao", "Descricao", "IdHistorico", "IdMedico" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateOnly(2025, 5, 10), "Consulta preventiva; pele sem alterações relevantes.", new Guid("30000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateOnly(2025, 6, 2), "Avaliação de rotina; ausculta cardíaca normal.", new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "relatorio_petcore",
                columns: new[] { "Id", "IdHistorico", "IdMedicoResponsavel", "Observacao" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), "Retorno sugerido em seis meses para acompanhamento preventivo." },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002"), "Manter rotina de hidratação e retorno anual." }
                });

            migrationBuilder.InsertData(
                table: "exame_petcore",
                columns: new[] { "Id", "Data", "IdMedico", "IdProntuario", "Nome", "Tipo" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), new DateOnly(2025, 5, 10), new Guid("40000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001"), "Hemograma da Luna", "Hemograma completo" },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new DateOnly(2025, 6, 2), new Guid("40000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000002"), "Ultrassom do Tobias", "Ultrassom abdominal" }
                });

            migrationBuilder.InsertData(
                table: "receita_petcore",
                columns: new[] { "Id", "IdMedicoResponsavel", "IdProntuario", "Nome", "Validade" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001"), "Receita dermatológica - Luna", new DateOnly(2025, 12, 10) },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000002"), "Receita preventiva - Tobias", new DateOnly(2025, 12, 2) }
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
                table: "tut_pet_petcore",
                columns: new[] { "ID_pet_FK", "ID_tut_FK" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "rec_medic_petcore",
                columns: new[] { "ID_medic_FK", "ID_rec_FK" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000002") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "rec_medic_petcore",
                keyColumns: new[] { "ID_medic_FK", "ID_rec_FK" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "rec_medic_petcore",
                keyColumns: new[] { "ID_medic_FK", "ID_rec_FK" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "rel_cli_petcore",
                keyColumns: new[] { "ID_cli_FK", "ID_rel_FK" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "rel_cli_petcore",
                keyColumns: new[] { "ID_cli_FK", "ID_rel_FK" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "tut_pet_petcore",
                keyColumns: new[] { "ID_pet_FK", "ID_tut_FK" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "tut_pet_petcore",
                keyColumns: new[] { "ID_pet_FK", "ID_tut_FK" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "clinica_petcore",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "clinica_petcore",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "medicamento_petcore",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "medicamento_petcore",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "pet_petcore",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "pet_petcore",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "tutor_petcore",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "tutor_petcore",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "endereco_petcore",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "endereco_petcore",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "historico_petcore",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "historico_petcore",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "medico_petcore",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "medico_petcore",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));
        }
    }
}
