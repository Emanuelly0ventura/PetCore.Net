using Microsoft.EntityFrameworkCore;
using PetCore.Domain.Entities;

namespace PetCore.Infrastructure.Persistence;

public class PetCoreContext(DbContextOptions<PetCoreContext> options) : DbContext(options)
{
    public DbSet<Clinica> Clinicas { get; set; }
    public DbSet<Endereco> Endereco { get; set; }
    public DbSet<Exame> Exame { get; set; }
    public DbSet<Historico> Historico { get; set; }
    public DbSet<Medicamento> Medicamento { get; set; }
    public DbSet<Medico> Medico { get; set; }
    public DbSet<Pet> Pet { get; set; }
    public DbSet<Protocolo> Protocolo { get; set; }
    public DbSet<Prontuario> Prontuario { get; set; }
    public DbSet<Receita> Receita { get; set; }
    public DbSet<Relatorio> Relatorio { get; set; }
    public DbSet<Tutor> Tutor { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetCoreContext).Assembly);
        DemoData.Apply(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
