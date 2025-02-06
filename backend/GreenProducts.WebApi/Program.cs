using GreenProducts.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddGreenProducts();

var app = builder.Build();
app.AddGreenProducts();

app.Run();