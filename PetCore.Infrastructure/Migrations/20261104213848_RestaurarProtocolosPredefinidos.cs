using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestaurarProtocolosPredefinidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO protocolo_petcore
                    ("Id", "Titulo", "Texto", "TutorId", "PetId", "VeterinarioId",
                     "TutorNome", "PetNome", "VeterinarioNome", "TipoRegistro", "DataEnvio")
                VALUES
                    ('PROTO1', 'Preventivo contra picadas', 'Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.', NULL, NULL, NULL, '', '', '', 'protocolo', NULL),
                    ('PROTO2', 'Vermifugação', 'A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.', NULL, NULL, NULL, '', '', '', 'protocolo', NULL),
                    ('PROTO3', 'FIV e FeLV', 'FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.', NULL, NULL, NULL, '', '', '', 'protocolo', NULL)
                ON CONFLICT ("Id") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM protocolo_petcore
                WHERE "Id" = 'PROTO1'
                  AND "Titulo" = 'Preventivo contra picadas'
                  AND "Texto" = 'Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.'
                  AND "TutorId" IS NULL AND "PetId" IS NULL AND "VeterinarioId" IS NULL
                  AND "TutorNome" = '' AND "PetNome" = '' AND "VeterinarioNome" = ''
                  AND "TipoRegistro" = 'protocolo' AND "DataEnvio" IS NULL;

                DELETE FROM protocolo_petcore
                WHERE "Id" = 'PROTO2'
                  AND "Titulo" = 'Vermifugação'
                  AND "Texto" = 'A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.'
                  AND "TutorId" IS NULL AND "PetId" IS NULL AND "VeterinarioId" IS NULL
                  AND "TutorNome" = '' AND "PetNome" = '' AND "VeterinarioNome" = ''
                  AND "TipoRegistro" = 'protocolo' AND "DataEnvio" IS NULL;

                DELETE FROM protocolo_petcore
                WHERE "Id" = 'PROTO3'
                  AND "Titulo" = 'FIV e FeLV'
                  AND "Texto" = 'FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.'
                  AND "TutorId" IS NULL AND "PetId" IS NULL AND "VeterinarioId" IS NULL
                  AND "TutorNome" = '' AND "PetNome" = '' AND "VeterinarioNome" = ''
                  AND "TipoRegistro" = 'protocolo' AND "DataEnvio" IS NULL;
                """);
        }
    }
}
