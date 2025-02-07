﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace GreenProducts.WebApi.Tests;

[CollectionDefinition(nameof(HostFixtureCollection))]
public class HostFixtureCollection : ICollectionFixture<HostFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

/// <summary>
/// Reusable fixture which:
/// Launches and migrates a postgres instance in docker.
/// Launches the application and configures it to connect to said postgres instance.
/// Migrates database and seeds with basic entities.
/// </summary>
public class HostFixture : IAsyncLifetime
{
    // Skipping nullability check here as it is created as part of the async lifetime
    public WebApplication WebApplication { get; private set; } = null!;
    public HttpClient HttpClient { get; private set; } = null!;
    private PostgreSqlContainer PostgreSqlContainer { get; } = new PostgreSqlBuilder()
        .WithDatabase("green_products")
        .WithUsername("root")
        .WithPassword("password")
        .Build();

    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();

        // Initialize application in-process, with override for postgres connection settings
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "ConnectionStrings:GreenProductsDbContext", PostgreSqlContainer.GetConnectionString() }
        });
        builder.AddGreenProducts();
        builder.Services.AddHttpClient();

        WebApplication = builder.Build();
        WebApplication.AddGreenProducts();

        // Apply any migrations to ephemeral database
        await using var scope = WebApplication.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GreenProductsDbContext>();
        await dbContext.Database.MigrateAsync();

        await WebApplication.StartAsync();

        // Seed database with seed endpoint
        HttpClient = WebApplication.Services.GetRequiredService<HttpClient>();
        HttpClient.BaseAddress = new Uri(WebApplication.Urls.First());
        await HttpClient.PutAsync("/seed", new StringContent(string.Empty));
    }

    public async Task DisposeAsync()
    {
        await WebApplication.StopAsync();
        await WebApplication.DisposeAsync();
        await PostgreSqlContainer.StopAsync();
        await PostgreSqlContainer.DisposeAsync();
    }
}