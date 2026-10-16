using Xunit;

namespace PetCore.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class ApiCollection : ICollectionFixture<PetCoreApiFactory>
{
    public const string Name = "PetCore API";
}
