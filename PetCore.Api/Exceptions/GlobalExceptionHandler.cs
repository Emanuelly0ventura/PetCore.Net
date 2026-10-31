using PetCore.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace PetCore.Exceptions;

public class GlobalExceptionHandler(IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Log.Error(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        var (statusCode, title, detail) = MapException(environment, exception);
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = httpContext.Request.Path
        };
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static (int StatusCode, string Title, string? Detail) MapException(IHostEnvironment environment, Exception exception) => exception switch
    {
        DomainValidationException e => (StatusCodes.Status400BadRequest, "Dados inválidos", e.Message),
        ArgumentNullException e => (StatusCodes.Status400BadRequest, "Requisição inválida", e.Message),
        ArgumentException e => (StatusCodes.Status400BadRequest, "Requisição inválida", e.Message),
        KeyNotFoundException e => (StatusCodes.Status404NotFound, "Recurso não encontrado", e.Message),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Não autorizado", "Credenciais inválidas ou ausentes."),
        InvalidOperationException e => (StatusCodes.Status400BadRequest, "Não foi possível concluir a operação", e.Message),
        DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation or PostgresErrorCodes.ForeignKeyViolation } } => (StatusCodes.Status409Conflict, "Conflito de dados", "Já existe um registro com os mesmos dados ou a operação viola um relacionamento."),
        DbUpdateException { InnerException: NpgsqlException } => (StatusCodes.Status503ServiceUnavailable, "Banco de dados indisponível", "Tente novamente mais tarde."),
        DbUpdateException => (StatusCodes.Status409Conflict, "Conflito de dados", "Não foi possível salvar os dados por uma restrição de integridade."),
        NpgsqlException => (StatusCodes.Status503ServiceUnavailable, "Banco de dados indisponível", "Tente novamente mais tarde."),
        _ when environment.IsDevelopment() => (StatusCodes.Status500InternalServerError, "Erro interno do servidor", exception.ToString()),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor", "Ocorreu um erro inesperado. Tente novamente mais tarde.")
    };
}
