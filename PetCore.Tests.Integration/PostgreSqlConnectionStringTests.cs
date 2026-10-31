using Npgsql;
using PetCore.Configuration;
using Xunit;

namespace PetCore.Tests.Integration;

public class PostgreSqlConnectionStringTests
{
    [Fact]
    public void Normalize_RenderUri_ProducesNpgsqlConnectionStringWithTls()
    {
        var normalized = PostgreSqlConnectionString.Normalize("postgresql://pet%40core:p%24ss%3Aword@db.example.com:5432/petcore?sslmode=require");
        var builder = new NpgsqlConnectionStringBuilder(normalized);

        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal(5432, builder.Port);
        Assert.Equal("pet@core", builder.Username);
        Assert.Equal("p$ss:word", builder.Password);
        Assert.Equal("petcore", builder.Database);
        Assert.Equal(SslMode.Require, builder.SslMode);
    }

    [Fact]
    public void Normalize_KeywordConnectionString_PreservesExistingFormat()
    {
        const string connectionString = "Host=localhost;Username=test;Password=secret;Database=petcore";

        Assert.Equal(connectionString, PostgreSqlConnectionString.Normalize(connectionString));
    }
}