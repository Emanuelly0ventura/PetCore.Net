using System.ComponentModel.DataAnnotations;
namespace PetCore.Authentication;

/// <summary>Credenciais para emissão de um token de acesso.</summary>
/// <param name="Email">E-mail cadastrado.</param>
/// <param name="Password">Senha em texto simples enviada pelo cliente.</param>
/// <param name="Role">Perfil de acesso: tutor, medico ou veterinario.</param>
public sealed record LoginRequest(string Email, string Password, string Role);

/// <summary>Link relacionado ao recurso retornado pela API.</summary>
/// <param name="Href">Caminho da operação relacionada.</param>
/// <param name="Rel">Relação do link com o recurso.</param>
/// <param name="Method">Método HTTP da operação.</param>
public sealed record HateoasLink(string Href, string Rel, string Method = "GET");

/// <summary>Resposta paginada de uma coleção.</summary>
/// <param name="Items">Itens da página atual.</param>
/// <param name="Page">Número da página atual.</param>
/// <param name="PageSize">Quantidade máxima de itens por página.</param>
/// <param name="TotalCount">Quantidade total de itens encontrados.</param>
/// <param name="TotalPages">Quantidade total de páginas.</param>
/// <param name="Links">Links de navegação da coleção.</param>
public sealed record PagedResponse<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalCount, int TotalPages, IReadOnlyCollection<HateoasLink> Links);

/// <summary>Resposta de um recurso acompanhada de links relacionados.</summary>
/// <param name="Data">Recurso retornado.</param>
/// <param name="Links">Links de navegação do recurso.</param>
public sealed record ResourceResponse<T>(T Data, IReadOnlyCollection<HateoasLink> Links);

/// <summary>Envelope padronizado para respostas de sucesso da API.</summary>
/// <param name="Data">Recurso ou coleção retornada.</param>
public sealed record ApiResponseEnvelope<T>(T Data);

/// <summary>Envelope paginado que mantém os metadados e links de navegação.</summary>
/// <param name="Items">Itens da página atual.</param>
/// <param name="Page">Número da página.</param>
/// <param name="PageSize">Quantidade máxima de itens.</param>
/// <param name="TotalCount">Quantidade total encontrada.</param>
/// <param name="TotalPages">Quantidade total de páginas.</param>
/// <param name="Links">Links HATEOAS.</param>
public sealed record PagedApiResponse<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalCount, int TotalPages, IReadOnlyCollection<HateoasLink> Links);

/// <summary>Dados de especialização expostos pela consulta pública do catálogo de médicos.</summary>
/// <param name="Especializacao">Nome da especialização do médico.</param>
public sealed record MedicoCatalogResponse(string Especializacao);

/// <summary>Nome do exame exposto pela consulta pública do catálogo de exames.</summary>
/// <param name="Nome">Nome do exame.</param>
public sealed record ExameCatalogResponse(string Nome);

/// <summary>Parâmetros de paginação, ordenação e filtragem de coleções.</summary>
public sealed class PaginationOptions
{
    /// <example>1</example>
    /// <summary>Número da página solicitada.</summary>
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
    /// <example>20</example>
    /// <summary>Quantidade de itens por página.</summary>
    [Range(1, 100)] public int PageSize { get; set; } = 20;
    /// <example>Nome</example>
    /// <summary>Propriedade usada para ordenar a coleção.</summary>
    public string? SortBy { get; set; }
    /// <summary>Indica se a ordenação será decrescente.</summary>
    public bool Descending { get; set; }
}
