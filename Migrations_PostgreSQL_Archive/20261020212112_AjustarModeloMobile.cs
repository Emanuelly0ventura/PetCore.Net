using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustarModeloMobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exame_petcore_prontuario_petcore_IdProntuario",
                table: "exame_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                table: "prontuario_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                table: "prontuario_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_receita_petcore_prontuario_petcore_IdProntuario",
                table: "receita_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                table: "relatorio_petcore");

            migrationBuilder.UpdateData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "Luna", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves", "Dra. Helena Duarte" });

            migrationBuilder.UpdateData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira", "Dr. Rafael Nogueira" });

            migrationBuilder.UpdateData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "Luna", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves" });

            migrationBuilder.UpdateData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira" });

            migrationBuilder.UpdateData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "Luna", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves", "Dra. Helena Duarte" });

            migrationBuilder.UpdateData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira", "Dr. Rafael Nogueira" });

            migrationBuilder.UpdateData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "Luna", new Guid("60000000-0000-0000-0000-000000000001"), "Marina Alves" });

            migrationBuilder.UpdateData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), "Tobias", new Guid("60000000-0000-0000-0000-000000000002"), "Bruno Ferreira" });

            migrationBuilder.AddForeignKey(
                name: "FK_exame_petcore_prontuario_petcore_IdProntuario",
                table: "exame_petcore",
                column: "IdProntuario",
                principalTable: "prontuario_petcore",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                table: "prontuario_petcore",
                column: "IdHistorico",
                principalTable: "historico_petcore",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                table: "prontuario_petcore",
                column: "IdMedico",
                principalTable: "medico_petcore",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_receita_petcore_prontuario_petcore_IdProntuario",
                table: "receita_petcore",
                column: "IdProntuario",
                principalTable: "prontuario_petcore",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                table: "relatorio_petcore",
                column: "IdHistorico",
                principalTable: "historico_petcore",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exame_petcore_prontuario_petcore_IdProntuario",
                table: "exame_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                table: "prontuario_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                table: "prontuario_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_receita_petcore_prontuario_petcore_IdProntuario",
                table: "receita_petcore");

            migrationBuilder.DropForeignKey(
                name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                table: "relatorio_petcore");

            migrationBuilder.UpdateData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { null, "", null, "", "" });

            migrationBuilder.UpdateData(
                table: "exame_petcore",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { null, "", null, "", "" });

            migrationBuilder.UpdateData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { null, "", null, "" });

            migrationBuilder.UpdateData(
                table: "prontuario_petcore",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { null, "", null, "" });

            migrationBuilder.UpdateData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { null, "", null, "", "" });

            migrationBuilder.UpdateData(
                table: "receita_petcore",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome", "VeterinarioNome" },
                values: new object[] { null, "", null, "", "" });

            migrationBuilder.UpdateData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { null, "", null, "" });

            migrationBuilder.UpdateData(
                table: "relatorio_petcore",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"),
                columns: new[] { "PetId", "PetNome", "TutorId", "TutorNome" },
                values: new object[] { null, "", null, "" });

            migrationBuilder.AddForeignKey(
                name: "FK_exame_petcore_prontuario_petcore_IdProntuario",
                table: "exame_petcore",
                column: "IdProntuario",
                principalTable: "prontuario_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prontuario_petcore_historico_petcore_IdHistorico",
                table: "prontuario_petcore",
                column: "IdHistorico",
                principalTable: "historico_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prontuario_petcore_medico_petcore_IdMedico",
                table: "prontuario_petcore",
                column: "IdMedico",
                principalTable: "medico_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_receita_petcore_prontuario_petcore_IdProntuario",
                table: "receita_petcore",
                column: "IdProntuario",
                principalTable: "prontuario_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_relatorio_petcore_historico_petcore_IdHistorico",
                table: "relatorio_petcore",
                column: "IdHistorico",
                principalTable: "historico_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
