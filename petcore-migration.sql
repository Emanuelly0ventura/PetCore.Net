CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `endereco_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Cep` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Complemento` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_endereco_petcore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `historico_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `DataAbertura` date NOT NULL,
    `Status` tinyint(1) NOT NULL,
    CONSTRAINT `PK_historico_petcore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `medicamento_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Dosagem` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Instrucao` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_medicamento_petcore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `medico_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Especialidade` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DataNascimento` date NOT NULL,
    `Telefone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Sexo` varchar(1) CHARACTER SET utf8mb4 NOT NULL,
    `Senha` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UrlImg` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_medico_petcore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `tutor_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DataNascimento` date NOT NULL,
    `Telefone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Sexo` varchar(1) CHARACTER SET utf8mb4 NOT NULL,
    `Senha` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UrlImg` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_tutor_petcore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `clinica_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Cnpj` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdEndereco` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_clinica_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_clinica_petcore_endereco_petcore_IdEndereco` FOREIGN KEY (`IdEndereco`) REFERENCES `endereco_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `pet_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Especie` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Raca` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DataNasc` date NOT NULL,
    `Pelagem` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Porte` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Sexo` int NOT NULL,
    `Status` tinyint(1) NOT NULL,
    `UrlImg` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdHistorico` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_pet_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_pet_petcore_historico_petcore_IdHistorico` FOREIGN KEY (`IdHistorico`) REFERENCES `historico_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `prontuario_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `DataEmissao` date NOT NULL,
    `Descricao` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdHistorico` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdMedico` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_prontuario_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_prontuario_petcore_historico_petcore_IdHistorico` FOREIGN KEY (`IdHistorico`) REFERENCES `historico_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_prontuario_petcore_medico_petcore_IdMedico` FOREIGN KEY (`IdMedico`) REFERENCES `medico_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `relatorio_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Observacao` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdHistorico` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdMedicoResponsavel` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_relatorio_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_relatorio_petcore_historico_petcore_IdHistorico` FOREIGN KEY (`IdHistorico`) REFERENCES `historico_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_relatorio_petcore_medico_petcore_IdMedicoResponsavel` FOREIGN KEY (`IdMedicoResponsavel`) REFERENCES `medico_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `tut_pet_petcore` (
    `ID_tut_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    `ID_pet_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_tut_pet_petcore` PRIMARY KEY (`ID_tut_FK`, `ID_pet_FK`),
    CONSTRAINT `FK_tut_pet_petcore_pet_petcore_ID_pet_FK` FOREIGN KEY (`ID_pet_FK`) REFERENCES `pet_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_tut_pet_petcore_tutor_petcore_ID_tut_FK` FOREIGN KEY (`ID_tut_FK`) REFERENCES `tutor_petcore` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `exame_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Data` date NOT NULL,
    `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdMedico` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdProntuario` char(36) COLLATE ascii_general_ci NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_exame_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_exame_petcore_medico_petcore_IdMedico` FOREIGN KEY (`IdMedico`) REFERENCES `medico_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_exame_petcore_prontuario_petcore_IdProntuario` FOREIGN KEY (`IdProntuario`) REFERENCES `prontuario_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `receita_petcore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Validade` date NOT NULL,
    `IdMedicoResponsavel` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdProntuario` char(36) COLLATE ascii_general_ci NOT NULL,
    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_receita_petcore` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_receita_petcore_medico_petcore_IdMedicoResponsavel` FOREIGN KEY (`IdMedicoResponsavel`) REFERENCES `medico_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_receita_petcore_prontuario_petcore_IdProntuario` FOREIGN KEY (`IdProntuario`) REFERENCES `prontuario_petcore` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `rel_cli_petcore` (
    `ID_cli_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    `ID_rel_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_rel_cli_petcore` PRIMARY KEY (`ID_cli_FK`, `ID_rel_FK`),
    CONSTRAINT `FK_rel_cli_petcore_clinica_petcore_ID_cli_FK` FOREIGN KEY (`ID_cli_FK`) REFERENCES `clinica_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_rel_cli_petcore_relatorio_petcore_ID_rel_FK` FOREIGN KEY (`ID_rel_FK`) REFERENCES `relatorio_petcore` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `rec_medic_petcore` (
    `ID_medic_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    `ID_rec_FK` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_rec_medic_petcore` PRIMARY KEY (`ID_medic_FK`, `ID_rec_FK`),
    CONSTRAINT `FK_rec_medic_petcore_medicamento_petcore_ID_medic_FK` FOREIGN KEY (`ID_medic_FK`) REFERENCES `medicamento_petcore` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_rec_medic_petcore_receita_petcore_ID_rec_FK` FOREIGN KEY (`ID_rec_FK`) REFERENCES `receita_petcore` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_clinica_petcore_IdEndereco` ON `clinica_petcore` (`IdEndereco`);

CREATE INDEX `IX_exame_petcore_IdMedico` ON `exame_petcore` (`IdMedico`);

CREATE INDEX `IX_exame_petcore_IdProntuario` ON `exame_petcore` (`IdProntuario`);

CREATE UNIQUE INDEX `IX_pet_petcore_IdHistorico` ON `pet_petcore` (`IdHistorico`);

CREATE INDEX `IX_prontuario_petcore_IdHistorico` ON `prontuario_petcore` (`IdHistorico`);

CREATE INDEX `IX_prontuario_petcore_IdMedico` ON `prontuario_petcore` (`IdMedico`);

CREATE INDEX `IX_rec_medic_petcore_ID_rec_FK` ON `rec_medic_petcore` (`ID_rec_FK`);

CREATE INDEX `IX_receita_petcore_IdMedicoResponsavel` ON `receita_petcore` (`IdMedicoResponsavel`);

CREATE INDEX `IX_receita_petcore_IdProntuario` ON `receita_petcore` (`IdProntuario`);

CREATE INDEX `IX_rel_cli_petcore_ID_rel_FK` ON `rel_cli_petcore` (`ID_rel_FK`);

CREATE INDEX `IX_relatorio_petcore_IdHistorico` ON `relatorio_petcore` (`IdHistorico`);

CREATE INDEX `IX_relatorio_petcore_IdMedicoResponsavel` ON `relatorio_petcore` (`IdMedicoResponsavel`);

CREATE INDEX `IX_tut_pet_petcore_ID_pet_FK` ON `tut_pet_petcore` (`ID_pet_FK`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260522013558_Initial', '9.0.14');

COMMIT;

