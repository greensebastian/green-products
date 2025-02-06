namespace GreenProducts.WebApi;

public static class ApplicationInitializationExtensions
{
    public static WebApplicationBuilder AddGreenProducts(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        return builder;
    }

    public static WebApplication AddGreenProducts(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        return app;
    }
}