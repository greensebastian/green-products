using GreenProducts.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddGreenProducts();

var app = builder.Build();
app.AddGreenProducts();

// Normally would not do this on startup, but since there's no release pipeline this is it.
await app.DoDatabaseMigrations();

app.Run();