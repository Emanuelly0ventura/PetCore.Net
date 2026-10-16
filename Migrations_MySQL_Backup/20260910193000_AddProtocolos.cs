using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PetCore.Infrastructure.Persistence;

#nullable disable

namespace PetCore.Infrastructure.Migrations;

[DbContext(typeof(PetCoreContext))]
[Migration("20260910193000_AddProtocolos")]
public partial class AddProtocolos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "protocolo_petcore",
            columns: table => new
            {
                Id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                Titulo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                Texto = table.Column<string>(type: "longtext", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_protocolo_petcore", x => x.Id))
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "protocolo_petcore",
            columns: new[] { "Id", "Texto", "Titulo" },
            values: new object[,]
            {
                { "PROTO1", "Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.", "Preventivo contra picadas" },
                { "PROTO2", "A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.", "Vermifugação" },
                { "PROTO3", "FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.", "FIV e FeLV" }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "protocolo_petcore");
}
