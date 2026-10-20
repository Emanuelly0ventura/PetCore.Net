CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE endereco_petcore (
    "Id" uuid NOT NULL,
    "Cep" text NOT NULL,
    "Complemento" text NOT NULL,
    CONSTRAINT "PK_endereco_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE medicamento_petcore (
    "Id" uuid NOT NULL,
    "Dosagem" text NOT NULL,
    "Instrucao" text NOT NULL,
    "Nome" text NOT NULL,
    CONSTRAINT "PK_medicamento_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE medico_petcore (
    "Id" uuid NOT NULL,
    "Especialidade" text NOT NULL,
    "Nome" text NOT NULL,
    "DataNascimento" date NOT NULL,
    "Telefone" text NOT NULL,
    "Email" text NOT NULL,
    "Sexo" character varying(1) NOT NULL,
    "Senha" text NOT NULL,
    "UrlImg" text NOT NULL,
    CONSTRAINT "PK_medico_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE pet_petcore (
    "Id" uuid NOT NULL,
    "Nome" text NOT NULL,
    "Especie" text NOT NULL,
    "Raca" text NOT NULL,
    "DataNasc" date NOT NULL,
    "Pelagem" text NOT NULL,
    "Porte" text NOT NULL,
    "Sexo" integer NOT NULL,
    "Status" boolean NOT NULL,
    "UrlImg" text NOT NULL,
    CONSTRAINT "PK_pet_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE protocolo_petcore (
    "Id" character varying(20) NOT NULL,
    "Titulo" character varying(150) NOT NULL,
    "Texto" text NOT NULL,
    CONSTRAINT "PK_protocolo_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE tutor_petcore (
    "Id" uuid NOT NULL,
    "Nome" text NOT NULL,
    "DataNascimento" date NOT NULL,
    "Telefone" text NOT NULL,
    "Email" text NOT NULL,
    "Sexo" character varying(1) NOT NULL,
    "Senha" text NOT NULL,
    "UrlImg" text NOT NULL,
    CONSTRAINT "PK_tutor_petcore" PRIMARY KEY ("Id")
);

