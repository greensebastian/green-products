using System.Net.Http.Json;
using GreenProducts.Core;
using GreenProducts.Core.Products;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GreenProducts.WebApi.Tests;

[Collection(nameof(HostFixtureCollection))]
public class GreenProductsWebApiTests(HostFixture hostFixture)
{
    private HttpClient TestClient => hostFixture.HttpClient;

    [Fact]
    public void HostFixture_Start_Started()
    {
        hostFixture.WebApplication.Lifetime.ApplicationStarted.IsCancellationRequested.ShouldBeTrue("App should be started");
        hostFixture.WebApplication.Lifetime.ApplicationStopped.IsCancellationRequested.ShouldBeFalse("App should not be stopped");
    }

    [Fact]
    public async Task HostFixture_Start_IsSeeded()
    {
        // Arrange

        // Act
        var response = await GetProductClassificationsAsync();

        // Assert
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty("Database should be seeded with classifications");
    }

    [Fact]
    public async Task Application_CreateValidProduct_IsCreated()
    {
        // Arrange
        var classifications = await GetProductClassificationsAsync();
        var productTypeId = classifications.Items.First(pc => pc.Type == ProductClassification.Types.ProductType).Id;
        var colourIds = classifications.Items.Where(pc => pc.Type == ProductClassification.Types.Colour).Select(pc => pc.Id).ToArray();

        // Act
        var response = await TestClient.PostAsJsonAsync("/products", new CreateProductRequest("LANDSKRONA", productTypeId, colourIds));
        var product = await response.Content.ReadFromJsonAsync<Product>();
        
        var queryResponse = await TestClient.GetAsync($"/products");
        var queryProduct = (await queryResponse.Content.ReadFromJsonAsync<PaginatedResponse<Product>>())!.Items.Single(p => p.Id == product!.Id);
        
        var getByIdResponse = await TestClient.GetAsync($"/products/{queryProduct.Id}");
        var getByIdProduct = await getByIdResponse.Content.ReadFromJsonAsync<Product>();

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("LANDSKRONA");
        product.ProductType.Id.ShouldBe(productTypeId);
        product.AvailableColours.Select(pc => pc.Id).ShouldHaveUnordered(colourIds);
        
        queryProduct.ShouldBeEquivalentTo(product);
        getByIdProduct.ShouldBeEquivalentTo(product);
    }
    
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "", 400)]
    public async Task Application_CreateInvalidProduct_ReturnsCorrectStatusCodeAndProblemDetails(string serializedProductTypeId, string commaSeparatedColourIds, int expectedStatusCode)
    {
        // Arrange
        var productTypeId = Guid.Parse(serializedProductTypeId);
        var colourIds = commaSeparatedColourIds.Split(",").Where(s => !string.IsNullOrWhiteSpace(s)).Select(Guid.Parse).ToArray();

        // Act
        var response = await TestClient.PostAsJsonAsync("/products", new CreateProductRequest("INVALID_PRODUCT", productTypeId, colourIds));
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        ((int)response.StatusCode).ShouldBe(expectedStatusCode);
        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(expectedStatusCode);
    }

    private async Task<PaginatedResponse<ProductClassification>> GetProductClassificationsAsync()
    {
        var productClassifications =
            await TestClient.GetFromJsonAsync<PaginatedResponse<ProductClassification>>("/products/classifications?pageSize=1000");

        if (productClassifications is null) throw new ApplicationException("Product classifications could not be fetched");
        return productClassifications;
    }
}