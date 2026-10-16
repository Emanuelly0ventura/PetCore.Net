using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PetCore.Infrastructure.Persistence;

#nullable disable

namespace PetCore.Infrastructure.Migrations;

[DbContext(typeof(PetCoreContext))]
[Migration("20260911200941_MoveHistoricoToPetRelationship")]
public partial class MoveHistoricoToPetRelationship : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "IdPet",
            table: "historico_petcore",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci");

        migrationBuilder.Sql("""
            UPDATE `historico_petcore` AS h
            INNER JOIN `pet_petcore` AS p ON p.`IdHistorico` = h.`Id`
            SET h.`IdPet` = p.`Id`;
            """);

        migrationBuilder.DropForeignKey(
            name: "FK_pet_petcore_historico_petcore_IdHistorico",
            table: "pet_petcore");
        migrationBuilder.DropIndex(name: "IX_pet_petcore_IdHistorico", table: "pet_petcore");
        migrationBuilder.DropColumn(name: "IdHistorico", table: "pet_petcore");

        migrationBuilder.CreateIndex(
            name: "IX_historico_petcore_IdPet",
            table: "historico_petcore",
            column: "IdPet",
            unique: true);
        migrationBuilder.AddForeignKey(
            name: "FK_historico_petcore_pet_petcore_IdPet",
            table: "historico_petcore",
            column: "IdPet",
            principalTable: "pet_petcore",
            principalColumn: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_historico_petcore_pet_petcore_IdPet", table: "historico_petcore");
        migrationBuilder.DropIndex(name: "IX_historico_petcore_IdPet", table: "historico_petcore");
        migrationBuilder.AddColumn<Guid>(
            name: "IdHistorico",
            table: "pet_petcore",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci");

        migrationBuilder.Sql("""
            UPDATE `pet_petcore` AS p
            INNER JOIN `historico_petcore` AS h ON h.`IdPet` = p.`Id`
            SET p.`IdHistorico` = h.`Id`;
            """);

        migrationBuilder.CreateIndex(name: "IX_pet_petcore_IdHistorico", table: "pet_petcore", column: "IdHistorico", unique: true);
        migrationBuilder.AddForeignKey(
            name: "FK_pet_petcore_historico_petcore_IdHistorico",
            table: "pet_petcore",
            column: "IdHistorico",
            principalTable: "historico_petcore",
            principalColumn: "Id");
        migrationBuilder.DropColumn(name: "IdPet", table: "historico_petcore");
    }
}
