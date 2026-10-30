IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [endereco_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Cep] nvarchar(max) NOT NULL,
        [Complemento] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_endereco_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [medicamento_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Dosagem] nvarchar(max) NOT NULL,
        [Instrucao] nvarchar(max) NOT NULL,
        [Nome] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_medicamento_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [medico_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Especialidade] nvarchar(max) NOT NULL,
        [Clinica] nvarchar(200) NULL,
        [NomeClinica] nvarchar(200) NULL,
        [CnpjClinica] nvarchar(20) NULL,
        [CepClinica] nvarchar(20) NULL,
        [ComplementoClinica] nvarchar(200) NULL,
        [Nome] nvarchar(max) NOT NULL,
        [DataNascimento] date NOT NULL,
        [Telefone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Sexo] nvarchar(1) NOT NULL,
        [Senha] nvarchar(max) NOT NULL,
        [UrlImg] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_medico_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [pet_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Nome] nvarchar(max) NOT NULL,
        [Especie] nvarchar(max) NOT NULL,
        [Raca] nvarchar(max) NOT NULL,
        [DataNasc] date NOT NULL,
        [Pelagem] nvarchar(max) NOT NULL,
        [Porte] nvarchar(max) NOT NULL,
        [Sexo] int NOT NULL,
        [Status] bit NOT NULL,
        [ObitoInformado] bit NOT NULL,
        [ComedouroStatus] nvarchar(20) NOT NULL,
        [UrlImg] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_pet_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [protocolo_petcore] (
        [Id] nvarchar(20) NOT NULL,
        [Titulo] nvarchar(150) NOT NULL,
        [Texto] nvarchar(max) NOT NULL,
        [TutorId] uniqueidentifier NULL,
        [TutorNome] nvarchar(max) NOT NULL,
        [PetId] uniqueidentifier NULL,
        [PetNome] nvarchar(max) NOT NULL,
        [VeterinarioId] uniqueidentifier NULL,
        [VeterinarioNome] nvarchar(max) NOT NULL,
        [TipoRegistro] nvarchar(40) NOT NULL,
        [DataEnvio] datetime2 NULL,
        CONSTRAINT [PK_protocolo_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [tutor_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Nome] nvarchar(max) NOT NULL,
        [DataNascimento] date NOT NULL,
        [Telefone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Sexo] nvarchar(1) NOT NULL,
        [Senha] nvarchar(max) NOT NULL,
        [UrlImg] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_tutor_petcore] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [clinica_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Nome] nvarchar(max) NOT NULL,
        [Cnpj] nvarchar(max) NOT NULL,
        [IdEndereco] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_clinica_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_clinica_petcore_endereco_petcore_IdEndereco] FOREIGN KEY ([IdEndereco]) REFERENCES [endereco_petcore] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [historico_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [DataAbertura] date NOT NULL,
        [Status] bit NOT NULL,
        [IdPet] uniqueidentifier NULL,
        CONSTRAINT [PK_historico_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_historico_petcore_pet_petcore_IdPet] FOREIGN KEY ([IdPet]) REFERENCES [pet_petcore] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [tut_pet_petcore] (
        [ID_tut_FK] uniqueidentifier NOT NULL,
        [ID_pet_FK] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_tut_pet_petcore] PRIMARY KEY ([ID_tut_FK], [ID_pet_FK]),
        CONSTRAINT [FK_tut_pet_petcore_pet_petcore_ID_pet_FK] FOREIGN KEY ([ID_pet_FK]) REFERENCES [pet_petcore] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_tut_pet_petcore_tutor_petcore_ID_tut_FK] FOREIGN KEY ([ID_tut_FK]) REFERENCES [tutor_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [prontuario_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [DataEmissao] date NOT NULL,
        [Descricao] nvarchar(max) NOT NULL,
        [IdHistorico] uniqueidentifier NULL,
        [IdMedico] uniqueidentifier NULL,
        [PetId] uniqueidentifier NULL,
        [TutorId] uniqueidentifier NULL,
        [PetNome] nvarchar(max) NOT NULL,
        [TutorNome] nvarchar(max) NOT NULL,
        [Temperatura] decimal(18,2) NULL,
        [Peso] decimal(18,2) NULL,
        [Tratamento] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_prontuario_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_prontuario_petcore_historico_petcore_IdHistorico] FOREIGN KEY ([IdHistorico]) REFERENCES [historico_petcore] ([Id]),
        CONSTRAINT [FK_prontuario_petcore_medico_petcore_IdMedico] FOREIGN KEY ([IdMedico]) REFERENCES [medico_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [relatorio_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Observacao] nvarchar(max) NOT NULL,
        [IdHistorico] uniqueidentifier NULL,
        [IdMedicoResponsavel] uniqueidentifier NOT NULL,
        [PetId] uniqueidentifier NULL,
        [TutorId] uniqueidentifier NULL,
        [PetNome] nvarchar(max) NOT NULL,
        [TutorNome] nvarchar(max) NOT NULL,
        [ArquivoHistorico] nvarchar(max) NOT NULL,
        [Prontuarios] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_relatorio_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_relatorio_petcore_historico_petcore_IdHistorico] FOREIGN KEY ([IdHistorico]) REFERENCES [historico_petcore] ([Id]),
        CONSTRAINT [FK_relatorio_petcore_medico_petcore_IdMedicoResponsavel] FOREIGN KEY ([IdMedicoResponsavel]) REFERENCES [medico_petcore] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [exame_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Data] date NOT NULL,
        [Tipo] nvarchar(max) NOT NULL,
        [IdMedico] uniqueidentifier NOT NULL,
        [IdProntuario] uniqueidentifier NULL,
        [PetId] uniqueidentifier NULL,
        [TutorId] uniqueidentifier NULL,
        [PetNome] nvarchar(max) NOT NULL,
        [TutorNome] nvarchar(max) NOT NULL,
        [VeterinarioNome] nvarchar(max) NOT NULL,
        [ArquivoSolicitacao] nvarchar(max) NOT NULL,
        [DataEnvio] datetime2 NULL,
        [Nome] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_exame_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_exame_petcore_medico_petcore_IdMedico] FOREIGN KEY ([IdMedico]) REFERENCES [medico_petcore] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_exame_petcore_prontuario_petcore_IdProntuario] FOREIGN KEY ([IdProntuario]) REFERENCES [prontuario_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [receita_petcore] (
        [Id] uniqueidentifier NOT NULL,
        [Validade] date NULL,
        [IdMedicoResponsavel] uniqueidentifier NOT NULL,
        [IdProntuario] uniqueidentifier NULL,
        [PetId] uniqueidentifier NULL,
        [TutorId] uniqueidentifier NULL,
        [PetNome] nvarchar(max) NOT NULL,
        [TutorNome] nvarchar(max) NOT NULL,
        [VeterinarioNome] nvarchar(max) NOT NULL,
        [ArquivoReceita] nvarchar(max) NOT NULL,
        [DataEnvio] datetime2 NULL,
        [Nome] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_receita_petcore] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_receita_petcore_medico_petcore_IdMedicoResponsavel] FOREIGN KEY ([IdMedicoResponsavel]) REFERENCES [medico_petcore] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_receita_petcore_prontuario_petcore_IdProntuario] FOREIGN KEY ([IdProntuario]) REFERENCES [prontuario_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [rel_cli_petcore] (
        [ID_cli_FK] uniqueidentifier NOT NULL,
        [ID_rel_FK] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_rel_cli_petcore] PRIMARY KEY ([ID_cli_FK], [ID_rel_FK]),
        CONSTRAINT [FK_rel_cli_petcore_clinica_petcore_ID_cli_FK] FOREIGN KEY ([ID_cli_FK]) REFERENCES [clinica_petcore] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_rel_cli_petcore_relatorio_petcore_ID_rel_FK] FOREIGN KEY ([ID_rel_FK]) REFERENCES [relatorio_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE TABLE [rec_medic_petcore] (
        [ID_medic_FK] uniqueidentifier NOT NULL,
        [ID_rec_FK] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_rec_medic_petcore] PRIMARY KEY ([ID_medic_FK], [ID_rec_FK]),
        CONSTRAINT [FK_rec_medic_petcore_medicamento_petcore_ID_medic_FK] FOREIGN KEY ([ID_medic_FK]) REFERENCES [medicamento_petcore] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_rec_medic_petcore_receita_petcore_ID_rec_FK] FOREIGN KEY ([ID_rec_FK]) REFERENCES [receita_petcore] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cep', N'Complemento') AND [object_id] = OBJECT_ID(N'[endereco_petcore]'))
        SET IDENTITY_INSERT [endereco_petcore] ON;
    EXEC(N'INSERT INTO [endereco_petcore] ([Id], [Cep], [Complemento])
    VALUES (''10000000-0000-0000-0000-000000000001'', N''01001-000'', N''Sala 12 - dados fictícios''),
    (''10000000-0000-0000-0000-000000000002'', N''01415-002'', N''Bloco B - dados fictícios'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cep', N'Complemento') AND [object_id] = OBJECT_ID(N'[endereco_petcore]'))
        SET IDENTITY_INSERT [endereco_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Dosagem', N'Instrucao', N'Nome') AND [object_id] = OBJECT_ID(N'[medicamento_petcore]'))
        SET IDENTITY_INSERT [medicamento_petcore] ON;
    EXEC(N'INSERT INTO [medicamento_petcore] ([Id], [Dosagem], [Instrucao], [Nome])
    VALUES (''90000000-0000-0000-0000-000000000001'', N''1 comprimido a cada 24 horas'', N''Administrar após a alimentação por 5 dias.'', N''Dermocalm Vet (fictício)''),
    (''90000000-0000-0000-0000-000000000002'', N''1/2 comprimido a cada 12 horas'', N''Administrar conforme prescrição veterinária.'', N''CardioPet (fictício)'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Dosagem', N'Instrucao', N'Nome') AND [object_id] = OBJECT_ID(N'[medicamento_petcore]'))
        SET IDENTITY_INSERT [medicamento_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CepClinica', N'Clinica', N'CnpjClinica', N'ComplementoClinica', N'DataNascimento', N'Email', N'Especialidade', N'Nome', N'NomeClinica', N'Senha', N'Sexo', N'Telefone', N'UrlImg') AND [object_id] = OBJECT_ID(N'[medico_petcore]'))
        SET IDENTITY_INSERT [medico_petcore] ON;
    EXEC(N'INSERT INTO [medico_petcore] ([Id], [CepClinica], [Clinica], [CnpjClinica], [ComplementoClinica], [DataNascimento], [Email], [Especialidade], [Nome], [NomeClinica], [Senha], [Sexo], [Telefone], [UrlImg])
    VALUES (''40000000-0000-0000-0000-000000000001'', NULL, NULL, NULL, NULL, ''1985-08-14'', N''helena.duarte@exemplo.test'', N''Dermatologia'', N''Dra. Helena Duarte'', NULL, N''SenhaDemo#2026'', N''F'', N''11987654321'', N''https://exemplo.test/medicos/helena.jpg''),
    (''40000000-0000-0000-0000-000000000002'', NULL, NULL, NULL, NULL, ''1982-02-20'', N''rafael.nogueira@exemplo.test'', N''Cardiologia'', N''Dr. Rafael Nogueira'', NULL, N''SenhaDemo#2026'', N''M'', N''11976543210'', N''https://exemplo.test/medicos/rafael.jpg'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CepClinica', N'Clinica', N'CnpjClinica', N'ComplementoClinica', N'DataNascimento', N'Email', N'Especialidade', N'Nome', N'NomeClinica', N'Senha', N'Sexo', N'Telefone', N'UrlImg') AND [object_id] = OBJECT_ID(N'[medico_petcore]'))
        SET IDENTITY_INSERT [medico_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ComedouroStatus', N'DataNasc', N'Especie', N'Nome', N'ObitoInformado', N'Pelagem', N'Porte', N'Raca', N'Sexo', N'Status', N'UrlImg') AND [object_id] = OBJECT_ID(N'[pet_petcore]'))
        SET IDENTITY_INSERT [pet_petcore] ON;
    EXEC(N'INSERT INTO [pet_petcore] ([Id], [ComedouroStatus], [DataNasc], [Especie], [Nome], [ObitoInformado], [Pelagem], [Porte], [Raca], [Sexo], [Status], [UrlImg])
    VALUES (''50000000-0000-0000-0000-000000000001'', N''vazio'', ''2021-06-18'', N''Canina'', N''Luna'', CAST(0 AS bit), N''Dourada'', N''Grande'', N''Golden Retriever'', 0, CAST(1 AS bit), N''https://exemplo.test/pets/luna.jpg''),
    (''50000000-0000-0000-0000-000000000002'', N''vazio'', ''2022-09-05'', N''Felina'', N''Tobias'', CAST(0 AS bit), N''Creme e marrom'', N''Pequeno'', N''Siamês'', 1, CAST(1 AS bit), N''https://exemplo.test/pets/tobias.jpg'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ComedouroStatus', N'DataNasc', N'Especie', N'Nome', N'ObitoInformado', N'Pelagem', N'Porte', N'Raca', N'Sexo', N'Status', N'UrlImg') AND [object_id] = OBJECT_ID(N'[pet_petcore]'))
        SET IDENTITY_INSERT [pet_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataEnvio', N'PetId', N'PetNome', N'Texto', N'TipoRegistro', N'Titulo', N'TutorId', N'TutorNome', N'VeterinarioId', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[protocolo_petcore]'))
        SET IDENTITY_INSERT [protocolo_petcore] ON;
    EXEC(N'INSERT INTO [protocolo_petcore] ([Id], [DataEnvio], [PetId], [PetNome], [Texto], [TipoRegistro], [Titulo], [TutorId], [TutorNome], [VeterinarioId], [VeterinarioNome])
    VALUES (N''PROTO1'', NULL, NULL, N'''', N''Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário.'', N''protocolo'', N''Preventivo contra picadas'', NULL, N'''', NULL, N''''),
    (N''PROTO2'', NULL, NULL, N'''', N''A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição.'', N''protocolo'', N''Vermifugação'', NULL, N'''', NULL, N''''),
    (N''PROTO3'', NULL, NULL, N'''', N''FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos.'', N''protocolo'', N''FIV e FeLV'', NULL, N'''', NULL, N'''')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataEnvio', N'PetId', N'PetNome', N'Texto', N'TipoRegistro', N'Titulo', N'TutorId', N'TutorNome', N'VeterinarioId', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[protocolo_petcore]'))
        SET IDENTITY_INSERT [protocolo_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataNascimento', N'Email', N'Nome', N'Senha', N'Sexo', N'Telefone', N'UrlImg') AND [object_id] = OBJECT_ID(N'[tutor_petcore]'))
        SET IDENTITY_INSERT [tutor_petcore] ON;
    EXEC(N'INSERT INTO [tutor_petcore] ([Id], [DataNascimento], [Email], [Nome], [Senha], [Sexo], [Telefone], [UrlImg])
    VALUES (''60000000-0000-0000-0000-000000000001'', ''1991-11-03'', N''marina.alves@exemplo.test'', N''Marina Alves'', N''SenhaDemo#2026'', N''F'', N''11991234567'', N''https://exemplo.test/tutores/marina.jpg''),
    (''60000000-0000-0000-0000-000000000002'', ''1988-05-25'', N''bruno.ferreira@exemplo.test'', N''Bruno Ferreira'', N''SenhaDemo#2026'', N''M'', N''11992345678'', N''https://exemplo.test/tutores/bruno.jpg'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataNascimento', N'Email', N'Nome', N'Senha', N'Sexo', N'Telefone', N'UrlImg') AND [object_id] = OBJECT_ID(N'[tutor_petcore]'))
        SET IDENTITY_INSERT [tutor_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cnpj', N'IdEndereco', N'Nome') AND [object_id] = OBJECT_ID(N'[clinica_petcore]'))
        SET IDENTITY_INSERT [clinica_petcore] ON;
    EXEC(N'INSERT INTO [clinica_petcore] ([Id], [Cnpj], [IdEndereco], [Nome])
    VALUES (''20000000-0000-0000-0000-000000000001'', N''12.345.678/0001-90'', ''10000000-0000-0000-0000-000000000001'', N''PetCore Clínica Centro (fictícia)''),
    (''20000000-0000-0000-0000-000000000002'', N''98.765.432/0001-10'', ''10000000-0000-0000-0000-000000000002'', N''PetCore Vet Jardins (fictícia)'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cnpj', N'IdEndereco', N'Nome') AND [object_id] = OBJECT_ID(N'[clinica_petcore]'))
        SET IDENTITY_INSERT [clinica_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataAbertura', N'IdPet', N'Status') AND [object_id] = OBJECT_ID(N'[historico_petcore]'))
        SET IDENTITY_INSERT [historico_petcore] ON;
    EXEC(N'INSERT INTO [historico_petcore] ([Id], [DataAbertura], [IdPet], [Status])
    VALUES (''30000000-0000-0000-0000-000000000001'', ''2025-03-12'', ''50000000-0000-0000-0000-000000000001'', CAST(1 AS bit)),
    (''30000000-0000-0000-0000-000000000002'', ''2025-04-08'', ''50000000-0000-0000-0000-000000000002'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataAbertura', N'IdPet', N'Status') AND [object_id] = OBJECT_ID(N'[historico_petcore]'))
        SET IDENTITY_INSERT [historico_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_pet_FK', N'ID_tut_FK') AND [object_id] = OBJECT_ID(N'[tut_pet_petcore]'))
        SET IDENTITY_INSERT [tut_pet_petcore] ON;
    EXEC(N'INSERT INTO [tut_pet_petcore] ([ID_pet_FK], [ID_tut_FK])
    VALUES (''50000000-0000-0000-0000-000000000001'', ''60000000-0000-0000-0000-000000000001''),
    (''50000000-0000-0000-0000-000000000002'', ''60000000-0000-0000-0000-000000000002'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_pet_FK', N'ID_tut_FK') AND [object_id] = OBJECT_ID(N'[tut_pet_petcore]'))
        SET IDENTITY_INSERT [tut_pet_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataEmissao', N'Descricao', N'IdHistorico', N'IdMedico', N'Peso', N'PetId', N'PetNome', N'Temperatura', N'Tratamento', N'TutorId', N'TutorNome') AND [object_id] = OBJECT_ID(N'[prontuario_petcore]'))
        SET IDENTITY_INSERT [prontuario_petcore] ON;
    EXEC(N'INSERT INTO [prontuario_petcore] ([Id], [DataEmissao], [Descricao], [IdHistorico], [IdMedico], [Peso], [PetId], [PetNome], [Temperatura], [Tratamento], [TutorId], [TutorNome])
    VALUES (''70000000-0000-0000-0000-000000000001'', ''2025-05-10'', N''Consulta preventiva; pele sem alterações relevantes.'', ''30000000-0000-0000-0000-000000000001'', ''40000000-0000-0000-0000-000000000001'', NULL, ''50000000-0000-0000-0000-000000000001'', N''Luna'', NULL, N'''', ''60000000-0000-0000-0000-000000000001'', N''Marina Alves''),
    (''70000000-0000-0000-0000-000000000002'', ''2025-06-02'', N''Avaliação de rotina; ausculta cardíaca normal.'', ''30000000-0000-0000-0000-000000000002'', ''40000000-0000-0000-0000-000000000002'', NULL, ''50000000-0000-0000-0000-000000000002'', N''Tobias'', NULL, N'''', ''60000000-0000-0000-0000-000000000002'', N''Bruno Ferreira'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataEmissao', N'Descricao', N'IdHistorico', N'IdMedico', N'Peso', N'PetId', N'PetNome', N'Temperatura', N'Tratamento', N'TutorId', N'TutorNome') AND [object_id] = OBJECT_ID(N'[prontuario_petcore]'))
        SET IDENTITY_INSERT [prontuario_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoHistorico', N'IdHistorico', N'IdMedicoResponsavel', N'Observacao', N'PetId', N'PetNome', N'Prontuarios', N'TutorId', N'TutorNome') AND [object_id] = OBJECT_ID(N'[relatorio_petcore]'))
        SET IDENTITY_INSERT [relatorio_petcore] ON;
    EXEC(N'INSERT INTO [relatorio_petcore] ([Id], [ArquivoHistorico], [IdHistorico], [IdMedicoResponsavel], [Observacao], [PetId], [PetNome], [Prontuarios], [TutorId], [TutorNome])
    VALUES (''b0000000-0000-0000-0000-000000000001'', N'''', ''30000000-0000-0000-0000-000000000001'', ''40000000-0000-0000-0000-000000000001'', N''Retorno sugerido em seis meses para acompanhamento preventivo.'', ''50000000-0000-0000-0000-000000000001'', N''Luna'', N'''', ''60000000-0000-0000-0000-000000000001'', N''Marina Alves''),
    (''b0000000-0000-0000-0000-000000000002'', N'''', ''30000000-0000-0000-0000-000000000002'', ''40000000-0000-0000-0000-000000000002'', N''Manter rotina de hidratação e retorno anual.'', ''50000000-0000-0000-0000-000000000002'', N''Tobias'', N'''', ''60000000-0000-0000-0000-000000000002'', N''Bruno Ferreira'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoHistorico', N'IdHistorico', N'IdMedicoResponsavel', N'Observacao', N'PetId', N'PetNome', N'Prontuarios', N'TutorId', N'TutorNome') AND [object_id] = OBJECT_ID(N'[relatorio_petcore]'))
        SET IDENTITY_INSERT [relatorio_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoSolicitacao', N'Data', N'DataEnvio', N'IdMedico', N'IdProntuario', N'Nome', N'PetId', N'PetNome', N'Tipo', N'TutorId', N'TutorNome', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[exame_petcore]'))
        SET IDENTITY_INSERT [exame_petcore] ON;
    EXEC(N'INSERT INTO [exame_petcore] ([Id], [ArquivoSolicitacao], [Data], [DataEnvio], [IdMedico], [IdProntuario], [Nome], [PetId], [PetNome], [Tipo], [TutorId], [TutorNome], [VeterinarioNome])
    VALUES (''80000000-0000-0000-0000-000000000001'', N'''', ''2025-05-10'', NULL, ''40000000-0000-0000-0000-000000000001'', ''70000000-0000-0000-0000-000000000001'', N''Hemograma da Luna'', ''50000000-0000-0000-0000-000000000001'', N''Luna'', N''Hemograma completo'', ''60000000-0000-0000-0000-000000000001'', N''Marina Alves'', N''Dra. Helena Duarte''),
    (''80000000-0000-0000-0000-000000000002'', N'''', ''2025-06-02'', NULL, ''40000000-0000-0000-0000-000000000002'', ''70000000-0000-0000-0000-000000000002'', N''Ultrassom do Tobias'', ''50000000-0000-0000-0000-000000000002'', N''Tobias'', N''Ultrassom abdominal'', ''60000000-0000-0000-0000-000000000002'', N''Bruno Ferreira'', N''Dr. Rafael Nogueira'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoSolicitacao', N'Data', N'DataEnvio', N'IdMedico', N'IdProntuario', N'Nome', N'PetId', N'PetNome', N'Tipo', N'TutorId', N'TutorNome', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[exame_petcore]'))
        SET IDENTITY_INSERT [exame_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoReceita', N'DataEnvio', N'IdMedicoResponsavel', N'IdProntuario', N'Nome', N'PetId', N'PetNome', N'TutorId', N'TutorNome', N'Validade', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[receita_petcore]'))
        SET IDENTITY_INSERT [receita_petcore] ON;
    EXEC(N'INSERT INTO [receita_petcore] ([Id], [ArquivoReceita], [DataEnvio], [IdMedicoResponsavel], [IdProntuario], [Nome], [PetId], [PetNome], [TutorId], [TutorNome], [Validade], [VeterinarioNome])
    VALUES (''a0000000-0000-0000-0000-000000000001'', N'''', NULL, ''40000000-0000-0000-0000-000000000001'', ''70000000-0000-0000-0000-000000000001'', N''Receita dermatológica - Luna'', ''50000000-0000-0000-0000-000000000001'', N''Luna'', ''60000000-0000-0000-0000-000000000001'', N''Marina Alves'', ''2025-12-10'', N''Dra. Helena Duarte''),
    (''a0000000-0000-0000-0000-000000000002'', N'''', NULL, ''40000000-0000-0000-0000-000000000002'', ''70000000-0000-0000-0000-000000000002'', N''Receita preventiva - Tobias'', ''50000000-0000-0000-0000-000000000002'', N''Tobias'', ''60000000-0000-0000-0000-000000000002'', N''Bruno Ferreira'', ''2025-12-02'', N''Dr. Rafael Nogueira'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArquivoReceita', N'DataEnvio', N'IdMedicoResponsavel', N'IdProntuario', N'Nome', N'PetId', N'PetNome', N'TutorId', N'TutorNome', N'Validade', N'VeterinarioNome') AND [object_id] = OBJECT_ID(N'[receita_petcore]'))
        SET IDENTITY_INSERT [receita_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_cli_FK', N'ID_rel_FK') AND [object_id] = OBJECT_ID(N'[rel_cli_petcore]'))
        SET IDENTITY_INSERT [rel_cli_petcore] ON;
    EXEC(N'INSERT INTO [rel_cli_petcore] ([ID_cli_FK], [ID_rel_FK])
    VALUES (''20000000-0000-0000-0000-000000000001'', ''b0000000-0000-0000-0000-000000000001''),
    (''20000000-0000-0000-0000-000000000002'', ''b0000000-0000-0000-0000-000000000002'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_cli_FK', N'ID_rel_FK') AND [object_id] = OBJECT_ID(N'[rel_cli_petcore]'))
        SET IDENTITY_INSERT [rel_cli_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_medic_FK', N'ID_rec_FK') AND [object_id] = OBJECT_ID(N'[rec_medic_petcore]'))
        SET IDENTITY_INSERT [rec_medic_petcore] ON;
    EXEC(N'INSERT INTO [rec_medic_petcore] ([ID_medic_FK], [ID_rec_FK])
    VALUES (''90000000-0000-0000-0000-000000000001'', ''a0000000-0000-0000-0000-000000000001''),
    (''90000000-0000-0000-0000-000000000002'', ''a0000000-0000-0000-0000-000000000002'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID_medic_FK', N'ID_rec_FK') AND [object_id] = OBJECT_ID(N'[rec_medic_petcore]'))
        SET IDENTITY_INSERT [rec_medic_petcore] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE UNIQUE INDEX [IX_clinica_petcore_IdEndereco] ON [clinica_petcore] ([IdEndereco]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_exame_petcore_IdMedico] ON [exame_petcore] ([IdMedico]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_exame_petcore_IdProntuario] ON [exame_petcore] ([IdProntuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_historico_petcore_IdPet] ON [historico_petcore] ([IdPet]) WHERE [IdPet] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_prontuario_petcore_IdHistorico] ON [prontuario_petcore] ([IdHistorico]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_prontuario_petcore_IdMedico] ON [prontuario_petcore] ([IdMedico]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_rec_medic_petcore_ID_rec_FK] ON [rec_medic_petcore] ([ID_rec_FK]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_receita_petcore_IdMedicoResponsavel] ON [receita_petcore] ([IdMedicoResponsavel]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_receita_petcore_IdProntuario] ON [receita_petcore] ([IdProntuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_rel_cli_petcore_ID_rel_FK] ON [rel_cli_petcore] ([ID_rel_FK]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_relatorio_petcore_IdHistorico] ON [relatorio_petcore] ([IdHistorico]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_relatorio_petcore_IdMedicoResponsavel] ON [relatorio_petcore] ([IdMedicoResponsavel]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    CREATE INDEX [IX_tut_pet_petcore_ID_pet_FK] ON [tut_pet_petcore] ([ID_pet_FK]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029220333_InitialSqlServer'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261029220333_InitialSqlServer', N'10.0.5');
END;

COMMIT;
GO

