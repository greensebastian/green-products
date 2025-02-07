using System.Net.Http.Json;
using GreenProducts.Core;
using GreenProducts.Core.Products;
using GreenProducts.WebApi.Infrastructure;
using GreenProducts.WebApi.Products;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GreenProducts.WebApi.Tests;

[Collection(nameof(HostFixtureCollection))]
public class WebApiTests(HostFixture hostFixture)
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
        var response = await TestClient.GetFromJsonAsync<PaginatedResponse<ProductAttribute>>("/products/attributes?pageSize=1000");

        // Assert
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty("Database should be seeded with attributes");
    }

    [Fact]
    public async Task Application_CreateValidProduct_IsCreated()
    {
        // Arrange
        var productTypeId = Guid.Parse(ProductAttributesSeed.Ids.Chair);
        var colourIds = new[]
            { Guid.Parse(ProductAttributesSeed.Ids.Yellow), Guid.Parse(ProductAttributesSeed.Ids.Navy) };
        var request = new CreateProductRequest("LANDSKRONA", productTypeId, colourIds);

        // Act
        var response = await TestClient.PostAsJsonAsync("/products", request);
        var product = await response.Content.ReadFromJsonAsync<Product>();

        var queryResponse = await TestClient.GetAsync($"/products");
        var queryProduct = (await queryResponse.Content.ReadFromJsonAsync<PaginatedResponse<Product>>())!.Items.Single(p => p.Id == product!.Id);

        var getByIdResponse = await TestClient.GetAsync($"/products/{queryProduct.Id}");
        var getByIdProduct = await getByIdResponse.Content.ReadFromJsonAsync<Product>();

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("LANDSKRONA");
        product.ProductType.Id.ShouldBe(productTypeId);
        product.AvailableColours.Select(pc => pc.Id).ShouldBeEquivalentToUnordered(colourIds);

        queryProduct.ShouldBeEquivalentTo(product);
        getByIdProduct.ShouldBeEquivalentTo(product);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "", 400)]
    [InlineData(ProductAttributesSeed.Ids.Chair, "", 400)]
    [InlineData(ProductAttributesSeed.Ids.Chair, ProductAttributesSeed.Ids.Chair, 400)]
    [InlineData(ProductAttributesSeed.Ids.Chair, $"{ProductAttributesSeed.Ids.Chair},{ProductAttributesSeed.Ids.Yellow}", 400)]
    [InlineData(ProductAttributesSeed.Ids.Yellow, ProductAttributesSeed.Ids.Yellow, 400)]
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

    [Fact]
    public async Task Application_GetMissingProduct_Returns404()
    {
        // Arrange

        // Act
        var response = await TestClient.GetAsync("/products/00000000-0000-0000-0000-000000000000");
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        ((int)response.StatusCode).ShouldBe(404);
        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(404);
    }

    [Fact]
    public async Task Application_GetNonGuidProductId_Returns400()
    {
        // Arrange

        // Act
        var response = await TestClient.GetAsync("/products/not-a-guid");
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        ((int)response.StatusCode).ShouldBe(400);
        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(400);
    }

    [Fact]
    public async Task Application_GetProducts_PaginationWorks()
    {
        // Arrange
        var productTypeId = Guid.Parse(ProductAttributesSeed.Ids.Chair);
        var colourIds = new[]
            { Guid.Parse(ProductAttributesSeed.Ids.Yellow), Guid.Parse(ProductAttributesSeed.Ids.Navy) };

        for (var i = 0; i < 15; i++)
        {
            await TestClient.PostAsJsonAsync("/products", new CreateProductRequest($"LANDSKRONA_{i:00}", productTypeId, colourIds));
        }

        // Act
        var firstPageResponse = await TestClient.GetAsync("/products?page=1&pageSize=4");
        var firstPageProducts = await firstPageResponse.Content.ReadFromJsonAsync<PaginatedResponse<Product>>();

        var secondPageResponse = await TestClient.GetAsync("/products?page=2&pageSize=4");
        var secondPageProducts = await secondPageResponse.Content.ReadFromJsonAsync<PaginatedResponse<Product>>();

        // Assert
        firstPageProducts.ShouldNotBeNull();
        firstPageProducts.Items.Count.ShouldBe(4);
        firstPageProducts.Page.ShouldBe(1);
        firstPageProducts.TotalCount.ShouldBeGreaterThanOrEqualTo(15);

        secondPageProducts.ShouldNotBeNull();
        secondPageProducts.Items.Count.ShouldBe(4);
        secondPageProducts.Page.ShouldBe(2);
        secondPageProducts.TotalCount.ShouldBeGreaterThanOrEqualTo(15);

        firstPageProducts.Items
            .Concat(secondPageProducts.Items)
            .Select(p => p.Id)
            .Distinct()
            .Count()
            .ShouldBe(8);
    }
    
    [Fact]
    public async Task Application_CreateDuplicateProductName_Returns400()
    {
        // Arrange
        var productTypeId = Guid.Parse(ProductAttributesSeed.Ids.Chair);
        var colourIds = new[]
            { Guid.Parse(ProductAttributesSeed.Ids.Yellow), Guid.Parse(ProductAttributesSeed.Ids.Navy) };
        var request = new CreateProductRequest("DUPLICATE_NAME", productTypeId, colourIds);
        
        await TestClient.PostAsJsonAsync("/products", request);

        // Act
        var duplicateResponse = await TestClient.PostAsJsonAsync("/products", request);
        var problemDetails = await duplicateResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        ((int)duplicateResponse.StatusCode).ShouldBe(400);
        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(400);
    }
}