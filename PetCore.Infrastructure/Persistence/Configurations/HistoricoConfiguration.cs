namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HistoricoConfiguration : IEntityTypeConfiguration<Historico>
{
    public void Configure(EntityTypeBuilder<Historico> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("historico_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.DataAbertura)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .IsRequired();

        // Campos exclusivos do documento Mongo; não pertencem à tabela legada do EF.
        builder.Ignore(x => x.TutorId);
        builder.Ignore(x => x.TutorNome);
        builder.Ignore(x => x.PetNome);
        builder.Ignore(x => x.TipoRegistro);
        builder.Ignore(x => x.StatusMobile);
        builder.Ignore(x => x.DataSolicitacao);

        
        // RELACIONAMENTOS
        //1:N Relatorios (RelatorioConfiguration)
      
        
        //1:1 Pet. O histórico é dependente do pet, permitindo criar o pet primeiro.
        builder.HasOne(x => x.Pet)
            .WithOne(x => x.Historico)
            .HasForeignKey<Historico>(x => x.IdPet)
            .OnDelete(DeleteBehavior.Cascade);

        
        //1:N Prontuario (ProntuarioConfiguration)
        
    }
}
