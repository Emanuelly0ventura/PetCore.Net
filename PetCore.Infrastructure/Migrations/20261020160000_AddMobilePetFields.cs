using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PetCore.Infrastructure.Persistence;

#nullable disable

namespace PetCore.Infrastructure.Migrations;

[DbContext(typeof(PetCoreContext))]
[Migration("20261020160000_AddMobilePetFields")]
public partial class AddMobilePetFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "ObitoInformado", table: "pet_petcore", type: "boolean", nullable: false, defaultValue: false);
        migrationBuilder.AddColumn<string>(
            name: "ComedouroStatus", table: "pet_petcore", type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "vazio");
        migrationBuilder.AlterColumn<Guid>(name: "IdMedico", table: "prontuario_petcore", type: "uuid", nullable: true, oldClrType: typeof(Guid), oldType: "uuid");
        migrationBuilder.AlterColumn<Guid>(name: "IdHistorico", table: "prontuario_petcore", type: "uuid", nullable: true, oldClrType: typeof(Guid), oldType: "uuid");
        migrationBuilder.AddColumn<Guid>(name: "PetId", table: "prontuario_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "TutorId", table: "prontuario_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<decimal>(name: "Temperatura", table: "prontuario_petcore", type: "numeric", nullable: true);
        migrationBuilder.AddColumn<decimal>(name: "Peso", table: "prontuario_petcore", type: "numeric", nullable: true);
        migrationBuilder.AddColumn<string>(name: "Tratamento", table: "prontuario_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "PetNome", table: "prontuario_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "TutorNome", table: "prontuario_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AlterColumn<Guid>(name: "IdProntuario", table: "exame_petcore", type: "uuid", nullable: true, oldClrType: typeof(Guid), oldType: "uuid");
        migrationBuilder.AddColumn<Guid>(name: "PetId", table: "exame_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "TutorId", table: "exame_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<string>(name: "PetNome", table: "exame_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "TutorNome", table: "exame_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "VeterinarioNome", table: "exame_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "ArquivoSolicitacao", table: "exame_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<DateTime>(name: "DataEnvio", table: "exame_petcore", type: "timestamp with time zone", nullable: true);
        migrationBuilder.AlterColumn<Guid>(name: "IdProntuario", table: "receita_petcore", type: "uuid", nullable: true, oldClrType: typeof(Guid), oldType: "uuid");
        migrationBuilder.AlterColumn<DateOnly>(name: "Validade", table: "receita_petcore", type: "date", nullable: true, oldClrType: typeof(DateOnly), oldType: "date");
        migrationBuilder.AddColumn<Guid>(name: "PetId", table: "receita_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "TutorId", table: "receita_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<string>(name: "PetNome", table: "receita_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "TutorNome", table: "receita_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "VeterinarioNome", table: "receita_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "ArquivoReceita", table: "receita_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<DateTime>(name: "DataEnvio", table: "receita_petcore", type: "timestamp with time zone", nullable: true);
        migrationBuilder.AlterColumn<Guid>(name: "IdHistorico", table: "relatorio_petcore", type: "uuid", nullable: true, oldClrType: typeof(Guid), oldType: "uuid");
        migrationBuilder.AddColumn<Guid>(name: "PetId", table: "relatorio_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "TutorId", table: "relatorio_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<string>(name: "PetNome", table: "relatorio_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "TutorNome", table: "relatorio_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "ArquivoHistorico", table: "relatorio_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "Prontuarios", table: "relatorio_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<Guid>(name: "TutorId", table: "protocolo_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "PetId", table: "protocolo_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>(name: "VeterinarioId", table: "protocolo_petcore", type: "uuid", nullable: true);
        migrationBuilder.AddColumn<string>(name: "TutorNome", table: "protocolo_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "PetNome", table: "protocolo_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "VeterinarioNome", table: "protocolo_petcore", type: "text", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>(name: "TipoRegistro", table: "protocolo_petcore", type: "character varying(40)", maxLength: 40, nullable: false, defaultValue: "protocolo");
        migrationBuilder.AddColumn<DateTime>(name: "DataEnvio", table: "protocolo_petcore", type: "timestamp with time zone", nullable: true);
        migrationBuilder.AddColumn<string>(name: "Clinica", table: "medico_petcore", type: "character varying(200)", maxLength: 200, nullable: true);
        migrationBuilder.AddColumn<string>(name: "NomeClinica", table: "medico_petcore", type: "character varying(200)", maxLength: 200, nullable: true);
        migrationBuilder.AddColumn<string>(name: "CnpjClinica", table: "medico_petcore", type: "character varying(20)", maxLength: 20, nullable: true);
        migrationBuilder.AddColumn<string>(name: "CepClinica", table: "medico_petcore", type: "character varying(20)", maxLength: 20, nullable: true);
        migrationBuilder.AddColumn<string>(name: "ComplementoClinica", table: "medico_petcore", type: "character varying(200)", maxLength: 200, nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "ObitoInformado", table: "pet_petcore");
        migrationBuilder.DropColumn(name: "ComedouroStatus", table: "pet_petcore");
        migrationBuilder.DropColumn(name: "PetId", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "TutorId", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "Temperatura", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "Peso", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "Tratamento", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "PetNome", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "TutorNome", table: "prontuario_petcore");
        migrationBuilder.DropColumn(name: "PetId", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "TutorId", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "PetNome", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "TutorNome", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "VeterinarioNome", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "ArquivoSolicitacao", table: "exame_petcore");
        migrationBuilder.DropColumn(name: "DataEnvio", table: "exame_petcore");
        migrationBuilder.AlterColumn<Guid>(name: "IdProntuario", table: "exame_petcore", type: "uuid", nullable: false, oldClrType: typeof(Guid), oldType: "uuid", oldNullable: true);
        migrationBuilder.DropColumn(name: "PetId", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "TutorId", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "PetNome", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "TutorNome", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "VeterinarioNome", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "ArquivoReceita", table: "receita_petcore");
        migrationBuilder.DropColumn(name: "DataEnvio", table: "receita_petcore");
        migrationBuilder.AlterColumn<Guid>(name: "IdProntuario", table: "receita_petcore", type: "uuid", nullable: false, oldClrType: typeof(Guid), oldType: "uuid", oldNullable: true);
        migrationBuilder.AlterColumn<DateOnly>(name: "Validade", table: "receita_petcore", type: "date", nullable: false, oldClrType: typeof(DateOnly), oldType: "date", oldNullable: true);
        migrationBuilder.DropColumn(name: "PetId", table: "relatorio_petcore");
        migrationBuilder.DropColumn(name: "TutorId", table: "relatorio_petcore");
        migrationBuilder.DropColumn(name: "PetNome", table: "relatorio_petcore");
        migrationBuilder.DropColumn(name: "TutorNome", table: "relatorio_petcore");
        migrationBuilder.DropColumn(name: "ArquivoHistorico", table: "relatorio_petcore");
        migrationBuilder.DropColumn(name: "Prontuarios", table: "relatorio_petcore");
        migrationBuilder.AlterColumn<Guid>(name: "IdHistorico", table: "relatorio_petcore", type: "uuid", nullable: false, oldClrType: typeof(Guid), oldType: "uuid", oldNullable: true);
        migrationBuilder.DropColumn(name: "TutorId", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "PetId", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "VeterinarioId", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "TutorNome", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "PetNome", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "VeterinarioNome", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "TipoRegistro", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "DataEnvio", table: "protocolo_petcore");
        migrationBuilder.DropColumn(name: "Clinica", table: "medico_petcore");
        migrationBuilder.DropColumn(name: "NomeClinica", table: "medico_petcore");
        migrationBuilder.DropColumn(name: "CnpjClinica", table: "medico_petcore");
        migrationBuilder.DropColumn(name: "CepClinica", table: "medico_petcore");
        migrationBuilder.DropColumn(name: "ComplementoClinica", table: "medico_petcore");
        migrationBuilder.AlterColumn<Guid>(name: "IdMedico", table: "prontuario_petcore", type: "uuid", nullable: false, oldClrType: typeof(Guid), oldType: "uuid", oldNullable: true);
        migrationBuilder.AlterColumn<Guid>(name: "IdHistorico", table: "prontuario_petcore", type: "uuid", nullable: false, oldClrType: typeof(Guid), oldType: "uuid", oldNullable: true);
    }
}
