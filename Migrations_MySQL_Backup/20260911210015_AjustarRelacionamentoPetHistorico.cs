using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacionamentoPetHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_historico_petcore_pet_petcore_IdPet",
                table: "historico_petcore");

            migrationBuilder.AddForeignKey(
                name: "FK_historico_petcore_pet_petcore_IdPet",
                table: "historico_petcore",
                column: "IdPet",
                principalTable: "pet_petcore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_historico_petcore_pet_petcore_IdPet",
                table: "historico_petcore");

            migrationBuilder.AddForeignKey(
                name: "FK_historico_petcore_pet_petcore_IdPet",
                table: "historico_petcore",
                column: "IdPet",
                principalTable: "pet_petcore",
                principalColumn: "Id");
        }
    }
}
