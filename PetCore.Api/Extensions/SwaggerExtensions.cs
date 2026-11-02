using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetCore.Extensions;

public static class SwaggerExtensions
{
     public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
           return services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header, Description = "Informe o token JWT." });
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer", document, null)] = new List<string>() });
                options.OperationFilter<AllowAnonymousOperationFilter>();
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

/// <summary>Remove o requisito global Bearer das operações marcadas como anônimas.</summary>
public sealed class AllowAnonymousOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var isAnonymous = context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() is not null
            || context.MethodInfo.DeclaringType?.GetCustomAttribute<AllowAnonymousAttribute>() is not null;

        if (isAnonymous)
            operation.Security = [];
    }
}



