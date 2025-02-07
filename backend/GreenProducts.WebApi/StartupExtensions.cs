using GreenProducts.Core.Products;
using GreenProducts.WebApi.Infrastructure;
using GreenProducts.WebApi.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi;

/// <summary>
/// Static class containing initialization extensions methods for the web application.
/// Allows for 
/// </summary>
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

        app.MapGet("/products/attributes",
            async ([FromServices] ProductService productService, CancellationToken cancellationToken, int page = 1,
                    int pageSize = 10) =>
                await productService.GetProductAttributes(page, pageSize, cancellationToken));

        return app;
    }

    /// <summary>
    /// Creates a /seed endpoint to populate database with attributes to use when creating products.
    /// Would not expose publicly in a regular application, only used here for simplicity in testing and showing work.
    /// </summary>
    /// <param name="app">WebApplication instance to map the /seed endpoint to</param>
    /// <returns>The WebApplication with the route /seed configured</returns>
    private static WebApplication MapSeedEndpoint(this WebApplication app)
    {
        app.MapPut("/seed",
            async ([FromServices] GreenProductsDbContext context, [FromServices] TimeProvider timeProvider, CancellationToken cancellationToken) =>
            {
                foreach (var attribute in ProductAttributesSeed.Attributes)
                {
                    var existingAttribute = await context.ProductAttributes.SingleOrDefaultAsync(pc => pc.Id == attribute.Id, cancellationToken);
                    if (existingAttribute is not null)
                    {
                        context.ProductAttributes.Update(existingAttribute);
                    }
                    else
                    {
                        context.ProductAttributes.Add(attribute);
                    }
                }
                await context.SaveChangesAsync(cancellationToken);
            });

        return app;
    }
}