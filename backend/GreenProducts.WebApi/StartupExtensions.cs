﻿using GreenProducts.Core.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddGreenProducts(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services.AddDbContextPool<GreenProductsDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("GreenProductsDbContext")));

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ProductService>();

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ProductExceptionHandler>();

        return builder;
    }

    public static WebApplication AddGreenProducts(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
        
        // Map domain exceptions to corresponding status codes.
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.MapProductEndpoints();
        app.MapSeedEndpoint();
        return app;
    }

    private static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products");
        
        app.MapPost("/products",
            async ([FromServices] ProductService productService, CancellationToken cancellationToken,
                CreateProductRequest createProductRequest) => await productService.AddProduct(
                createProductRequest.Name, createProductRequest.ProductTypeId, createProductRequest.ColourIds,
                cancellationToken));

        app.MapGet("/products",
            async ([FromServices] ProductService productService, CancellationToken cancellationToken, int page = 1,
                    int pageSize = 10) =>
                await productService.GetProducts(page, pageSize, cancellationToken));

        app.MapGet("/products/{id}",
            async ([FromServices] ProductService productService, CancellationToken cancellationToken, Guid id) =>
            await productService.GetProductDetails(id, cancellationToken));

        app.MapGet("/products/classifications",
            async ([FromServices] ProductService productService, CancellationToken cancellationToken, int page = 1,
                    int pageSize = 10) =>
                await productService.GetProductClassifications(page, pageSize, cancellationToken));

        return app;
    }

    /// <summary>
    /// Creates a /seed endpoint to populate database with classifications to use when creating products.
    /// Would not expose publicly in a regular application, only used here for simplicity in testing and showing work.
    /// </summary>
    /// <param name="app">WebApplication instance to map the /seed endpoint to</param>
    /// <returns>The WebApplication with the route /seed configured</returns>
    private static WebApplication MapSeedEndpoint(this WebApplication app)
    {
        app.MapPut("/seed",
            async ([FromServices] GreenProductsDbContext context, [FromServices] TimeProvider timeProvider, CancellationToken cancellationToken) =>
            {
                var classifications = new List<ProductClassification>
                {
                    new()
                    {
                        Id = Guid.Parse("1d4fe195-622e-46c6-ae47-39e7d83154a9"),
                        Type = ProductClassification.Types.ProductType,
                        Value = "CHAIR",
                        DisplayName = "Chair",
                        CreatedOn = timeProvider.GetUtcNow()
                    },
                    new()
                    {
                        Id = Guid.Parse("b5c64c7f-7055-4aa0-b8a4-14568fcff4fd"),
                        Type = ProductClassification.Types.ProductType,
                        Value = "PLANT",
                        DisplayName = "Plant",
                        CreatedOn = timeProvider.GetUtcNow()
                    },
                    new()
                    {
                        Id = Guid.Parse("71b707ab-2ab3-42b6-8e29-ba91f19181f9"),
                        Type = ProductClassification.Types.ProductType,
                        Value = "DECORATION",
                        DisplayName = "Decoration",
                        CreatedOn = timeProvider.GetUtcNow()
                    },
                    new()
                    {
                        Id = Guid.Parse("a6d1a6fe-73fa-4eef-88d0-c1cd58f87f6e"),
                        Type = ProductClassification.Types.Colour,
                        Value = "BIRCH",
                        DisplayName = "Birch tree",
                        CreatedOn = timeProvider.GetUtcNow()
                    },
                    new()
                    {
                        Id = Guid.Parse("ac5d2362-6e12-4ff7-9b21-c2dc164b8d14"),
                        Type = ProductClassification.Types.Colour,
                        Value = "NAVY",
                        DisplayName = "Navy blue",
                        CreatedOn = timeProvider.GetUtcNow()
                    },
                    new()
                    {
                        Id = Guid.Parse("0f2b7812-766a-41f9-a928-b6c259706d7f"),
                        Type = ProductClassification.Types.Colour,
                        Value = "YELLOW",
                        DisplayName = "Yellow",
                        CreatedOn = timeProvider.GetUtcNow()
                    }
                };
                foreach (var classification in classifications)
                {
                    var existingClassification = await context.ProductClassifications.SingleOrDefaultAsync(pc => pc.Id == classification.Id, cancellationToken);
                    if (existingClassification is not null)
                    {
                        context.ProductClassifications.Update(existingClassification);
                    }
                    else
                    {
                        context.ProductClassifications.Add(classification);
                    }
                }
                await context.SaveChangesAsync(cancellationToken);
            });

        return app;
    }
}

public record CreateProductRequest(string Name, Guid ProductTypeId, ICollection<Guid> ColourIds);