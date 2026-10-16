using Microsoft.EntityFrameworkCore;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Infrastructure.Persistence;

/// <summary>Dados totalmente fictícios para demonstração local da API.</summary>
internal static class DemoData
{
    private static readonly Guid EnderecoCentro = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid EnderecoJardins = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid ClinicaCentro = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid ClinicaJardins = Guid.Parse("20000000-0000-0000-0000-000000000002");
    private static readonly Guid HistoricoLuna = Guid.Parse("30000000-0000-0000-0000-000000000001");
    private static readonly Guid HistoricoTobias = Guid.Parse("30000000-0000-0000-0000-000000000002");
    private static readonly Guid MedicoHelena = Guid.Parse("40000000-0000-0000-0000-000000000001");
    private static readonly Guid MedicoRafael = Guid.Parse("40000000-0000-0000-0000-000000000002");
    private static readonly Guid PetLuna = Guid.Parse("50000000-0000-0000-0000-000000000001");
    private static readonly Guid PetTobias = Guid.Parse("50000000-0000-0000-0000-000000000002");
    private static readonly Guid TutorMarina = Guid.Parse("60000000-0000-0000-0000-000000000001");
    private static readonly Guid TutorBruno = Guid.Parse("60000000-0000-0000-0000-000000000002");
    private static readonly Guid ProntuarioLuna = Guid.Parse("70000000-0000-0000-0000-000000000001");
    private static readonly Guid ProntuarioTobias = Guid.Parse("70000000-0000-0000-0000-000000000002");
    private static readonly Guid ExameLuna = Guid.Parse("80000000-0000-0000-0000-000000000001");
    private static readonly Guid ExameTobias = Guid.Parse("80000000-0000-0000-0000-000000000002");
    private static readonly Guid MedicamentoLuna = Guid.Parse("90000000-0000-0000-0000-000000000001");
    private static readonly Guid MedicamentoTobias = Guid.Parse("90000000-0000-0000-0000-000000000002");
    private static readonly Guid ReceitaLuna = Guid.Parse("a0000000-0000-0000-0000-000000000001");
    private static readonly Guid ReceitaTobias = Guid.Parse("a0000000-0000-0000-0000-000000000002");
    private static readonly Guid RelatorioLuna = Guid.Parse("b0000000-0000-0000-0000-000000000001");
    private static readonly Guid RelatorioTobias = Guid.Parse("b0000000-0000-0000-0000-000000000002");

    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Endereco>().HasData(
            new { Id = EnderecoCentro, Cep = "01001-000", Complemento = "Sala 12 - dados fictícios" },
            new { Id = EnderecoJardins, Cep = "01415-002", Complemento = "Bloco B - dados fictícios" });

        modelBuilder.Entity<Clinica>().HasData(
            new { Id = ClinicaCentro, Nome = "PetCore Clínica Centro (fictícia)", Cnpj = "12.345.678/0001-90", IdEndereco = EnderecoCentro },
            new { Id = ClinicaJardins, Nome = "PetCore Vet Jardins (fictícia)", Cnpj = "98.765.432/0001-10", IdEndereco = EnderecoJardins });

        modelBuilder.Entity<Historico>().HasData(
            new { Id = HistoricoLuna, DataAbertura = new DateOnly(2025, 3, 12), Status = true, IdPet = PetLuna },
            new { Id = HistoricoTobias, DataAbertura = new DateOnly(2025, 4, 8), Status = true, IdPet = PetTobias });

        modelBuilder.Entity<Medico>().HasData(
            new { Id = MedicoHelena, Nome = "Dra. Helena Duarte", DataNascimento = new DateOnly(1985, 8, 14), Telefone = "11987654321", Email = "helena.duarte@exemplo.test", Sexo = GeneroSexoEnum.F, Senha = "SenhaDemo#2026", UrlImg = "https://exemplo.test/medicos/helena.jpg", Especialidade = "Dermatologia" },
            new { Id = MedicoRafael, Nome = "Dr. Rafael Nogueira", DataNascimento = new DateOnly(1982, 2, 20), Telefone = "11976543210", Email = "rafael.nogueira@exemplo.test", Sexo = GeneroSexoEnum.M, Senha = "SenhaDemo#2026", UrlImg = "https://exemplo.test/medicos/rafael.jpg", Especialidade = "Cardiologia" });

        modelBuilder.Entity<Pet>().HasData(
            new { Id = PetLuna, Nome = "Luna", Especie = "Canina", Raca = "Golden Retriever", DataNasc = new DateOnly(2021, 6, 18), Pelagem = "Dourada", Porte = "Grande", Sexo = GeneroSexoEnum.F, Status = true, UrlImg = "https://exemplo.test/pets/luna.jpg" },
            new { Id = PetTobias, Nome = "Tobias", Especie = "Felina", Raca = "Siamês", DataNasc = new DateOnly(2022, 9, 5), Pelagem = "Creme e marrom", Porte = "Pequeno", Sexo = GeneroSexoEnum.M, Status = true, UrlImg = "https://exemplo.test/pets/tobias.jpg" });

