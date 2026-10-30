namespace PetCore.Domain.Exceptions;

/// <summary>Represents invalid data rejected by a domain rule.</summary>
public sealed class DomainValidationException(string message) : Exception(message)
{
}