CREATE TABLE clinica_petcore (
    "Id" uuid NOT NULL,
    "Nome" text NOT NULL,
    "Cnpj" text NOT NULL,
    "IdEndereco" uuid NOT NULL,
    CONSTRAINT "PK_clinica_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_clinica_petcore_endereco_petcore_IdEndereco" FOREIGN KEY ("IdEndereco") REFERENCES endereco_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE historico_petcore (
    "Id" uuid NOT NULL,
    "DataAbertura" date NOT NULL,
    "Status" boolean NOT NULL,
    "IdPet" uuid,
    CONSTRAINT "PK_historico_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_historico_petcore_pet_petcore_IdPet" FOREIGN KEY ("IdPet") REFERENCES pet_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE tut_pet_petcore (
    "ID_tut_FK" uuid NOT NULL,
    "ID_pet_FK" uuid NOT NULL,
    CONSTRAINT "PK_tut_pet_petcore" PRIMARY KEY ("ID_tut_FK", "ID_pet_FK"),
    CONSTRAINT "FK_tut_pet_petcore_pet_petcore_ID_pet_FK" FOREIGN KEY ("ID_pet_FK") REFERENCES pet_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_tut_pet_petcore_tutor_petcore_ID_tut_FK" FOREIGN KEY ("ID_tut_FK") REFERENCES tutor_petcore ("Id")
);

CREATE TABLE prontuario_petcore (
    "Id" uuid NOT NULL,
    "DataEmissao" date NOT NULL,
    "Descricao" text NOT NULL,
    "IdHistorico" uuid NOT NULL,
    "IdMedico" uuid NOT NULL,
    CONSTRAINT "PK_prontuario_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_prontuario_petcore_historico_petcore_IdHistorico" FOREIGN KEY ("IdHistorico") REFERENCES historico_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_prontuario_petcore_medico_petcore_IdMedico" FOREIGN KEY ("IdMedico") REFERENCES medico_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE relatorio_petcore (
    "Id" uuid NOT NULL,
    "Observacao" text NOT NULL,
    "IdHistorico" uuid NOT NULL,
    "IdMedicoResponsavel" uuid NOT NULL,
    CONSTRAINT "PK_relatorio_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_relatorio_petcore_historico_petcore_IdHistorico" FOREIGN KEY ("IdHistorico") REFERENCES historico_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_relatorio_petcore_medico_petcore_IdMedicoResponsavel" FOREIGN KEY ("IdMedicoResponsavel") REFERENCES medico_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE exame_petcore (
    "Id" uuid NOT NULL,
    "Data" date NOT NULL,
    "Tipo" text NOT NULL,
    "IdMedico" uuid NOT NULL,
    "IdProntuario" uuid NOT NULL,
    "Nome" text NOT NULL,
    CONSTRAINT "PK_exame_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_exame_petcore_medico_petcore_IdMedico" FOREIGN KEY ("IdMedico") REFERENCES medico_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_exame_petcore_prontuario_petcore_IdProntuario" FOREIGN KEY ("IdProntuario") REFERENCES prontuario_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE receita_petcore (
    "Id" uuid NOT NULL,
    "Validade" date NOT NULL,
    "IdMedicoResponsavel" uuid NOT NULL,
    "IdProntuario" uuid NOT NULL,
    "Nome" text NOT NULL,
    CONSTRAINT "PK_receita_petcore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_receita_petcore_medico_petcore_IdMedicoResponsavel" FOREIGN KEY ("IdMedicoResponsavel") REFERENCES medico_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_receita_petcore_prontuario_petcore_IdProntuario" FOREIGN KEY ("IdProntuario") REFERENCES prontuario_petcore ("Id") ON DELETE CASCADE
);

CREATE TABLE rel_cli_petcore (
    "ID_cli_FK" uuid NOT NULL,
    "ID_rel_FK" uuid NOT NULL,
    CONSTRAINT "PK_rel_cli_petcore" PRIMARY KEY ("ID_cli_FK", "ID_rel_FK"),
    CONSTRAINT "FK_rel_cli_petcore_clinica_petcore_ID_cli_FK" FOREIGN KEY ("ID_cli_FK") REFERENCES clinica_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_rel_cli_petcore_relatorio_petcore_ID_rel_FK" FOREIGN KEY ("ID_rel_FK") REFERENCES relatorio_petcore ("Id")
);

CREATE TABLE rec_medic_petcore (
    "ID_medic_FK" uuid NOT NULL,
    "ID_rec_FK" uuid NOT NULL,
    CONSTRAINT "PK_rec_medic_petcore" PRIMARY KEY ("ID_medic_FK", "ID_rec_FK"),
    CONSTRAINT "FK_rec_medic_petcore_medicamento_petcore_ID_medic_FK" FOREIGN KEY ("ID_medic_FK") REFERENCES medicamento_petcore ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_rec_medic_petcore_receita_petcore_ID_rec_FK" FOREIGN KEY ("ID_rec_FK") REFERENCES receita_petcore ("Id")
);

INSERT INTO endereco_petcore ("Id", "Cep", "Complemento")
VALUES ('10000000-0000-0000-0000-000000000001', '01001-000', 'Sala 12 - dados fictícios');
INSERT INTO endereco_petcore ("Id", "Cep", "Complemento")
VALUES ('10000000-0000-0000-0000-000000000002', '01415-002', 'Bloco B - dados fictícios');

INSERT INTO medicamento_petcore ("Id", "Dosagem", "Instrucao", "Nome")
VALUES ('90000000-0000-0000-0000-000000000001', '1 comprimido a cada 24 horas', 'Administrar após a alimentação por 5 dias.', 'Dermocalm Vet (fictício)');
INSERT INTO medicamento_petcore ("Id", "Dosagem", "Instrucao", "Nome")
VALUES ('90000000-0000-0000-0000-000000000002', '1/2 comprimido a cada 12 horas', 'Administrar conforme prescrição veterinária.', 'CardioPet (fictício)');

INSERT INTO medico_petcore ("Id", "DataNascimento", "Email", "Especialidade", "Nome", "Senha", "Sexo", "Telefone", "UrlImg")
VALUES ('40000000-0000-0000-0000-000000000001', DATE '1985-08-14', 'helena.duarte@exemplo.test', 'Dermatologia', 'Dra. Helena Duarte', 'SenhaDemo#2026', 'F', '11987654321', 'https://exemplo.test/medicos/helena.jpg');
INSERT INTO medico_petcore ("Id", "DataNascimento", "Email", "Especialidade", "Nome", "Senha", "Sexo", "Telefone", "UrlImg")
VALUES ('40000000-0000-0000-0000-000000000002', DATE '1982-02-20', 'rafael.nogueira@exemplo.test', 'Cardiologia', 'Dr. Rafael Nogueira', 'SenhaDemo#2026', 'M', '11976543210', 'https://exemplo.test/medicos/rafael.jpg');

INSERT INTO pet_petcore ("Id", "DataNasc", "Especie", "Nome", "Pelagem", "Porte", "Raca", "Sexo", "Status", "UrlImg")
VALUES ('50000000-0000-0000-0000-000000000001', DATE '2021-06-18', 'Canina', 'Luna', 'Dourada', 'Grande', 'Golden Retriever', 0, TRUE, 'https://exemplo.test/pets/luna.jpg');
INSERT INTO pet_petcore ("Id", "DataNasc", "Especie", "Nome", "Pelagem", "Porte", "Raca", "Sexo", "Status", "UrlImg")
VALUES ('50000000-0000-0000-0000-000000000002', DATE '2022-09-05', 'Felina', 'Tobias', 'Creme e marrom', 'Pequeno', 'Siamês', 1, TRUE, 'https://exemplo.test/pets/tobias.jpg');

INSERT INTO protocolo_petcore ("Id", "Texto", "Titulo")
VALUES ('PROTO1', 'Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.', 'Preventivo contra picadas');
INSERT INTO protocolo_petcore ("Id", "Texto", "Titulo")
VALUES ('PROTO2', 'A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.', 'Vermifugação');
INSERT INTO protocolo_petcore ("Id", "Texto", "Titulo")
VALUES ('PROTO3', 'FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.', 'FIV e FeLV');

INSERT INTO tutor_petcore ("Id", "DataNascimento", "Email", "Nome", "Senha", "Sexo", "Telefone", "UrlImg")
VALUES ('60000000-0000-0000-0000-000000000001', DATE '1991-11-03', 'marina.alves@exemplo.test', 'Marina Alves', 'SenhaDemo#2026', 'F', '11991234567', 'https://exemplo.test/tutores/marina.jpg');
INSERT INTO tutor_petcore ("Id", "DataNascimento", "Email", "Nome", "Senha", "Sexo", "Telefone", "UrlImg")
VALUES ('60000000-0000-0000-0000-000000000002', DATE '1988-05-25', 'bruno.ferreira@exemplo.test', 'Bruno Ferreira', 'SenhaDemo#2026', 'M', '11992345678', 'https://exemplo.test/tutores/bruno.jpg');

INSERT INTO clinica_petcore ("Id", "Cnpj", "IdEndereco", "Nome")
VALUES ('20000000-0000-0000-0000-000000000001', '12.345.678/0001-90', '10000000-0000-0000-0000-000000000001', 'PetCore Clínica Centro (fictícia)');
INSERT INTO clinica_petcore ("Id", "Cnpj", "IdEndereco", "Nome")
VALUES ('20000000-0000-0000-0000-000000000002', '98.765.432/0001-10', '10000000-0000-0000-0000-000000000002', 'PetCore Vet Jardins (fictícia)');

INSERT INTO historico_petcore ("Id", "DataAbertura", "IdPet", "Status")
VALUES ('30000000-0000-0000-0000-000000000001', DATE '2025-03-12', '50000000-0000-0000-0000-000000000001', TRUE);
INSERT INTO historico_petcore ("Id", "DataAbertura", "IdPet", "Status")
VALUES ('30000000-0000-0000-0000-000000000002', DATE '2025-04-08', '50000000-0000-0000-0000-000000000002', TRUE);

INSERT INTO tut_pet_petcore ("ID_pet_FK", "ID_tut_FK")
VALUES ('50000000-0000-0000-0000-000000000001', '60000000-0000-0000-0000-000000000001');
INSERT INTO tut_pet_petcore ("ID_pet_FK", "ID_tut_FK")
VALUES ('50000000-0000-0000-0000-000000000002', '60000000-0000-0000-0000-000000000002');

INSERT INTO prontuario_petcore ("Id", "DataEmissao", "Descricao", "IdHistorico", "IdMedico")
VALUES ('70000000-0000-0000-0000-000000000001', DATE '2025-05-10', 'Consulta preventiva; pele sem alterações relevantes.', '30000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001');
INSERT INTO prontuario_petcore ("Id", "DataEmissao", "Descricao", "IdHistorico", "IdMedico")
VALUES ('70000000-0000-0000-0000-000000000002', DATE '2025-06-02', 'Avaliação de rotina; ausculta cardíaca normal.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002');

INSERT INTO relatorio_petcore ("Id", "IdHistorico", "IdMedicoResponsavel", "Observacao")
VALUES ('b0000000-0000-0000-0000-000000000001', '30000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'Retorno sugerido em seis meses para acompanhamento preventivo.');
INSERT INTO relatorio_petcore ("Id", "IdHistorico", "IdMedicoResponsavel", "Observacao")
VALUES ('b0000000-0000-0000-0000-000000000002', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002', 'Manter rotina de hidratação e retorno anual.');

INSERT INTO exame_petcore ("Id", "Data", "IdMedico", "IdProntuario", "Nome", "Tipo")
VALUES ('80000000-0000-0000-0000-000000000001', DATE '2025-05-10', '40000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', 'Hemograma da Luna', 'Hemograma completo');
INSERT INTO exame_petcore ("Id", "Data", "IdMedico", "IdProntuario", "Nome", "Tipo")
VALUES ('80000000-0000-0000-0000-000000000002', DATE '2025-06-02', '40000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002', 'Ultrassom do Tobias', 'Ultrassom abdominal');

INSERT INTO receita_petcore ("Id", "IdMedicoResponsavel", "IdProntuario", "Nome", "Validade")
VALUES ('a0000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', 'Receita dermatológica - Luna', DATE '2025-12-10');
INSERT INTO receita_petcore ("Id", "IdMedicoResponsavel", "IdProntuario", "Nome", "Validade")
VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002', 'Receita preventiva - Tobias', DATE '2025-12-02');

INSERT INTO rel_cli_petcore ("ID_cli_FK", "ID_rel_FK")
VALUES ('20000000-0000-0000-0000-000000000001', 'b0000000-0000-0000-0000-000000000001');
INSERT INTO rel_cli_petcore ("ID_cli_FK", "ID_rel_FK")
VALUES ('20000000-0000-0000-0000-000000000002', 'b0000000-0000-0000-0000-000000000002');

INSERT INTO rec_medic_petcore ("ID_medic_FK", "ID_rec_FK")
VALUES ('90000000-0000-0000-0000-000000000001', 'a0000000-0000-0000-0000-000000000001');
INSERT INTO rec_medic_petcore ("ID_medic_FK", "ID_rec_FK")
VALUES ('90000000-0000-0000-0000-000000000002', 'a0000000-0000-0000-0000-000000000002');

CREATE UNIQUE INDEX "IX_clinica_petcore_IdEndereco" ON clinica_petcore ("IdEndereco");

CREATE INDEX "IX_exame_petcore_IdMedico" ON exame_petcore ("IdMedico");

CREATE INDEX "IX_exame_petcore_IdProntuario" ON exame_petcore ("IdProntuario");

CREATE UNIQUE INDEX "IX_historico_petcore_IdPet" ON historico_petcore ("IdPet");

CREATE INDEX "IX_prontuario_petcore_IdHistorico" ON prontuario_petcore ("IdHistorico");

CREATE INDEX "IX_prontuario_petcore_IdMedico" ON prontuario_petcore ("IdMedico");

CREATE INDEX "IX_rec_medic_petcore_ID_rec_FK" ON rec_medic_petcore ("ID_rec_FK");

CREATE INDEX "IX_receita_petcore_IdMedicoResponsavel" ON receita_petcore ("IdMedicoResponsavel");

CREATE INDEX "IX_receita_petcore_IdProntuario" ON receita_petcore ("IdProntuario");

CREATE INDEX "IX_rel_cli_petcore_ID_rel_FK" ON rel_cli_petcore ("ID_rel_FK");

CREATE INDEX "IX_relatorio_petcore_IdHistorico" ON relatorio_petcore ("IdHistorico");

CREATE INDEX "IX_relatorio_petcore_IdMedicoResponsavel" ON relatorio_petcore ("IdMedicoResponsavel");

CREATE INDEX "IX_tut_pet_petcore_ID_pet_FK" ON tut_pet_petcore ("ID_pet_FK");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260912214150_InitialPostgreSQL', '10.0.4');

COMMIT;

START TRANSACTION;
ALTER TABLE pet_petcore ADD "ObitoInformado" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE pet_petcore ADD "ComedouroStatus" character varying(20) NOT NULL DEFAULT 'vazio';

ALTER TABLE prontuario_petcore ALTER COLUMN "IdMedico" TYPE uuid;
ALTER TABLE prontuario_petcore ALTER COLUMN "IdMedico" DROP NOT NULL;

ALTER TABLE prontuario_petcore ALTER COLUMN "IdHistorico" TYPE uuid;
ALTER TABLE prontuario_petcore ALTER COLUMN "IdHistorico" DROP NOT NULL;

ALTER TABLE prontuario_petcore ADD "PetId" uuid;

ALTER TABLE prontuario_petcore ADD "TutorId" uuid;

ALTER TABLE prontuario_petcore ADD "Temperatura" numeric;

ALTER TABLE prontuario_petcore ADD "Peso" numeric;

ALTER TABLE prontuario_petcore ADD "Tratamento" text NOT NULL DEFAULT '';

ALTER TABLE prontuario_petcore ADD "PetNome" text NOT NULL DEFAULT '';

ALTER TABLE prontuario_petcore ADD "TutorNome" text NOT NULL DEFAULT '';

ALTER TABLE exame_petcore ALTER COLUMN "IdProntuario" TYPE uuid;
ALTER TABLE exame_petcore ALTER COLUMN "IdProntuario" DROP NOT NULL;

ALTER TABLE exame_petcore ADD "PetId" uuid;

ALTER TABLE exame_petcore ADD "TutorId" uuid;

ALTER TABLE exame_petcore ADD "PetNome" text NOT NULL DEFAULT '';

ALTER TABLE exame_petcore ADD "TutorNome" text NOT NULL DEFAULT '';

ALTER TABLE exame_petcore ADD "VeterinarioNome" text NOT NULL DEFAULT '';

ALTER TABLE exame_petcore ADD "ArquivoSolicitacao" text NOT NULL DEFAULT '';

ALTER TABLE exame_petcore ADD "DataEnvio" timestamp with time zone;

ALTER TABLE receita_petcore ALTER COLUMN "IdProntuario" TYPE uuid;
ALTER TABLE receita_petcore ALTER COLUMN "IdProntuario" DROP NOT NULL;

ALTER TABLE receita_petcore ALTER COLUMN "Validade" TYPE date;
ALTER TABLE receita_petcore ALTER COLUMN "Validade" DROP NOT NULL;

ALTER TABLE receita_petcore ADD "PetId" uuid;

ALTER TABLE receita_petcore ADD "TutorId" uuid;

ALTER TABLE receita_petcore ADD "PetNome" text NOT NULL DEFAULT '';

ALTER TABLE receita_petcore ADD "TutorNome" text NOT NULL DEFAULT '';

ALTER TABLE receita_petcore ADD "VeterinarioNome" text NOT NULL DEFAULT '';

ALTER TABLE receita_petcore ADD "ArquivoReceita" text NOT NULL DEFAULT '';

ALTER TABLE receita_petcore ADD "DataEnvio" timestamp with time zone;

ALTER TABLE relatorio_petcore ALTER COLUMN "IdHistorico" TYPE uuid;
ALTER TABLE relatorio_petcore ALTER COLUMN "IdHistorico" DROP NOT NULL;

ALTER TABLE relatorio_petcore ADD "PetId" uuid;

ALTER TABLE relatorio_petcore ADD "TutorId" uuid;

ALTER TABLE relatorio_petcore ADD "PetNome" text NOT NULL DEFAULT '';

ALTER TABLE relatorio_petcore ADD "TutorNome" text NOT NULL DEFAULT '';

ALTER TABLE relatorio_petcore ADD "ArquivoHistorico" text NOT NULL DEFAULT '';

ALTER TABLE relatorio_petcore ADD "Prontuarios" text NOT NULL DEFAULT '';

ALTER TABLE protocolo_petcore ADD "TutorId" uuid;

ALTER TABLE protocolo_petcore ADD "PetId" uuid;

ALTER TABLE protocolo_petcore ADD "VeterinarioId" uuid;

ALTER TABLE protocolo_petcore ADD "TutorNome" text NOT NULL DEFAULT '';

ALTER TABLE protocolo_petcore ADD "PetNome" text NOT NULL DEFAULT '';

ALTER TABLE protocolo_petcore ADD "VeterinarioNome" text NOT NULL DEFAULT '';

ALTER TABLE protocolo_petcore ADD "TipoRegistro" character varying(40) NOT NULL DEFAULT 'protocolo';

ALTER TABLE protocolo_petcore ADD "DataEnvio" timestamp with time zone;

ALTER TABLE medico_petcore ADD "Clinica" character varying(200);

ALTER TABLE medico_petcore ADD "NomeClinica" character varying(200);

ALTER TABLE medico_petcore ADD "CnpjClinica" character varying(20);

ALTER TABLE medico_petcore ADD "CepClinica" character varying(20);

ALTER TABLE medico_petcore ADD "ComplementoClinica" character varying(200);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261020160000_AddMobilePetFields', '10.0.4');

COMMIT;