        modelBuilder.Entity<Tutor>().HasData(
            new { Id = TutorMarina, Nome = "Marina Alves", DataNascimento = new DateOnly(1991, 11, 3), Telefone = "11991234567", Email = "marina.alves@exemplo.test", Sexo = GeneroSexoEnum.F, Senha = "SenhaDemo#2026", UrlImg = "https://exemplo.test/tutores/marina.jpg" },
            new { Id = TutorBruno, Nome = "Bruno Ferreira", DataNascimento = new DateOnly(1988, 5, 25), Telefone = "11992345678", Email = "bruno.ferreira@exemplo.test", Sexo = GeneroSexoEnum.M, Senha = "SenhaDemo#2026", UrlImg = "https://exemplo.test/tutores/bruno.jpg" });

        modelBuilder.Entity<Prontuario>().HasData(
            new { Id = ProntuarioLuna, DataEmissao = new DateOnly(2025, 5, 10), Descricao = "Consulta preventiva; pele sem alterações relevantes.", IdMedico = MedicoHelena, IdHistorico = HistoricoLuna },
            new { Id = ProntuarioTobias, DataEmissao = new DateOnly(2025, 6, 2), Descricao = "Avaliação de rotina; ausculta cardíaca normal.", IdMedico = MedicoRafael, IdHistorico = HistoricoTobias });

        modelBuilder.Entity<Exame>().HasData(
            new { Id = ExameLuna, Nome = "Hemograma da Luna", Data = new DateOnly(2025, 5, 10), Tipo = "Hemograma completo", IdMedico = MedicoHelena, IdProntuario = ProntuarioLuna },
            new { Id = ExameTobias, Nome = "Ultrassom do Tobias", Data = new DateOnly(2025, 6, 2), Tipo = "Ultrassom abdominal", IdMedico = MedicoRafael, IdProntuario = ProntuarioTobias });

        modelBuilder.Entity<Medicamento>().HasData(
            new { Id = MedicamentoLuna, Nome = "Dermocalm Vet (fictício)", Dosagem = "1 comprimido a cada 24 horas", Instrucao = "Administrar após a alimentação por 5 dias." },
            new { Id = MedicamentoTobias, Nome = "CardioPet (fictício)", Dosagem = "1/2 comprimido a cada 12 horas", Instrucao = "Administrar conforme prescrição veterinária." });

        modelBuilder.Entity<Receita>().HasData(
            new { Id = ReceitaLuna, Nome = "Receita dermatológica - Luna", Validade = new DateOnly(2025, 12, 10), IdMedicoResponsavel = MedicoHelena, IdProntuario = ProntuarioLuna },
            new { Id = ReceitaTobias, Nome = "Receita preventiva - Tobias", Validade = new DateOnly(2025, 12, 2), IdMedicoResponsavel = MedicoRafael, IdProntuario = ProntuarioTobias });

        modelBuilder.Entity<Relatorio>().HasData(
            new { Id = RelatorioLuna, Observacao = "Retorno sugerido em seis meses para acompanhamento preventivo.", IdHistorico = HistoricoLuna, IdMedicoResponsavel = MedicoHelena },
            new { Id = RelatorioTobias, Observacao = "Manter rotina de hidratação e retorno anual.", IdHistorico = HistoricoTobias, IdMedicoResponsavel = MedicoRafael });

        modelBuilder.Entity<Protocolo>().HasData(
            new { Id = "PROTO1", Titulo = "Preventivo contra picadas", Texto = "Pulgas, carrapatos e mosquitos podem transmitir doenças importantes aos pets. A prevenção pode ser feita com produtos tópicos, comprimidos ou coleiras específicas. Alguns produtos têm aplicação mensal, enquanto outros podem proteger por até três meses, conforme orientação do médico veterinário." },
            new { Id = "PROTO2", Titulo = "Vermifugação", Texto = "A vermifugação ajuda a prevenir parasitas intestinais que podem causar perda de peso, diarreia, anemia e outros problemas. A frequência varia conforme idade, ambiente, hábitos do pet e risco de exposição." },
            new { Id = "PROTO3", Titulo = "FIV e FeLV", Texto = "FIV e FeLV são doenças virais que acometem gatos. A testagem é importante, especialmente em felinos resgatados, com acesso à rua ou que convivem com outros gatos." });

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("tut_pet_petcore").HasData(
            new { ID_tut_FK = TutorMarina, ID_pet_FK = PetLuna },
            new { ID_tut_FK = TutorBruno, ID_pet_FK = PetTobias });
        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("rec_medic_petcore").HasData(
            new { ID_medic_FK = MedicamentoLuna, ID_rec_FK = ReceitaLuna },
            new { ID_medic_FK = MedicamentoTobias, ID_rec_FK = ReceitaTobias });
        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("rel_cli_petcore").HasData(
            new { ID_cli_FK = ClinicaCentro, ID_rel_FK = RelatorioLuna },
            new { ID_cli_FK = ClinicaJardins, ID_rel_FK = RelatorioTobias });
    }
}
