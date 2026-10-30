namespace PetCore.Application.Security;

public interface IPasswordHashService
{
    string Hash(string password);
    bool Verify(string password, string storedValue);
    bool IsHash(string storedValue);
}