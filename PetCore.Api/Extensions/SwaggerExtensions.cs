using System.Reflection;
using Microsoft.OpenApi;

namespace PetCore.Extensions;

public static class SwaggerExtensions
{
     public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
           return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PetCore API",
                    Version = "v1",
                    Description = "API REST para gerenciamento de receitas, exames e medicamentos dos pets para usuario e médico.",
                    TermsOfService = new Uri("https://example.com/petcore/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe PetCore",
                        Email = "contato@example.com",
                        Url = new Uri("https://example.com/petcore")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://example.com/petcore/license")
                    }
                });
    
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
        }
}