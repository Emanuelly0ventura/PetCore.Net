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