using Microsoft.EntityFrameworkCore;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Implementations;
using PetCore.Application.Services.Interfaces;
using PetCore.Infrastructure.Persistence;
using PetCore.Infrastructure.Repositories;
using MongoDB.Driver;
using PetCore.Infrastructure.Persistence.Mongo;

namespace PetCore.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClinicaService, ClinicaService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IExameService, ExameService>();
        services.AddScoped<IHistoricoService, HistoricoService>();
        services.AddScoped<IMedicamentoService, MedicamentoService>();
        services.AddScoped<IMedicoService, MedicoService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IProntuarioService, ProntuarioService>();
        services.AddScoped<IProtocoloService, ProtocoloService>();
        services.AddScoped<IReceitaService, ReceitaService>();
        services.AddScoped<IRelatorioService, RelatorioService>();
        services.AddScoped<ITutorService, TutorService>();

        return services;
    }

    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IClinicaRepository, ClinicaRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IExameRepository, ExameRepository>();
        services.AddScoped<IHistoricoRepository, MongoHistoricoRepository>();
        services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IProntuarioRepository, ProntuarioRepository>();
        services.AddScoped<IProtocoloRepository, ProtocoloRepository>();
        services.AddScoped<IReceitaRepository, ReceitaRepository>();
        services.AddScoped<IRelatorioRepository, RelatorioRepository>();
        services.AddScoped<ITutorRepository, TutorRepository>();

        return services;
    }


    public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetConnectionString("MongoDb")
            ?? configuration["MongoDb:ConnectionString"];
        if (string.IsNullOrWhiteSpace(mongoConnection))
            throw new InvalidOperationException("A conexão MongoDB não foi configurada. Defina ConnectionStrings__MongoDb.");

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnection));
        services.AddSingleton(provider => provider.GetRequiredService<IMongoClient>()
            .GetDatabase(configuration["MongoDb:Database"] ?? "petcore"));
        services.AddSingleton(provider => provider.GetRequiredService<IMongoDatabase>()
            .GetCollection<HistoricoDocument>(configuration["MongoDb:HistoricosCollection"] ?? "historicos"));

        services.AddDbContext<PetCoreContext>(options =>
        {
  
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "A conexão PostgreSQL não foi configurada. Defina ConnectionStrings__DefaultConnection.");
            }
            options.UseNpgsql(connectionString);
        });
        return services;
    }
}
